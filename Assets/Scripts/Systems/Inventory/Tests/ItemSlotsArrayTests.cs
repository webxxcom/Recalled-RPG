using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.Systems.Inventory.Tests
{
    /// <summary>
    /// EditMode tests for ItemSlotsArray.
    ///
    /// Contract under test:
    ///   bool Add(ItemInstance)        — fills what it can. Returns false ONLY when capacity is insufficient;
    ///                                   on false the incoming instance's Count is reduced to the leftover.
    ///                                   Invalid input throws.
    ///   int  Add(ItemDefinition, int) — partial; adds what fits and returns the remainder. Invalid input throws.
    ///   bool Take(ItemDefinition, int)— atomic; takes all or nothing.
    ///
    /// Tests marked "ASSUMPTION" encode behaviour not stated in the contract —
    /// if your intended behaviour differs, change the assertion, not the production code.
    /// </summary>
    public class ItemSlotsArrayTests
    {
        private const int Capacity = 4;
        private const int StackableMax = 999;
        private const int NonStackableMax = 1;

        private readonly List<ItemDefinition> _createdDefinitions = new();

        private ItemDefinition _potion; // stackable
        private ItemDefinition _sword;  // non-stackable

        // ================================================================ setup / helpers

        [SetUp]
        public void SetUp()
        {
            _potion = CreateDefinition(StackableMax);
            _sword = CreateDefinition(NonStackableMax);
        }

        [TearDown]
        public void TearDown()
        {
            // ScriptableObject.CreateInstance creates real UnityEngine.Objects — destroy them
            // so tests don't leak objects into the editor session.
            foreach (var definition in _createdDefinitions)
            {
                if (definition != null)
                    UnityEngine.Object.DestroyImmediate(definition);
            }
            _createdDefinitions.Clear();
        }

        private ItemDefinition CreateDefinition(int maxStockSize)
        {
            var definition = ScriptableObject.CreateInstance<ItemDefinition>();
            definition.MaxStockSize = maxStockSize;
            _createdDefinitions.Add(definition);
            return definition;
        }

        // Built directly instead of via ItemDefinition.CreateInstance so these tests
        // only exercise ItemSlotsArray, not the factory method.
        private static ItemInstance Instance(ItemDefinition definition, int count = 1) =>
            new(definition, count);

        private ItemInstance[] FullOfSwords(int capacity = Capacity) =>
            Enumerable.Range(0, capacity).Select(_ => Instance(_sword)).ToArray();

        private static int EmptySlotCount(ItemSlotsArray slots) =>
            slots.Count(slot => slot.IsEmpty);

        private static int OccupiedSlotCount(ItemSlotsArray slots, ItemDefinition definition) =>
            slots.Count(slot => !slot.IsEmpty && slot.Item.Definition == definition);

        private static int TotalCountOf(ItemSlotsArray slots, ItemDefinition definition) =>
            slots.Where(slot => !slot.IsEmpty && slot.Item.Definition == definition)
                 .Sum(slot => slot.Item.Count);

        private static bool ContainsReference(ItemSlotsArray slots, ItemInstance instance) =>
            slots.Any(slot => ReferenceEquals(slot.Item, instance));

        /// <summary>
        /// Structural invariants that must hold after ANY operation, successful or not.
        /// Call it at the end of every test that mutates — it catches bugs the test's
        /// own assertions weren't looking for.
        /// </summary>
        private static void AssertInvariants(ItemSlotsArray slots)
        {
            // Not HashSet + ReferenceEqualityComparer: that comparer is .NET 5+ and may not
            // exist in Unity's class library. Linear check is fine at inventory sizes.
            var seen = new List<ItemInstance>();

            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];

                Assert.That(slot.IsEmpty, Is.EqualTo(slot.Item == null),
                    $"slot {i}: IsEmpty disagrees with Item");

                if (slot.IsEmpty)
                    continue;

                Assert.That(slot.Item.Definition != null, Is.True,
                    $"slot {i}: item without a definition");
                Assert.That(slot.Item.Count, Is.GreaterThan(0),
                    $"slot {i}: occupied slot with non-positive count");
                Assert.That(slot.Item.Count, Is.LessThanOrEqualTo(slot.Item.Definition.MaxStockSize),
                    $"slot {i}: stack exceeds MaxStockSize");
                Assert.That(seen.Any(other => ReferenceEquals(other, slot.Item)), Is.False,
                    $"slot {i}: same ItemInstance referenced by more than one slot");
                seen.Add(slot.Item);
            }
        }

        /// <summary>Subscribes a counter to SlotsChanged and returns a reader for it.</summary>
        private static Func<int> CountSlotsChanged(ItemSlotsArray slots)
        {
            int raised = 0;
            slots.SlotsChanged += () => raised++;
            return () => raised;
        }

        // ================================================================ ctor(capacity)

        [Test]
        public void Ctor_WithCapacity_CreatesThatManySlots()
        {
            var slots = new ItemSlotsArray(Capacity);

            Assert.That(slots.Count, Is.EqualTo(Capacity));
        }

        [Test]
        public void Ctor_WithCapacity_AllSlotsAreEmpty()
        {
            var slots = new ItemSlotsArray(Capacity);

            Assert.That(EmptySlotCount(slots), Is.EqualTo(Capacity));
            AssertInvariants(slots);
        }

        // ASSUMPTION: a non-positive capacity is a programming error and throws.
        [TestCase(0)]
        [TestCase(-1)]
        public void Ctor_WithNonPositiveCapacity_Throws(int capacity)
        {
            Assert.That(() => new ItemSlotsArray(capacity), Throws.InstanceOf<ArgumentException>());
        }

        // ================================================================ ctor(capacity, loadout)

        [Test]
        public void Ctor_WithLoadout_PlacesItemsInOrder_AndLeavesRestEmpty()
        {
            var sword = Instance(_sword);
            var potions = Instance(_potion, 5);

            var slots = new ItemSlotsArray(Capacity, new[] { sword, potions });

            // SameAs = reference identity: the loadout instance itself is stored, not a copy.
            Assert.That(slots[0].Item, Is.SameAs(sword));
            Assert.That(slots[1].Item, Is.SameAs(potions));
            Assert.That(slots.Skip(2).All(slot => slot.IsEmpty), Is.True);
            AssertInvariants(slots);
        }

        [Test]
        public void Ctor_WithEmptyLoadout_AllSlotsAreEmpty()
        {
            var slots = new ItemSlotsArray(Capacity, Array.Empty<ItemInstance>());

            Assert.That(slots.Count, Is.EqualTo(Capacity));
            Assert.That(EmptySlotCount(slots), Is.EqualTo(Capacity));
        }

        [Test]
        public void Ctor_WithLoadout_DoesNotKeepReferenceToTheArray()
        {
            var original = Instance(_sword);
            var loadout = new[] { original };
            var slots = new ItemSlotsArray(Capacity, loadout);

            loadout[0] = Instance(_sword); // caller mutates its own array afterwards

            Assert.That(slots[0].Item, Is.SameAs(original));
        }

        // ASSUMPTION: a loadout that doesn't fit is a configuration error and throws
        // rather than silently dropping items.
        [Test]
        public void Ctor_WithLoadoutLargerThanCapacity_Throws()
        {
            var loadout = FullOfSwords(Capacity + 1);

            Assert.That(() => new ItemSlotsArray(Capacity, loadout), Throws.InstanceOf<ArgumentException>());
        }

        // ASSUMPTION: null loadout throws.
        [Test]
        public void Ctor_WithNullLoadout_Throws()
        {
            Assert.That(() => new ItemSlotsArray(Capacity, null), Throws.InstanceOf<ArgumentNullException>());
        }

        // ================================================================ bool Add(ItemInstance)

        [Test]
        public void AddInstance_NonStackable_ReturnsTrue_AndStoresSameInstanceInFirstSlot()
        {
            var slots = new ItemSlotsArray(Capacity);
            var sword = Instance(_sword);

            bool added = slots.Add(sword);

            Assert.That(added, Is.True);
            Assert.That(slots[0].Item, Is.SameAs(sword));
            AssertInvariants(slots);
        }

        [Test]
        public void AddInstance_NonStackableTwice_UsesSeparateSlots()
        {
            var slots = new ItemSlotsArray(Capacity);
            var first = Instance(_sword);
            var second = Instance(_sword);

            slots.Add(first);
            slots.Add(second);

            Assert.That(slots[0].Item, Is.SameAs(first));
            Assert.That(slots[1].Item, Is.SameAs(second));
            AssertInvariants(slots);
        }

        [Test]
        public void AddInstance_FillsFirstEmptySlot_EvenWhenItIsAGap()
        {
            var a = Instance(_sword);
            var b = Instance(_sword);
            var c = Instance(_sword);
            var slots = new ItemSlotsArray(Capacity, new[] { a, b, c });
            slots.Remove(b); // gap at index 1

            var incoming = Instance(_sword);
            slots.Add(incoming);

            Assert.That(slots[1].Item, Is.SameAs(incoming));
            AssertInvariants(slots);
        }

        [Test]
        public void AddInstance_Stackable_MergesIntoExistingStack()
        {
            var slots = new ItemSlotsArray(Capacity);

            slots.Add(Instance(_potion, 3));
            bool added = slots.Add(Instance(_potion, 2));

            Assert.That(added, Is.True);
            Assert.That(OccupiedSlotCount(slots, _potion), Is.EqualTo(1));
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(5));
            AssertInvariants(slots);
        }

        [Test]
        public void AddInstance_StackableBeyondMaxStack_OverflowsIntoNextSlot()
        {
            var slots = new ItemSlotsArray(Capacity);

            slots.Add(Instance(_potion, StackableMax));
            bool added = slots.Add(Instance(_potion, 1));

            Assert.That(added, Is.True);
            Assert.That(OccupiedSlotCount(slots, _potion), Is.EqualTo(2));
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(StackableMax + 1));
            AssertInvariants(slots);
        }

        [Test]
        public void AddInstance_StackableFitsExactlyAcrossStackAndEmptySlot_ReturnsTrue()
        {
            var existing = Instance(_potion, StackableMax - 9);
            var slots = new ItemSlotsArray(2, new[] { existing });

            bool added = slots.Add(Instance(_potion, 20)); // 9 top up, 11 into the empty slot

            Assert.That(added, Is.True);
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(StackableMax + 11));
            AssertInvariants(slots);
        }

        [Test]
        public void AddInstance_StackablePartlyFits_ReturnsFalse_AddsWhatFits_AndLeavesLeftoverOnIncoming()
        {
            // Only room left is 9 more potions on the existing stack; no empty slot.
            var existing = Instance(_potion, StackableMax - 9);
            var sword = Instance(_sword);
            var slots = new ItemSlotsArray(2, new[] { existing, sword });
            var raised = CountSlotsChanged(slots);
            var incoming = Instance(_potion, 20);

            bool added = slots.Add(incoming);

            Assert.That(added, Is.False);
            Assert.That(existing.Count, Is.EqualTo(StackableMax));
            Assert.That(incoming.Count, Is.EqualTo(11));
            Assert.That(slots[1].Item, Is.SameAs(sword));
            Assert.That(raised(), Is.EqualTo(1));
            AssertInvariants(slots);
        }

        // An instance can never exceed MaxStockSize, and one empty slot holds MaxStockSize.
        // So whenever an empty slot exists, the whole incoming instance must fit.
        // Partial failure is only possible when there is NO empty slot.
        [Test]
        public void AddInstance_StackableWithAnEmptySlot_AlwaysFitsEntirely()
        {
            var sword = Instance(_sword);
            var slots = new ItemSlotsArray(3, new[] { Instance(_potion, 1), sword });
            var incoming = Instance(_potion, StackableMax); // largest possible instance

            bool added = slots.Add(incoming);

            Assert.That(added, Is.True);
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(StackableMax + 1));
            Assert.That(slots[1].Item, Is.SameAs(sword));
            AssertInvariants(slots);
        }

        [Test]
        public void AddInstance_LeftoverPlusAdded_EqualsOriginalCount()
        {
            var existing = Instance(_potion, StackableMax - 9);
            var slots = new ItemSlotsArray(2, new[] { existing, Instance(_sword) });
            const int original = 500;
            var incoming = Instance(_potion, original);

            slots.Add(incoming);

            int added = existing.Count - (StackableMax - 9);
            Assert.That(added + incoming.Count, Is.EqualTo(original));
        }

        // On partial failure the caller keeps `incoming` with the leftover. If the inventory
        // ever stored that same object in a slot, the slot's count would silently BE the leftover.
        [Test]
        public void AddInstance_PartialFailure_IncomingObjectIsNotStoredInAnySlot()
        {
            var slots = new ItemSlotsArray(2, new[] { Instance(_potion, StackableMax - 9), Instance(_sword) });
            var incoming = Instance(_potion, 20);

            slots.Add(incoming);

            Assert.That(ContainsReference(slots, incoming), Is.False);
            AssertInvariants(slots);
        }

        [Test]
        public void AddInstance_StackableNothingFits_ReturnsFalse_IncomingUnchanged_NoEvent()
        {
            var slots = new ItemSlotsArray(Capacity, FullOfSwords());
            var raised = CountSlotsChanged(slots);
            var incoming = Instance(_potion, 20);

            bool added = slots.Add(incoming);

            Assert.That(added, Is.False);
            Assert.That(incoming.Count, Is.EqualTo(20));
            Assert.That(raised(), Is.EqualTo(0));
        }

        [Test]
        public void AddInstance_NonStackableWhenFull_ReturnsFalse_AndLeavesSlotsUnchanged()
        {
            var loadout = FullOfSwords();
            var slots = new ItemSlotsArray(Capacity, loadout);
            var incoming = Instance(_sword);

            bool added = slots.Add(incoming);

            Assert.That(added, Is.False);
            Assert.That(incoming.Count, Is.EqualTo(1));
            for (int i = 0; i < Capacity; i++)
                Assert.That(slots[i].Item, Is.SameAs(loadout[i]));
        }

        [Test]
        public void AddInstance_Success_RaisesSlotsChangedOnce()
        {
            var slots = new ItemSlotsArray(Capacity);
            var raised = CountSlotsChanged(slots);

            slots.Add(Instance(_sword));

            Assert.That(raised(), Is.EqualTo(1));
        }

        [Test]
        public void AddInstance_SlotsChanged_IsRaisedAfterStateIsFinal()
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_potion, StackableMax - 1) });
            int totalSeenByHandler = -1;
            slots.SlotsChanged += () => totalSeenByHandler = TotalCountOf(slots, _potion);

            slots.Add(Instance(_potion, 10)); // 1 top-up + 9 into a new slot

            Assert.That(totalSeenByHandler, Is.EqualTo(StackableMax + 9));
        }

        // ---- invalid input. Invalid instances (null definition, bad count) can't be constructed —
        // ItemInstance's constructor rejects them, so those checks live in ItemInstanceTests.
        // Null is the only invalid input Add itself can still receive.

        [Test]
        public void AddInstance_Null_Throws_AndChangesNothing()
        {
            var slots = new ItemSlotsArray(Capacity);
            var raised = CountSlotsChanged(slots);

            Assert.That(() => slots.Add(null), Throws.InstanceOf<ArgumentNullException>());
            Assert.That(EmptySlotCount(slots), Is.EqualTo(Capacity));
            Assert.That(raised(), Is.EqualTo(0));
        }

        // ASSUMPTION: adding an instance that's already in the inventory is a programming error.
        // Without this, a stackable would merge into itself (doubling) and a non-stackable
        // would be referenced by two slots.
        [Test]
        public void AddInstance_AlreadyContained_Throws_AndChangesNothing()
        {
            var potions = Instance(_potion, 5);
            var slots = new ItemSlotsArray(Capacity, new[] { potions });

            Assert.That(() => slots.Add(potions), Throws.InstanceOf<ArgumentException>());
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(5));
            AssertInvariants(slots);
        }

        // ================================================================ int Add(ItemDefinition, int)

        [Test]
        public void AddDefinition_StackableAllFits_SingleStack_RemainderZero()
        {
            var slots = new ItemSlotsArray(Capacity);

            int remainder = slots.Add(_potion, 10);

            Assert.That(remainder, Is.EqualTo(0));
            Assert.That(OccupiedSlotCount(slots, _potion), Is.EqualTo(1));
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(10));
            AssertInvariants(slots);
        }

        [Test]
        public void AddDefinition_CreatedInstancesHaveTheRequestedDefinition()
        {
            var slots = new ItemSlotsArray(Capacity);

            slots.Add(_sword, 2);

            Assert.That(slots.Where(s => !s.IsEmpty).All(s => s.Item.Definition == _sword), Is.True);
        }

        [Test]
        public void AddDefinition_StackableAboveMaxStack_SplitsAcrossSlots_RemainderZero()
        {
            var slots = new ItemSlotsArray(Capacity);

            int remainder = slots.Add(_potion, StackableMax + 1);

            Assert.That(remainder, Is.EqualTo(0));
            Assert.That(OccupiedSlotCount(slots, _potion), Is.EqualTo(2));
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(StackableMax + 1));
            AssertInvariants(slots);
        }

        [Test]
        public void AddDefinition_Stackable_TopsUpExistingStackBeforeUsingEmptySlot()
        {
            var existing = Instance(_potion, 5);
            var slots = new ItemSlotsArray(Capacity, new[] { existing });

            int remainder = slots.Add(_potion, 10);

            Assert.That(remainder, Is.EqualTo(0));
            Assert.That(existing.Count, Is.EqualTo(15));
            Assert.That(OccupiedSlotCount(slots, _potion), Is.EqualTo(1));
            AssertInvariants(slots);
        }

        [Test]
        public void AddDefinition_StackablePartlyFits_AddsWhatFits_AndReturnsRest()
        {
            // Room for exactly 9: top-up only, no empty slot.
            var existing = Instance(_potion, StackableMax - 9);
            var sword = Instance(_sword);
            var slots = new ItemSlotsArray(2, new[] { existing, sword });

            int remainder = slots.Add(_potion, 20);

            Assert.That(remainder, Is.EqualTo(11));
            Assert.That(existing.Count, Is.EqualTo(StackableMax));
            Assert.That(slots[1].Item, Is.SameAs(sword));
            AssertInvariants(slots);
        }

        [Test]
        public void AddDefinition_StackablePartlyFitsAcrossEmptySlots_FillsEachToMax()
        {
            var slots = new ItemSlotsArray(2);

            int remainder = slots.Add(_potion, StackableMax * 2 + 5);

            Assert.That(remainder, Is.EqualTo(5));
            Assert.That(slots.All(s => !s.IsEmpty && s.Item.Count == StackableMax), Is.True);
            AssertInvariants(slots);
        }

        [Test]
        public void AddDefinition_NonStackable_OccupiesOneSlotPerItem_WithDistinctInstances()
        {
            var slots = new ItemSlotsArray(Capacity);

            int remainder = slots.Add(_sword, 3);

            Assert.That(remainder, Is.EqualTo(0));
            Assert.That(OccupiedSlotCount(slots, _sword), Is.EqualTo(3));
            AssertInvariants(slots); // includes "no shared references"
        }

        [Test]
        public void AddDefinition_NonStackablePartlyFits_FillsEmptySlots_AndReturnsRest()
        {
            var slots = new ItemSlotsArray(Capacity);

            int remainder = slots.Add(_sword, Capacity + 2);

            Assert.That(remainder, Is.EqualTo(2));
            Assert.That(OccupiedSlotCount(slots, _sword), Is.EqualTo(Capacity));
            AssertInvariants(slots);
        }

        [Test]
        public void AddDefinition_NothingFits_RemainderEqualsRequested_AndSlotsUnchanged()
        {
            var loadout = FullOfSwords();
            var slots = new ItemSlotsArray(Capacity, loadout);

            int remainder = slots.Add(_potion, 7);

            Assert.That(remainder, Is.EqualTo(7));
            for (int i = 0; i < Capacity; i++)
                Assert.That(slots[i].Item, Is.SameAs(loadout[i]));
        }

        [Test]
        public void AddDefinition_AddedPlusRemainder_EqualsRequested()
        {
            var slots = new ItemSlotsArray(2, new[] { Instance(_potion, StackableMax - 50) });
            const int requested = StackableMax + 100;

            int remainder = slots.Add(_potion, requested);

            int added = TotalCountOf(slots, _potion) - (StackableMax - 50);
            Assert.That(added + remainder, Is.EqualTo(requested));
        }

        [Test]
        public void AddDefinition_FullSuccess_RaisesSlotsChangedOnce()
        {
            var slots = new ItemSlotsArray(Capacity);
            var raised = CountSlotsChanged(slots);

            slots.Add(_sword, 3); // several slots change, still one operation

            Assert.That(raised(), Is.EqualTo(1));
        }

        [Test]
        public void AddDefinition_PartialSuccess_RaisesSlotsChangedOnce()
        {
            var slots = new ItemSlotsArray(Capacity);
            var raised = CountSlotsChanged(slots);

            _ = slots.Add(_sword, Capacity + 2);

            Assert.That(raised(), Is.EqualTo(1));
        }

        [Test]
        public void AddDefinition_NothingFits_DoesNotRaiseSlotsChanged()
        {
            var slots = new ItemSlotsArray(Capacity, FullOfSwords());
            var raised = CountSlotsChanged(slots);

            _ = slots.Add(_potion, 7);

            Assert.That(raised(), Is.EqualTo(0));
        }

        // ASSUMPTION: invalid input is a programming error and throws.
        [TestCase(0)]
        [TestCase(-5)]
        public void AddDefinition_NonPositiveCount_Throws(int count)
        {
            var slots = new ItemSlotsArray(Capacity);

            Assert.That(() => slots.Add(_potion, count), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void AddDefinition_NullDefinition_Throws()
        {
            var slots = new ItemSlotsArray(Capacity);

            Assert.That(() => slots.Add(null, 1), Throws.InstanceOf<ArgumentNullException>());
        }

        // ================================================================ Remove(ItemInstance)

        [Test]
        public void RemoveInstance_Present_ReturnsTrue_AndEmptiesItsSlot()
        {
            var sword = Instance(_sword);
            var slots = new ItemSlotsArray(Capacity, new[] { sword });

            bool removed = slots.Remove(sword);

            Assert.That(removed, Is.True);
            Assert.That(slots[0].IsEmpty, Is.True);
            AssertInvariants(slots);
        }

        [Test]
        public void RemoveInstance_RemovesByReference_NotByDefinition()
        {
            var first = Instance(_sword);
            var second = Instance(_sword);
            var slots = new ItemSlotsArray(Capacity, new[] { first, second });

            slots.Remove(second);

            Assert.That(slots[0].Item, Is.SameAs(first));
            Assert.That(slots[1].IsEmpty, Is.True);
        }

        [Test]
        public void RemoveInstance_DoesNotShiftOtherSlots()
        {
            var a = Instance(_sword);
            var b = Instance(_sword);
            var c = Instance(_sword);
            var slots = new ItemSlotsArray(Capacity, new[] { a, b, c });

            slots.Remove(a);

            Assert.That(slots[1].Item, Is.SameAs(b));
            Assert.That(slots[2].Item, Is.SameAs(c));
        }

        [Test]
        public void RemoveInstance_StackableRemovesWholeStack_RegardlessOfCount()
        {
            var potions = Instance(_potion, 50);
            var slots = new ItemSlotsArray(Capacity, new[] { potions });

            slots.Remove(potions);

            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(0));
        }

        [Test]
        public void RemoveInstance_NotPresent_ReturnsFalse_AndChangesNothing()
        {
            var sword = Instance(_sword);
            var slots = new ItemSlotsArray(Capacity, new[] { sword });
            var raised = CountSlotsChanged(slots);

            bool removed = slots.Remove(Instance(_sword));

            Assert.That(removed, Is.False);
            Assert.That(slots[0].Item, Is.SameAs(sword));
            Assert.That(raised(), Is.EqualTo(0));
        }

        [Test]
        public void RemoveInstance_Twice_SecondReturnsFalse()
        {
            var sword = Instance(_sword);
            var slots = new ItemSlotsArray(Capacity, new[] { sword });

            slots.Remove(sword);
            bool removedAgain = slots.Remove(sword);

            Assert.That(removedAgain, Is.False);
        }

        [Test]
        public void RemoveInstance_Success_RaisesSlotsChangedOnce()
        {
            var sword = Instance(_sword);
            var slots = new ItemSlotsArray(Capacity, new[] { sword });
            var raised = CountSlotsChanged(slots);

            slots.Remove(sword);

            Assert.That(raised(), Is.EqualTo(1));
        }

        // ASSUMPTION: null is a programming error and throws.
        [Test]
        public void RemoveInstance_Null_Throws()
        {
            var slots = new ItemSlotsArray(Capacity);

            Assert.That(() => slots.Remove((ItemInstance)null), Throws.InstanceOf<ArgumentNullException>());
        }

        // ================================================================ Remove(ItemDefinition)

        [Test]
        public void RemoveDefinition_Present_ReturnsTrue_AndEmptiesMatchingSlot()
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_sword) });

            bool removed = slots.Remove(_sword);

            Assert.That(removed, Is.True);
            Assert.That(OccupiedSlotCount(slots, _sword), Is.EqualTo(0));
            AssertInvariants(slots);
        }

        [Test]
        public void RemoveDefinition_LeavesOtherDefinitionsUntouched()
        {
            var potions = Instance(_potion, 5);
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_sword), potions });

            slots.Remove(_sword);

            Assert.That(slots[1].Item, Is.SameAs(potions));
            Assert.That(potions.Count, Is.EqualTo(5));
        }

        // ASSUMPTION: Remove(definition) removes only the FIRST matching slot.
        // If it's meant to remove every slot of that definition, flip this to expect both empty.
        [Test]
        public void RemoveDefinition_WithSeveralMatches_RemovesOnlyFirst()
        {
            var first = Instance(_sword);
            var second = Instance(_sword);
            var slots = new ItemSlotsArray(Capacity, new[] { first, second });

            slots.Remove(_sword);

            Assert.That(slots[0].IsEmpty, Is.True);
            Assert.That(slots[1].Item, Is.SameAs(second));
        }

        [Test]
        public void RemoveDefinition_NotPresent_ReturnsFalse_AndDoesNotRaiseEvent()
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_potion, 3) });
            var raised = CountSlotsChanged(slots);

            bool removed = slots.Remove(_sword);

            Assert.That(removed, Is.False);
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(3));
            Assert.That(raised(), Is.EqualTo(0));
        }

        [Test]
        public void RemoveDefinition_Success_RaisesSlotsChangedOnce()
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_sword) });
            var raised = CountSlotsChanged(slots);

            slots.Remove(_sword);

            Assert.That(raised(), Is.EqualTo(1));
        }

        // ASSUMPTION: null is a programming error and throws.
        [Test]
        public void RemoveDefinition_Null_Throws()
        {
            var slots = new ItemSlotsArray(Capacity);

            Assert.That(() => slots.Remove((ItemDefinition)null), Throws.InstanceOf<ArgumentNullException>());
        }

        // ================================================================ Take(ItemDefinition, int)

        [Test]
        public void Take_LessThanStack_ReducesCount_AndKeepsSlot()
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_potion, 5) });

            bool taken = slots.Take(_potion, 2);

            Assert.That(taken, Is.True);
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(3));
            Assert.That(slots[0].IsEmpty, Is.False);
            AssertInvariants(slots);
        }

        [Test]
        public void Take_ExactStack_EmptiesSlot()
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_potion, 5) });

            bool taken = slots.Take(_potion, 5);

            Assert.That(taken, Is.True);
            Assert.That(slots[0].IsEmpty, Is.True);
            AssertInvariants(slots);
        }

        [Test]
        public void Take_AcrossSeveralStacks_TakesTotalAmount()
        {
            var slots = new ItemSlotsArray(Capacity,
                new[] { Instance(_potion, StackableMax), Instance(_potion, 10) });

            bool taken = slots.Take(_potion, StackableMax + 5);

            Assert.That(taken, Is.True);
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(5));
            AssertInvariants(slots); // no zero-count leftovers
        }

        [Test]
        public void Take_ExactTotalAcrossSeveralStacks_EmptiesAllOfThem()
        {
            var slots = new ItemSlotsArray(Capacity,
                new[] { Instance(_potion, 7), Instance(_sword), Instance(_potion, 3) });

            bool taken = slots.Take(_potion, 10);

            Assert.That(taken, Is.True);
            Assert.That(OccupiedSlotCount(slots, _potion), Is.EqualTo(0));
            Assert.That(OccupiedSlotCount(slots, _sword), Is.EqualTo(1));
            AssertInvariants(slots);
        }

        [Test]
        public void Take_NonStackable_TakesAcrossSlots()
        {
            var slots = new ItemSlotsArray(Capacity, FullOfSwords());

            bool taken = slots.Take(_sword, 3);

            Assert.That(taken, Is.True);
            Assert.That(OccupiedSlotCount(slots, _sword), Is.EqualTo(1));
            AssertInvariants(slots);
        }

        [Test]
        public void Take_MoreThanOwned_ReturnsFalse_AndTakesNothing()
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_potion, 3) });
            var raised = CountSlotsChanged(slots);

            bool taken = slots.Take(_potion, 4);

            Assert.That(taken, Is.False);
            Assert.That(TotalCountOf(slots, _potion), Is.EqualTo(3));
            Assert.That(raised(), Is.EqualTo(0));
        }

        [Test]
        public void Take_MoreThanOwnedAcrossStacks_ReturnsFalse_AndTakesNothingFromAnyStack()
        {
            var first = Instance(_potion, 5);
            var second = Instance(_potion, 5);
            var slots = new ItemSlotsArray(Capacity, new[] { first, second });

            bool taken = slots.Take(_potion, 11);

            Assert.That(taken, Is.False);
            Assert.That(first.Count, Is.EqualTo(5));
            Assert.That(second.Count, Is.EqualTo(5));
        }

        [Test]
        public void Take_DefinitionNotPresent_ReturnsFalse()
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_sword) });

            bool taken = slots.Take(_potion, 1);

            Assert.That(taken, Is.False);
        }

        [Test]
        public void Take_Success_RaisesSlotsChangedOnce()
        {
            var slots = new ItemSlotsArray(Capacity,
                new[] { Instance(_potion, StackableMax), Instance(_potion, 10) });
            var raised = CountSlotsChanged(slots);

            slots.Take(_potion, StackableMax + 5); // touches two slots, still one operation

            Assert.That(raised(), Is.EqualTo(1));
        }

        // ASSUMPTION: invalid input is a programming error and throws.
        [TestCase(0)]
        [TestCase(-2)]
        public void Take_NonPositiveCount_Throws(int count)
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_potion, 5) });

            Assert.That(() => slots.Take(_potion, count), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void Take_NullDefinition_Throws()
        {
            var slots = new ItemSlotsArray(Capacity);

            Assert.That(() => slots.Take(null, 1), Throws.InstanceOf<ArgumentNullException>());
        }

        // ================================================================ IReadOnlyList / IEnumerable

        [Test]
        public void Enumeration_YieldsSlotsInIndexOrder()
        {
            var slots = new ItemSlotsArray(Capacity, new[] { Instance(_sword), Instance(_potion, 2) });

            var enumerated = slots.ToList();

            Assert.That(enumerated.Count, Is.EqualTo(slots.Count));
            for (int i = 0; i < slots.Count; i++)
                Assert.That(enumerated[i], Is.SameAs(slots[i]));
        }

        [TestCase(-1)]
        [TestCase(Capacity)]
        public void Indexer_OutOfRange_Throws(int index)
        {
            var slots = new ItemSlotsArray(Capacity);

            // Array-backed → IndexOutOfRangeException, List-backed → ArgumentOutOfRangeException.
            // Either is fine; tighten this once you know which you use.
            Assert.That(() => slots[index], Throws.Exception);
        }
    }
}