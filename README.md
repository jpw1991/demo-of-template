# Cheb's Valheim New Item Template

A template tailored toward beginners to easily create a new item mod.

## Video Tutorial

I recommend you watch my tutorial [video]() on how to use this. You may also consult the written guide below.

## About Me

[![image1](https://imgur.com/Fahi6sP.png)](https://necrobase.chebgonaz.com)
[![image2](https://imgur.com/X18OyQs.png)](https://ko-fi.com/chebgonaz)
[![image3](https://imgur.com/4e64jQ8.png)](https://www.patreon.com/chebgonaz?fan_landing=true)

I'm a YouTuber/Game Developer/Modder who is interested in all things necromancy and minion-related. Please check out my [YouTube channel](https://www.youtube.com/channel/UCPlZ1XnekiJxKymXbXyvkCg) and if you like the work I do and want to give back, please consider supporting me on [Patreon](https://www.patreon.com/chebgonaz?fan_landing=true) or throwing me a dime on [Ko-fi](https://ko-fi.com/chebgonaz). You can also check out my [website](https://necrobase.chebgonaz.com) where I host information on all known necromancy mods, games, books, videos and also some written reviews/guides.

Thank you and I hope this template helps you! If you have questions or need help please join the [Discord](https://discord.gg/BweDFym6sc) and ping me.

## Things you'll need

**Patience** and a small degree of **technical competence** and **tenacity**. Every component of this is a rabbit hole on its own and I only skim the surface of what there is to know, and what you need to know, to pull this off.

- A GitHub account
- Git
- An IDE for compiling with:
    - On Linux, Rider is the best choice.
    - On Windows, you can use either Rider or Visual Studio
- Valheim and BepInEx installed

## Things you might need

- Blender
  - Useful for resizing, rotating, etc. the mesh.
  - Useful for converting non-FBX to FBX
- Unity (if not making the item using pure code)
- A 3D model in FBX format
- Textures for that 3D model (if they're not already inside the FBX file)

**Trivia:** FBX is a proprietary 3D format used by Unity. The best open equivalent to this is GLTF 2.0. Another common model format is OBJ.

# Written Tutorial

**Disclaimer:** This guide is intended to get you on your feet as fast as possible, but omits a lot of fundamental/important knowledge. By the end of it you will have something that works but a lot of questions. Future learning will be required from you beyond this guide.

## 1. Creating your repository with the template

![](img/create_repository.jpg)

## 2. Cloning the repository

On your command line, navigate to somewhere you want to store the project. These commands will work on both Linux and also on Windows from within the git bash terminal:

```shell
git clone your-repository-cloning-url-here
```

Replace the `your-repository-cloning-url-here` with the cloning URL of your repository. This can be either an http address which looks similar to `https://github.com/jpw1991/template-test` or an ssh address like `git@github.com:jpw1991/template-test.git`

## 3. Renaming the repository

After cloning, the repository will be identical to the template. It needs to be renamed to whatever you wish your mod to be. I have prepared two separate scripts to assist with this purpose, but you may also do it yourself by hand if you prefer.

### a) Windows

Find and edit the `RenameTemplate.ps1` file and change the `MyMod` parts to be whatever name you want to use for your mod.

Then execute the script by right clicking and clicking run with powershell. A console box will probably flash on your screen, and then you will notice that the folders and files have been renamed.

**Attention:** By default, executing scripts is disabled on Windows. To fix this, you can run `Set-ExecutionPolicy RemoteSigned` in a powershell as administrator.

### b) Linux

Find and edit the `rename_template.sh` file and change the parts of the script which have different versions of `MyMod` to be whatever name you want your mod to be and execute the script.

## 5. Adjusting the solution

Assuming you have either Rider or Visual Studio installed, double click on the solution file which will now be named something like `YourMod.sln` depending on what name you decided to assign during the renaming phase.

### 5.1 Fixing the errors

When you open the solution, it will be unable to compile because the paths will be probably be wrong. In laymans terms, the solution has no idea where to find Valheim and BepInEx and needs to be told where these things are.

Find and edit the project file in a text editor (or from within Rider/VS via right click). You need to find and fix every reference to point to something on your drive. For example:

```xml
    <Reference Include="BepInEx">
      <HintPath>..\..\..\.config\r2modmanPlus-local\Valheim\profiles\cheb-development\BepInEx\core\BepInEx.dll</HintPath>
    </Reference>
```

Might need to become:

```xml
    <Reference Include="BepInEx">
      <HintPath>C:\Program Files (x86)\Steam\steamapps\common\Valheim\profiles\cheb-development\BepInEx\core\BepInEx.dll</HintPath>
    </Reference>
```

It depends on your system. Use your patience and the aforementioned technical tenacity to figure it out.

### 5.2 Compiling

After all the errors have been fixed, the project should compile easily. After compiling you should have a `bin` folder and opening it up and drilling down further will eventually lead to a folder with many DLLs inside. One of them will be `MyMod.dll` or whatever you renamed the project to. This is what you will later install and/or distribute, along with the asset bundle (more on that later).

### 5.3 Editing

Edit the project's main C# file (probably called `MyMod.cs`) and edit the following lines near the top of the file:

```csharp
        public const string PluginGuid = "com.chebgonaz.mymod";
        public const string PluginName = "MyMod";
        public const string PluginVersion = "0.0.1";

        private string _vanillaPrefab = "SledgeDemolisher";
        private string _bundleName = "template";
        private string _meshName = "ReplacerMesh";
        private string _materialName = "ReplacerMaterial";
```

Property | Explanation
--- | ---
`PluginGuid` | This is a unique identifier for your mod. Make it something unique and best practice is for it to be a backwards URL. It doesn't need to correspond to an existing website. The main thing is that it's unique. If your name is Bob and your mod is called Vampire Sword I recommend something like `com.bobsmods.vampiresword`.
`PluginName` | Your mod's name. This should already be correct if you used the scripts. If not, change it to whatever your mod's name is.
`PluginVersion` | The mod version. While your mod is in development/unfinished, it should be less than `1.0.0`. Every time you make a fix or improvement, increase the version. This doesn't matter much at this stage, but is important should you wish to publish your mod.
`_vanillaPrefab` | This is the prefab name of the vanilla Valheim item you want to replace.
`_bundleName` | The name of your asset bundle from step 4.
`_meshName` | The name of your mesh within the bundle.
`_materialName` | The name of your material within the bundle.

### 5.4 Recompile

With the changes we just made, you can now compile again and your mod is ready to test.

## 6. Testing

Go and take your DLL file from the build folder. It will be something like `YourMod/bin/net48/YourMod.dll` and chuck it in your BepInEx folder, along with the asset bundle. They should be side by side. Start the game and everything should work. 

## 7. Packaging

Thunderstore has a standardized package format and rejects anything that doesn't conform to it. You can read more about it [here](https://thunderstore.io/c/valheim/create/docs/), but I will explain the basics:

```
Package
├── icon.png
├── manifest.json
├── plugins
│   ├── assetbundle
│   └── mod.dll
└── README.md
```

### 7.1 Copy the mod files into Package/plugins

Inside the template we've been working on, you'll see a `Package` folder. This is the Thunderstore package folder. After building your asset bundle and DLL, copy these inside the plugins folder.

### 7.2 Package README

A README.md file is required. A demo one is provided in the template. Modify it as needed.

### 7.3 manifest.json

The `manifest.json` contains all the info that the Thunderstore cares about. Every time you make changes to the mod and wish to update it, you need to increase the version within the manifest otherwise Thunderstore will just reject it.

[Semantic Versioning](https://semver.org/) is a whole topic of its own. For small changes, all you need to do is increase the number on the end eg. `1.0.0` -> `1.0.1`.

Versions that are less than `1.0.0` (like `0.0.1`) are typically what you use while your mod is unfinished. Your first release will be version `1.0.0`.

### 7.4 Zipping & Uploading

Zip the contents of the `Package` folder and then upload it to the Thunderstore and you're done.