#!/usr/bin/env bash
#
# Creates labels, the Vertical Slice milestone, and 14 issues.
# Run from inside the repo, with gh already authenticated:
#
#   chmod +x create-issues.sh
#   ./create-issues.sh
#
# Safe to inspect first with:  bash -n create-issues.sh

set -euo pipefail

MILESTONE="Vertical Slice"

REPO="$(gh repo view --json nameWithOwner -q .nameWithOwner)"
echo "Target repo: $REPO"
echo

# ---------------------------------------------------------------- labels ----
echo "Creating labels..."

gh label create "bug"            --color "d73a4a" --description "Something is broken"        --force
gh label create "feature"        --color "0e8a16" --description "New functionality"          --force
gh label create "refactor"       --color "fbca04" --description "Restructuring, no new behavior" --force
gh label create "content"        --color "1d76db" --description "Authoring, placement, tuning"   --force

gh label create "area:input"     --color "5319e7" --description "Input System, devices, bindings" --force
gh label create "area:ui"        --color "5319e7" --description "Canvas, screens, navigation"     --force
gh label create "area:gameplay"  --color "5319e7" --description "Combat, enemies, scene logic"    --force
gh label create "area:settings"  --color "5319e7" --description "Settings model and screens"      --force
gh label create "area:inventory" --color "5319e7" --description "Inventory model and UI"          --force

echo

# ------------------------------------------------------------- milestone ----
echo "Creating milestone..."

gh api "repos/$REPO/milestones" -f title="$MILESTONE" \
  -f description="Everything required for one finished, polished playable segment." \
  >/dev/null 2>&1 && echo "  created: $MILESTONE" || echo "  already exists (or not permitted): $MILESTONE"

echo

# ------------------------------------------------------------ helpers ------
# Creates an issue and echoes its number to stdout. Everything else goes to
# stderr so the number can be captured cleanly with $(...).
mk() {
  local title="$1"; shift
  local body="$1"; shift
  local url num
  url="$(gh issue create --title "$title" --body "$body" --milestone "$MILESTONE" "$@")"
  num="${url##*/}"
  echo "  #$num  $title" >&2
  echo "$num"
}

echo "Creating issues..."

# ============================================================== gamepad =====

mk "Explicit navigation graph on all UI screens" \
"Set every selectable to \`Navigation.Mode = Explicit\` and wire up/down/left/right by hand across all UI screens.

Automatic navigation infers direction from RectTransform geometry, which breaks as soon as layout groups, scroll rects, or conditionally disabled elements are involved. Explicit is more setup and strictly more predictable.

**Acceptance criteria**
- Every screen is fully traversable with a gamepad.
- No dead ends: every selectable can be left in all four directions, or deliberately terminates.
- No jumps to visually unrelated elements.
- Disabled or hidden elements are never reachable." \
  --label "feature" --label "area:ui" >/dev/null

