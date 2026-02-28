# Main Menu + Upgrade Hub Setup

## 1) Create scenes
Create and save these scenes:
- `Assets/Scenes/MainMenu.unity`
- `Assets/Scenes/UpgradeHub.unity`
- `Assets/Scenes/Game.unity` (already exists)

## 2) MainMenu scene
- Create empty object `MainMenuController`
- Add script `MainMenuController`
- Keep default scene names: `Game`, `UpgradeHub`

## 3) UpgradeHub scene
- Create empty object `UpgradeHubController`
- Add script `UpgradeHubController`
- Keep default scene names: `MainMenu`, `Game`

## 4) Build Settings order
File -> Build Settings -> Scenes In Build:
1. MainMenu
2. UpgradeHub
3. Game

Set MainMenu as startup scene (first in list).

## 5) Optional
In `GameManager`, you can later change Game Over flow to go back to UpgradeHub instead of in-run upgrade buttons.
