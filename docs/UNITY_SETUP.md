# Unity Setup (10-20 min)

## 1) Scene Objects
Create these GameObjects in `Game` scene:
- `GameManager` (attach `GameManager`, `RuntimeHUD`)
- `Player` (attach `PlayerController`, `PlayerStats`, `AutoAttack`, `RuneRing`)
- `Spawner` (attach `Spawner`)
- `WaveDirector` (attach `WaveDirector`)
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
- In `Spawner`, assign enemy prefab + player transform (WaveDirector can auto-link)
- In `WaveDirector`, tune spawn pacing and spike intensity (defaults are good for MVP)

## 4) Input
- Desktop: Unity classic input axes (`Horizontal`, `Vertical`).
- Mobile: built-in touch joystick in `PlayerController` (left side drag).

Recommended `PlayerController` values:
- `enableMobileTouchJoystick = true`
- `joystickRadius = 90`
- `deadZone = 0.15`

## 5) Level-up UI
A built-in 3-card level-up overlay is included in `LevelUpSystem` (IMGUI).
When you level up, the game pauses and waits for your card pick.

## 6) Runtime HUD
`RuntimeHUD` shows HP/XP bars, level, timer, and a top-right Pause/Resume button.

## 7) Layers
(Optional) create `Player`, `Enemy`, `Projectile` layers for cleaner collision rules.

## 8) Meta progression
- Progress is saved with `PlayerPrefs` (coins + permanent upgrades).
- On Game Over, you can buy upgrades and tap **Play Again**.

## 9) Debug cheats (testing)
- Add `DebugCheats` script to `GameManager` object.
- Hotkeys in Play mode:
  - `K` = kill player instantly
  - `C` = add test coins
  - `L` = force level up

## 10) Mobile playability setup
- Add `MobileRuntimeSettings` to `GameManager` object.
- In Player Settings set orientation to Landscape (or let runtime script force it).
- If you use a Canvas HUD, put `SafeAreaFitter` on your root panel to avoid notches.

## 11) Build APK
- Install Android support from Unity Hub
- Build Settings → Android → Switch Platform
- Build
