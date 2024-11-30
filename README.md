# SyncUp

## Changelog (from 2024/11/29 | 15:14)

> ### from 2024/11/29 | 15:14 | OlvrMns
> - README init
> - Deleted Level1 Scene
> - Created directory -> Assets/Prefabs/GameStates/Singletons/
> - GameObject/MenuBehavior : Canvas -> LevelSelector/Menu/Store Overlay
> - Created directory -> Assets/Scenes/CompletedScenes
> - Created directory -> Assets/Scenes/CompletedScenes/Levels
> - Created UIUtils
> - Created ComponentUtils
> - Added IsInitialized/AutoStart/SampleRate/BitsPerSample/Channels attribute 
> - Singleton AudioManager 
> - Added condition in AbilityController for [NONE] ability type
> ### to ...

> ### from 2024/11/29 | 18:55 | OlvrMns
> - Added Singleton gameObjects in Menu Scene
> - Changed Singleton execution order 
> - Changed InitializeLoopbackCapture() in AudioManager
> - Imported AllSky, Poly Angel - Mini Pirates Island, Lowpoly Cowboy RIO V1.1, AI Navigation
> - Changed FPSDisplayTextPrefab
> - Added TextColorSync
> - Added ColorSync to FPSText
> - Rearranged Menu Lab Objects for ColorSync
> - Added Assets/Animations/
> - Added Enemy scripts and Cow bow Animations [from Tommy](https://github.com/TommySpag/SyncUp-Level1)
> - Added ButtonEffect Script 
> - Changed Menu Inferface buttons
> - Added Sync Cubes to menu lab
> ### to 2024/11/29 | 23:42

> ### from 2024/11/29 | 23:47 | OlvrMns
> - Removed some assets (AllSky)
> - Linked Menu -> Scene and Scene -> Menu
> - Made PauseController NOT a Singleton
> - Changed Singleton Destroy(this) -> Destroy(this.gameObject)
> - Changed AudioManager OnDestroy (only if not null)
> - Fixed Capture not starting after going to Menu from another scene + Time.timeScale
> ### to 2024/11/30 | 1:06 

> ### from 2024/11/30 | 12:43 | OlvrMns
> - Imported Level1 [from Tommy](https://github.com/TommySpag/SyncUp-Level1)
> - Added Settings Menu Button And Settings Canvas
> - Added Settings Canvas to menu navigation
> - Added DebugMode boolean
> - Changed GameEnvironment Script 
> - Added Level Restart Button
> - Changed Pause Interface buttons style
> - Added OverlayController (Reformated InGame Overlay Control and Reformated Title Screen Navigation with Optional parameters)
> - 
> ### to ... | ...

## Prio
- (1) LevelSelector Position OffSet In UI for more than 1 scene
- (1) Sync level 1 
- (1) Settings tab
- (2) PulseSyncer
- (2) TimeFreeze
- (2) Spotify Controller => .ContinueWith And Set UserName 
- (2) Unlock abilities with credits
- (2) SetActive vs Enabled (?)
- (2) Ability UI for GameOverlay
- (2) Make normal bullet
- (2) Button to reset spotify user array
- (3) Finish spotify interface
- (3) Challenge/Level logic
- (3) Add sounds
- (3) Fix TempoSetter
- (4) Perfect hit 
- (4) Ricochet
- (5) Main menu / Pause menu / Death menu style (Animate with music)
- (3) AudioManager initialization retry interface
- (4) Custom FrameTempo
