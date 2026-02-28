# Unity Setup (10-20 min)

## 1) Scene Objects
Create these GameObjects in `Game` scene:
- `GameManager` (attach `GameManager`)
- `Player` (attach `PlayerController`, `PlayerStats`, `AutoAttack`, `RuneRing`)
- `Spawner` (attach `Spawner`)
- `Main Camera` (optional follow script later)

## 2) Prefabs
- Enemy prefab:
  - Add `SpriteRenderer`, `Rigidbody2D` (Gravity Scale 0), `CircleCollider2D` (isTrigger off)
  - Add `EnemyStats` + `EnemyAI`
- Projectile prefab:
  - Add `SpriteRenderer`, `Rigidbody2D` (Gravity Scale 0, Body Type Kinematic)
  - Add `CircleCollider2D` (isTrigger on)
  - Add `Projectile`

## 3) Script Wiring
- In `AutoAttack`, assign projectile prefab and set fireRate (e.g. 1.5)
- In `RuneRing`, assign `bladeVisualPrefab` (you can reuse projectile sprite as placeholder)
- In `Spawner`, assign player transform and configure `enemies` array entries with your enemy prefabs + weights

## 4) Input
This version uses Unity classic input axes (`Horizontal`, `Vertical`).
For mobile joystick later, map your joystick output to `PlayerController.SetInput(Vector2)`.

## 5) Level-up UI
A built-in 3-card level-up overlay is included in `LevelUpSystem` (IMGUI).
When you level up, the game pauses and waits for your card pick.

## 6) Layers
(Optional) create `Player`, `Enemy`, `Projectile` layers for cleaner collision rules.

## 7) Meta progression
- Progress is saved with `PlayerPrefs` (coins + permanent upgrades).
- On Game Over, you can buy upgrades and tap **Play Again**.

## 8) Debug cheats (testing)
- Add `DebugCheats` script to `GameManager` object.
- Hotkeys in Play mode:
  - `K` = kill player instantly
  - `C` = add test coins
  - `L` = force level up

## 9) Build APK
- Install Android support from Unity Hub
- Build Settings → Android → Switch Platform
- Build
