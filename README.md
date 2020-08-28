# game-settings

A sample Unity project for doing simple game settings/option in your game-jam or as a starter template for your game's settings.

The project requires Textmesh Pro, a free asset from Unity. The project includes an example of usage with supporting assets. Import the Unity package if you just want the scripts and prefabs for your own project.

There are prefabs for the whole settings panel to get you started quickly. You then only need to make a call to `GameSettingsManager.RestoreSettings();` as soon as poissible. For example in the `Start()` method of a main menu script or game bootstrapping script. See the `Sample.cs` script.

This asset consists mainly of UI components and the main `GameSettingsManager` static class. Have a look at `GameSettingsManager` to see what is available. The Components under, menu: `Component > GameSettings > UI` can be used to build your own settings controls. 

* Toggle Fullscreen: used with a a Toggle element to switch between `Windowed` and `FullScreenWindow` (not Exclusive FullScreen) modes.
* Screen Mode Dropdown: or you can use this with a Dropdown element to display all the [Fullscreen mode options](https://docs.unity3d.com/ScriptReference/FullScreenMode.html) to the player: "Exclusive FullScreen", "FullScreen Window", "Maximized Window", and "Windowed".
* Resolution Dropdown: used with a Dropdown element to show all available resolutions. These are read from Unity's [Screen.resolutions](https://docs.unity3d.com/ScriptReference/Screen-resolutions.html) property which are all the supported fullscreen resolutions. Note that you can set a minimum resolution to present to the player via `GameSettingsManager.MinScreenSize`, which defaults to 1024x768. This drip-down's options will deffer depending on whether the screen is in Exclusive FullScreen or not. Only Exclusive FullScreen will provide the option to also select a refresh rate per resolution.
* Quality Dropdown: used with a Dropdown element to present the player with a list of the defined [Quality Settings](https://docs.unity3d.com/Manual/class-QualitySettings.html).
* Sound Volume Slider: used with a Slider element to allow the player to change the sound volume.

**Sound Volume**

The Sound Volume can be controlled per sound type. There are definitions for the Main/Overall Volume, and Music, GUI, Effects, Voice, Ambient, Other1, Other2, Other3, Other4, and Other5. You will choose which are associated to the volume slider in the `Sound Volume Slider`'s properties in the Inspector.

To control the volume of [AudioSources](https://docs.unity3d.com/ScriptReference/AudioSource.html) you need to attach a, menu: `Component > GameSettings > Sound Volume Updater` component to each object that has an `AudioSource` and associated that AudioSource with the volume updater via its properties in the Inspector. You will also choose here what sound type (Music, GUI, Effects, etc) the updater is related to.

