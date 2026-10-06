# foretales-localsave
Makes the game save/load locally to game-state.

This fixes the v1.2.4232 Epic Games Store version of Foretales, which is bugged to try to save/load from the EGS cloud. Save/Load will instead save to 'game-state' in the same directory as the exe, instead of 'AppData\LocalLow\Alkemi\Foretales\release_game-state'.

**REQUIRES BEPINEX**

Grab BepInEx Bleeding Edge for IL2CPP (**x86, NOT x64**) at https://builds.bepinex.dev/projects/bepinex_be

Download the repo, you want the dll. Code > Download Zip.

Once you have BepInEx setup, simply place 'MyFirstPlugin.dll' into the 'BepInEx\plugins' folder.
