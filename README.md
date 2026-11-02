# Recalled RPG

A 2D top-down pixel-art action RPG, built solo in Unity (URP).



A boy is trapped in a dungeon he doesn't recognise. The deeper he goes, the more the people he meets begin to *recall* him.

[](https://github.com/user-attachments/assets/a637a892-8808-4e13-8f5e-a5127c2ab364)

> **Status:** work in progress. I'm building a playable vertical slice: one dungeon, its enemies, and a boss.

## About

Recalled is my portfolio project for gameplay programming roles. The focus is not content volume but systems that are easy to extend and reason about. Each highlight below says what a system does and why it is built that way.

## Highlights

### Architecture

- **Definition / instance split.** ScriptableObjects hold immutable configuration (items, movement AI, factions). Plain C# objects own the mutable runtime state. Assets can be tuned without touching code, many entities can share one asset without sharing state, and play-mode changes never write back into assets.
- **Two-phase damage pipeline.** A hit first passes through an ordered chain of modifiers that resolve it into a shared damage context. A stateless notify phase then broadcasts the result to whatever reacts to it. New modifiers and new reactions plug in independently: N + M connections instead of N × M.
- **Events chosen by scope.** Plain C# events carry signals inside a single entity. ScriptableObject event channels carry signals between systems and out to the UI.
- **Dependency direction enforced by the compiler.** Code is split into assembly definitions under `Recalled.*`. UI depends on Gameplay; Gameplay never depends on UI and owns the event contracts the UI listens to.

### Gameplay systems

- **Movement AI as data.** `MovementAI` ScriptableObjects define behaviours and create a plain C# runtime instance per enemy.
- **Faction-based collision filtering.** `FactionSO` assets decide who can hit whom, so hit rules are data rather than hard-coded checks.
- **Health and HUD.** `HealthSO` and `ValueProviderSO` feed relay components that forward changes to HUD event channels. Gameplay code has no knowledge of the HUD.
- **Inventory.** `ItemDefinition` / `ItemInstance` split, a Factory Method for creating instances, and quick slots.
- **Dialogue.** Full supported dialogue system with SO objects holding text assets.


## Project structure

Assemblies:
- **Recalled.Core**: Runtime variables and event channels 
- **Recalled.Gameplay**: Entities, combat, AI, inventory; defines the event payload contracts
- **Recalled.Dialogue**: Dialogue
- **Recalled.UI**: HUD and menus; references Gameplay, never the other way round

## Running the project

1. Install latest Unity through Unity Hub.
2. Clone this repository and add the folder in Unity Hub.
3. Open the scene `Dungeon Scene` and press Play.

Gamepad support is almost full and is in progress.

## Roadmap

- Gamepad support
- Dungeon enemy and boss placement
- Settings panel (rebinding, resolution)
- Dedicated item-use component

## AI Usage

I’ve used AI throughout development, and still do, mainly as a learning tool. It let me pick up concepts much faster than searching the web for answers would have. I also used it for tedious work such as writing custom Editor tooling. And Game over screen

## Credits

meeee
