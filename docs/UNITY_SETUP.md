# Unity Setup (10-20 min)

## 1) Scene Objects
Create these GameObjects in `Game` scene:
- `GameManager` (attach `GameManager`)
- `Player` (attach `PlayerController`, `PlayerStats`, `AutoAttack`)
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
- In `Spawner`, assign enemy prefab and player transform

## 4) Input
This version uses Unity classic input axes (`Horizontal`, `Vertical`).
For mobile joystick later, map your joystick output to `PlayerController.SetInput(Vector2)`.

## 5) Layers
(Optional) create `Player`, `Enemy`, `Projectile` layers for cleaner collision rules.

## 6) Build APK
- Install Android support from Unity Hub
- Build Settings → Android → Switch Platform
- Build
