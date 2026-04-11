# Emblem Versus

A turn-based strategy game built with Unity, inspired by classic tactical RPGs. Two players face off on a grid-based battlefield, commanding armies with different unit types and weapons.

## Gameplay

- **Two-player local multiplayer** — Player 1 and Player 2 take turns on the same machine.
- **Turn-based system** — The starting player is chosen randomly. A turn ends when all of the current player's units have acted.
- **Move & Attack** — On your turn, select a unit to see its movement range (highlighted tiles), move it to a valid tile, then choose to attack or wait.
- **Combat** — Attackable enemies are highlighted in red. Click an enemy tile to attack. Combat outcome depends on weapon accuracy, attack stat, and a 20% critical-hit chance.
- **Win condition** — Eliminate all enemy units to win the match.

## Units

### Player 1
| Code | Unit |
|------|------|
| `cu` | Cuirassier |
| `hu` | Hussard |
| `if` | Infantry |

### Player 2
| Code | Unit |
|------|------|
| `mzt` | Mpandefa Zanatsipika |
| `mad` | Mpanao Ady |
| `mdb` | Mpandatsa Baratra |

Each unit has the following stats:
- **Health** — Hit points; the unit is eliminated when health reaches 0.
- **Attack** — Melee attack power.
- **Defense** — Damage reduction.
- **Spirituality** — Power stat used by spiritual weapons.
- **Mobility** — Number of tiles the unit can move per turn.
- **Weapon** — Determines attack style and range.

## Weapons

Weapons belong to one of three categories, each using a different formula to calculate hit rate:

| Category | Hit formula | Critical |
|----------|-------------|---------|
| `MELEE_WEAPON` | Accuracy + Attack | ×1.5 on 20% chance |
| `FIRE_WEAPON` | Accuracy | ×1.5 on 20% chance |
| `SPIRITUAL_WEAPON` | Accuracy + Spirituality | ×1.5 on 20% chance |

Each weapon also defines a minimum and maximum attack range (in tiles).

## Terrain

| Symbol | Tile | Effect |
|--------|------|--------|
| *(empty)* | Grass | Walkable |
| `o` | Water | Blocks movement |
| `T` | Forest | Blocks movement |
| `t` | Dead Forest | Blocks movement |

## Controls

| Action | Input |
|--------|-------|
| Select / Move / Attack | Left Click |
| Cancel action | Escape |

## Project Structure

```
Assets/
├── Scripts/
│   ├── GamePlay/
│   │   ├── Board/          # Board generation and unit placement
│   │   ├── Camera/         # Camera management and zoom
│   │   ├── Input/          # Input handling and click modes
│   │   ├── Level/          # Level/ground utilities
│   │   ├── Manager/        # GameManager — main game loop controller
│   │   ├── System/         # Core systems (Move, Fight, Tile, TurnBase)
│   │   ├── Tools/          # A* pathfinding
│   │   └── UI/             # UI controller and HUD
│   ├── Units/              # Unit, Commander enum, Inventory, UnitAction
│   └── Weapons/            # Weapon, WeaponCategory, WeaponType, WeaponAttribute
├── Prefabs/                # Level, UI, and Unit prefabs
├── Scenes/                 # MainScene, OverView, TestScene
├── Sprites/                # 2D sprites and textures
├── Fbx/                    # 3D models
└── Settings/               # URP rendering settings
```

## Requirements

- **Unity** 2022.3.36f1
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Input System**: Unity Input System package

## Getting Started

1. Clone the repository.
2. Open the project in **Unity 2022.3.36f1** (or a compatible 2022.3 LTS version).
3. Open `Assets/Scenes/MainScene.unity`.
4. Press **Play** to start the game.