FIRST_SELECTED=$(mk "Set first-selected object when a screen opens" \
"Each screen sets its own initial selection when it becomes active.

\`EventSystem.firstSelectedGameObject\` only fires once at application start; it does nothing when a screen is activated later. Selection also cannot resolve to an inactive or non-interactable object, so the target must already be live at the moment selection is set.

**Acceptance criteria**
- Opening any screen with a gamepad leaves exactly one element selected.
- No screen ever opens with nothing highlighted.
- Works regardless of whether the screen was opened by gamepad, keyboard, or mouse." \
  --label "feature" --label "area:ui")

mk "Selection highlight visuals" \
"Make the current selection unmistakable on screen.

Selectable's built-in colour tint is usually too subtle to read against pixel art. Decide on one approach and apply it consistently: colour tint, animated highlight sprite, or a moving cursor object.

**Acceptance criteria**
- The selected element is identifiable at a glance on every screen.
- The treatment is consistent across all screens.
- Highlight remains legible against every background it appears over." \
  --label "feature" --label "area:ui" >/dev/null

mk "Selection restore and mouse/gamepad handover" \
"Two related selection failures:

1. Closing a submenu returns selection to the first element rather than the element that opened it.
2. Clicking with the mouse clears selection entirely, so the next gamepad input has no anchor and navigation does nothing.

**Acceptance criteria**
- Back-navigation restores the selection that was active before the submenu opened.
- After a mouse click that clears selection, the next gamepad input reselects a sensible element rather than doing nothing.
- Switching between mouse and gamepad mid-screen never leaves the UI unnavigable." \
  --label "bug" --label "area:input" --label "area:ui" >/dev/null

# ============================================================== dungeon =====

mk "Place boss encounter in dungeon scene" \
"Boss behavior is complete; this is placement only. Position the boss, set up the arena, and make it reachable as the final encounter of the dungeon.

**Acceptance criteria**
- Dungeon is playable from start through boss defeat with no placeholders.
- Boss arena is reachable by normal play, not only by teleport or editor start position.
- Boss does not activate before the player enters the arena." \
  --label "content" --label "area:gameplay" >/dev/null

LEVEL_COMPLETE=$(mk "Level completion condition and run time capture" \
"Boss death raises a level-complete event that stops the run timer and carries the final elapsed time as data.

Two things to get right:
- The timer stops at the completion event, not when the finish screen finishes animating in. Otherwise recorded times silently include UI transition duration.
- The event carries the time. The finish screen should not reach back into the timer to query it.

**Acceptance criteria**
- Defeating the boss raises a completion event carrying the final time.
- The timer does not advance after that event is raised.
- Nothing outside the timer's owner mutates the elapsed value." \
  --label "feature" --label "area:gameplay")

mk "Finish screen" \
"Screen shown on level completion, displaying the run time and exit options.

Depends on #${LEVEL_COMPLETE} (completion event carries the time).
Should be built after #${FIRST_SELECTED} so it follows the established selection convention rather than needing a revisit.

**Acceptance criteria**
- Appears on level completion.
- Displays the captured run time in a readable format.
- Fully gamepad-navigable with a valid first selection.
- Exit options work and return to a defined destination." \
  --label "feature" --label "area:ui" >/dev/null

# ============================================================ inventory =====

INV_CAPACITY=$(mk "Inventory capacity as authoritative data" \
"Capacity currently exists only as an implication of the UI layout, which is why 11 items render out of bounds. Move capacity onto the inventory model and enforce it there.

Adds beyond capacity fail at the model and the caller is informed of the failure.

**Acceptance criteria**
- An inventory with capacity 9 never holds 10 items, regardless of how items are added.
- Add attempts that exceed capacity return a failure the caller can act on.
- Capacity is configured as data, not hardcoded at the call site." \
  --label "bug" --label "area:inventory")

mk "Inventory UI binds to model capacity" \
"Slot views are generated from the model's capacity rather than assumed by the layout.

Depends on #${INV_CAPACITY}.

**Acceptance criteria**
- Changing capacity in the definition changes the rendered slot count with no UI edits.
- No item can render outside the slot grid.
- Empty slots render as empty rather than being omitted." \
  --label "bug" --label "area:ui" --label "area:inventory" >/dev/null

mk "Refused pickup feedback message" \
"When the inventory is full, the pickup is refused, the item stays in the world, and a short-lived world-space message naming the item appears.

Depends on #${INV_CAPACITY}.

This is the same shape as damage numbers: a fire-and-forget world-positioned text. Check whether the existing pooled VFX spawner can take a message type before building anything parallel.

**Acceptance criteria**
- Pickup on a full inventory leaves the item in the world, still pickable later.
- A message containing the item's display name appears at the pickup location.
- Message expires on its own; no cleanup burden on the caller.
- Repeated attempts do not stack messages unreadably." \
  --label "feature" --label "area:inventory" --label "area:ui" >/dev/null

# ============================================================= settings =====

SETTINGS_MODEL=$(mk "Runtime settings model with apply/revert" \
"One authoritative owner of settings values, holding pending changes separately from applied ones.

This split is what makes Apply and Cancel meaningful, and it is the foundation every individual setting builds on.

Systems should read from this model rather than having the settings UI push values into them. If the UI pushes, every new setting means new wiring from the settings screen to another system. If systems pull, a new setting is one field.

**Acceptance criteria**
- Changing a control mutates pending state only.
- Apply commits pending values to live values.
- Cancel discards pending values, leaving live values untouched.
- Exactly one object owns the applied values." \
  --label "feature" --label "area:settings")

mk "Persist settings across launches" \
"Serialize applied settings to disk and load them at boot.

Load ordering is where this usually breaks: values must be loaded and applied before any system reads them on the first frame.

Depends on #${SETTINGS_MODEL}.

**Acceptance criteria**
- Change a setting, quit, relaunch: the setting is still in effect.
- Loading completes before the first frame that consumes any setting.
- A missing or corrupt save file falls back to defaults without erroring." \
  --label "feature" --label "area:settings" >/dev/null

mk "Screen shake intensity setting" \
"The camera shake system reads its intensity multiplier from the settings model.

Depends on #${SETTINGS_MODEL}.

**Acceptance criteria**
- Slider at 0 produces no shake at all.
- Intermediate values scale shake proportionally.
- Changes take effect immediately, without a restart." \
  --label "feature" --label "area:settings" --label "area:gameplay" >/dev/null

mk "Damage numbers toggle" \
"The damage number spawner consults the settings model before spawning.

Depends on #${SETTINGS_MODEL}.

**Acceptance criteria**
- Toggle off suppresses spawning entirely, not just visibility.
- No pooled objects are allocated while the setting is off.
- Changes take effect immediately, without a restart." \
  --label "feature" --label "area:settings" --label "area:gameplay" >/dev/null

echo
echo "Done."
