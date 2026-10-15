// Rx examples with R3 (Cysharp), applied to problems from our inventory/chest/health discussions.
// Requires: R3 from NuGetForUnity + the R3.Unity package (see the R3 README). Minimum Unity 2021.3.
//
// Operator names used here (Select, Where, CombineLatest, Merge, ThrottleFirst, Chunk, AsUnitObservable,
// ToReadOnlyReactiveProperty, AddTo) are from R3. Skip is standard Rx; verify it in R3's operator reference.
// AddTo(this) on a Component comes from the R3.Unity assembly: your asmdef must reference R3.Unity.

using System;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.Examples.Rx
{
    public readonly struct DamageInfo
    {
        public readonly int Amount;
        public readonly Vector2 Direction;

        public DamageInfo(int amount, Vector2 direction)
        {
            Amount = amount;
            Direction = direction;
        }
    }

    // =====================================================================================
    // 1. LEVELS AND EDGES AS FIRST-CLASS CONCEPTS
    //
    // ReactiveProperty = a level: it holds a current value and REPLAYS it to every new
    //                    subscriber, so late subscribers and loaded saves start correct.
    // Subject          = an edge: it only notifies about things that happen after you subscribe.
    // =====================================================================================

    public sealed class Health : IDisposable
    {
        // Mutable privately, read-only publicly: nobody outside can set HP and skip the rules.
        readonly ReactiveProperty<int> _current;
        readonly ReactiveProperty<int> _max;
        readonly Subject<DamageInfo> _damaged = new();

        // Levels
        public ReadOnlyReactiveProperty<int> Current => _current;
        public ReadOnlyReactiveProperty<int> Max => _max;
        public ReadOnlyReactiveProperty<float> Fraction { get; }
        public ReadOnlyReactiveProperty<bool> IsDead { get; }

        // Edges
        public Observable<DamageInfo> Damaged => _damaged;
        public Observable<Unit> Died { get; }

        public Health(int max)
        {
            _max = new ReactiveProperty<int>(max);
            _current = new ReactiveProperty<int>(max);

            // DERIVED levels: declared once, always consistent with their sources.
            // No "remember to update the fraction when max changes" bugs.
            Fraction = _current.CombineLatest(_max, (current, maximum) => (float)current / maximum)
                               .ToReadOnlyReactiveProperty();
            IsDead = _current.Select(current => current <= 0).ToReadOnlyReactiveProperty();

            // An edge derived from a level. Skip(1) is ESSENTIAL: a ReactiveProperty replays its
            // current value on subscribe, so without it a dead enemy loaded from a save would
            // fire Died the moment anything subscribes, and play its death scream on load.
            Died = IsDead.Skip(1).Where(dead => dead).AsUnitObservable();
        }

        public void TakeDamage(DamageInfo info)
        {
            if (_current.Value <= 0)
                return;

            _current.Value = Math.Max(0, _current.Value - info.Amount); // level
            _damaged.OnNext(info);                                       // edge
        }

        /// For loading a save: changes the level, fires no Damaged edge.
        public void Restore(int current) => _current.Value = Math.Clamp(current, 0, _max.Value);

        public void Dispose()
        {
            // R3 subjects send OnCompleted on dispose, which ends every downstream subscription,
            // including the derived properties above.
            Disposable.Dispose(_current, _max, _damaged);
        }
    }

    // =====================================================================================
    // 2. NO STARTUP-ORDER BUGS
    //
    // The bar subscribes whenever it likes and immediately receives the current fraction.
    // Compare with "push in Start", which breaks if the listener subscribes later.
    // =====================================================================================

    public sealed class HealthBarView : MonoBehaviour
    {
        [SerializeField] Image _fill;

        public void Bind(Health health)
        {
            health.Fraction
                  .Subscribe(fraction => _fill.fillAmount = fraction)
                  .AddTo(this); // 3. LIFETIME: unsubscribed automatically when this object is destroyed
        }
    }

    // =====================================================================================
    // 4. TIME AS AN OPERATOR
    //
    // Things that would otherwise need timers, flags and Update() code become one line each.
    // =====================================================================================

    public sealed class HitFeedback : MonoBehaviour
    {
        [SerializeField] AudioSource _hitSound;

        public void Bind(Health health, Rigidbody2D body)
        {
            // Five hits in one frame from a shotgun: play the hit sound at most every 0.1 s.
            health.Damaged
                  .ThrottleFirst(TimeSpan.FromSeconds(0.1))
                  .Subscribe(_ => _hitSound.Play())
                  .AddTo(this);

            // Rapid hits: sum everything within 0.25 s into ONE damage number instead of five.
            health.Damaged
                  .Chunk(TimeSpan.FromSeconds(0.25))
                  .Where(batch => batch.Length > 0)
                  .Subscribe(batch =>
                  {
                      int total = 0;
                      foreach (var hit in batch)
                          total += hit.Amount;
                      ShowDamageNumber(total);
                  })
                  .AddTo(this);

            // The adapter idea from our discussion: the source emits FACTS (DamageInfo);
            // Select turns a fact into what one effect needs (a knockback direction).
            health.Damaged
                  .Select(hit => hit.Direction)
                  .Subscribe(direction => body.AddForce(direction * 5f, ForceMode2D.Impulse))
                  .AddTo(this);

            // Death edge: never fires on load, thanks to Skip(1) in Health.
            health.Died
                  .Subscribe(_ => Debug.Log($"{name} died"))
                  .AddTo(this);
        }

        void ShowDamageNumber(int total) => Debug.Log($"-{total}");
    }

    // =====================================================================================
    // 5. COMBINING LEVELS — the problem UnityEvents couldn't solve
    //
    // "Show the popup while the player is near AND the chest can still be opened."
    // =====================================================================================

    public sealed class ChestPopupPresenter : MonoBehaviour
    {
        [SerializeField] GameObject _popup;

        // These would live on the zone and the chest; inlined here to keep the example short.
        readonly ReactiveProperty<int> _playersInZone = new(0);
        readonly ReactiveProperty<bool> _openable = new(true);

        void Start()
        {
            var playerNear = _playersInZone.Select(count => count > 0); // occupant COUNT, so two
                                                                        // overlapping colliders can't
                                                                        // break enter/exit pairing
            playerNear
                .CombineLatest(_openable, (near, canOpen) => near && canOpen)
                .Subscribe(visible => _popup.SetActive(visible)) // correct immediately, and on every change
                .AddTo(this);
        }

        void OnTriggerEnter2D(Collider2D other) => _playersInZone.Value++;
        void OnTriggerExit2D(Collider2D other) => _playersInZone.Value--;

        public void MarkOpened() => _openable.Value = false;

        void OnDestroy() => Disposable.Dispose(_playersInZone, _openable);
    }

    // =====================================================================================
    // 6. MERGING SOURCES
    //
    // "Shake the camera when ANY enemy in this room is hit" — without the camera knowing enemies.
    // =====================================================================================

    public sealed class RoomCameraShake : MonoBehaviour
    {
        public void Bind(Health[] enemiesInRoom)
        {
            var anyHit = new Observable<Unit>[enemiesInRoom.Length];
            for (int i = 0; i < enemiesInRoom.Length; i++)
                anyHit[i] = enemiesInRoom[i].Damaged.AsUnitObservable();

            Observable.Merge(anyHit)
                      .ThrottleFirst(TimeSpan.FromSeconds(0.2))
                      .Subscribe(_ => Shake())
                      .AddTo(this);
        }

        void Shake() => Debug.Log("shake");
    }
}