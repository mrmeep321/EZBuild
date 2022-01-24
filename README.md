<h1>EZBuild</h1>

Release 1.0.0: https://github.com/mrmeep321/EZBuild/releases/tag/v1.0.0</h1>

EZBuild is a utility mod developed for OWML to simplify the process of creating and placing in-game objects and mechanics like buildings and trigger volumes.

The goal of EZBuild is to allow modders to easily develop their mechanics or objects within Unity itself, and import them into the game with minimal effort.

EZBuild is still in early stages of development, but in the end, I want this mod to become a gateway for new modders to dip their feet into the water and get into modding without having to slog through too much Unity's massive and complex API base.

If you find any bugs (i'm sure there are a lot. v1.0.0 was developed in around 7 hours) or have any suggestions for features, please either @ me in the modding discord (https://discord.gg/GZvCmcsK), my handle is @mrmeep321, or submit an issue request.

## Current functionality

Implement new custom objects into the game with Unity AssetBundles

Manage object hierarchies with these new objects

Manage object spatial position

Manage object planetary position to allow for efficient placement of buildings on planets.

Suppport for planets created with New Horizons


## Roadmap
Manage object rotation intuitively and relative to other bodies to simplify angular stuff

Inbuilt full angular and translational velocity/acceleration/position control

Interaction volume support for newly created objects to make interactable and pick-uppable objects

Animated characters and objects

Rebuilt physics support for mass/gravity relationships to be used for dynamic systems created with NewHorizons like binary stars


### Documentation 

## Installation
EZBuild is utility mod, meaning it doesn't really do anything on its own. In order to make it do anything, you'll need to access its methods by referencing it as a library.
1.  Install EZbuild using Outer Wilds Mod Loader
2.  Open up your project directory, and open your .csproj.user file
	- Inside of that file, drop the following block in right after your Output Path block.
```
<OuterWildsModDirectory>$(AppData)\OuterWildsModManager\OWML\Mods</OuterWildsModsDirectory>
```

- It should look something like this afterwards
![](https://i.imgur.com/TTm9Grw.png)


3. Still inside your project directory, open up your .csproj file
  - Inside of that file, drop the following in right before the </Project> block
```xml
<ItemGroup>
<Reference Include="EZBuild"><HintPath>$(OuterWildsModsDirectory)\mrmeep321.EZBuild\EZBuild.dll</HintPath>
<Private>False</Private>
</Reference>
</ItemGroup>
  ```
- After that, it should look something like this
![](https://i.imgur.com/JXMcCVt.png)
3. Now, restart Visual Studio, and check to make sure that EZBuild exists in your Dependencies now. You can check this using the Solution explorer, which you open by going to View -> Solution Explorer.
- When in Solution Explorer, drop down your project, and you'll see the Dependencies tab. EZBuild should be under the assemblies tab in there. It will look like this if you did it right.

![](https://i.imgur.com/VNhGC7F.png)

3. EZBuild should automatically load up before your mod does since it's counted as a dependency.

# Usage

EZBuild has a few different class types to learn about before using. Each of them is generated using methods from the EZBuild class.

## EZBuild

EZBuild is the main class for the API, and contains most of the methods you'll use. EZBuild automatically detects and uses NewHorizons if it's installed. If it isn't installed, methods that use NewHorizons won't work (obviously). Now, This mod was made in a pretty limited amount of time, so I haven't really gotten around to actually disabling NH methods when NH isn't installed, so **please for the love of god don't try and generate objects on a New Horizons planet. Your computer will explode.** This will be fixed in a later release.

### Methods
```C#
EZBuild.inst.loadBundleAsset(String bundlePath, String prefabPath, String modelName);
```
This method generates and returns a [Model](#model) based on the parameters which are as follows
- bundlePath - The path of your AssetBundle
- prefabPath - The path within your AssetBundle that points towards the prefab you want to use
- modelName - your chosen name for this Model. At this point, this doesn't really do anything.

There is a method called loadModelAsset within EZBuild that can load an object into the game using just a .obj and .png texture, but using a raw .obj file is a very bad idea unless you want to manually assign a ton of Unity components in-script. I would highly recommend using AssetBundles. Instructions for making these are under the OWML docs, and I'll be making my own guide soon.

```C#
EZBuild.inst.spawnObject(Model model, int x, int y, int z);
EZBuild.inst.spawnObject(Model model, GameObject parent);
EZBuild.inst.spawnObject(Model model, Transform parent);
EZBuild.inst.spawnObject(Model model);
```
This method returns a [SpawnedObject](#spawnedobject), which we'll get into later. The creation of a SpawnedObject will generate a physical object in the world that uses the Model provided in the method. Providing an x, y, and z will set the starting position (See Coordinate systems to see how these work), and providing either a GameObject or Transform will set the default Parent (see [object hierarchies](#object-hierarchies) for more details).

## Model
The Model class creates objects that store imported Unity asset data. These essentially just represent the imported objects that you create by loading AssetBundles. To create one of these, use the loadBundleAsset method or the loadModelAsset method if you're insane.

## SpawnedObject
SpawnObjects are essentiially a code hook that connects to a physically spawned object in game.
