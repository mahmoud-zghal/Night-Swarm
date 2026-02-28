# Night Swarm Unity MVP (Git-first workflow)

This is a lightweight Unity MVP starter for a Survivor-like mobile game.

## Requirements
- Unity **2022.3 LTS** (or newer LTS)
- Android Build Support installed in Unity Hub (SDK + NDK + OpenJDK)

## Git Workflow
1. Create a new Unity 2D project locally named `NightSwarm`.
2. Close Unity.
3. Copy this repo folder contents into your Unity project root (merge, do not replace `ProjectSettings/` if you already configured things).
4. Commit:
   ```bash
   git add .
   git commit -m "chore: bootstrap Night Swarm MVP"
   ```
5. Open project in Unity.
6. Create scene `Assets/Scenes/Game.unity` and attach scripts per `docs/UNITY_SETUP.md`.
7. Build APK:
   - File → Build Settings → Android → Switch Platform
   - Player Settings: set package name (e.g. `com.yourname.nightswarm`)
   - Build → output `NightSwarm.apk`

## Included Scripts
- Player movement and stats
- Enemy chase AI
- Auto attack + projectiles
- XP + leveling
- Spawner with simple wave scaling
- Basic game state manager

## Notes
This scaffold is intentionally simple so you can get a playable loop quickly, then iterate.
