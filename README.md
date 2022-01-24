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


# Documentation 

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

## Usage

EZBuild has a few different class types to learn about before using. Each of them is generated using methods from the EZBuild class.

### EZBuild

EZBuild is the main class for the API, and contains most of the methods you'll use. EZBuild automatically detects and uses NewHorizons if it's installed. If it isn't installed, methods that use NewHorizons won't work (obviously). Now, This mod was made in a pretty limited amount of time, so I haven't really gotten around to actually disabling NH methods when NH isn't installed, so **please for the love of god don't try and generate objects on a New Horizons planet. Your computer will explode.** This will be fixed in a later release.

#### Methods
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

```C#
EZBuild.inst.getNewHorizonsPlanet(String name, double radius);
```
This method returns a [Planet](#planet) object which stores the data for a New Horizons planet. The name parameter specifies the name given to it by the .json file, and the radius corresponds to the distance from the core to the "surface". I know there are multi-level planets, but this is effectively where you want (x, y, 0) to be on [the planetary mapping coordinates for your planet](#planetary-coordinates).

```C#
EZBuild.inst.getLoadedModel(String name)
```
If somehow you lose one of your models, EZBuild stores them dictionarily. Drop the model name specified in the loadBundleAsset method and it'll spit out the existing model.

## Model
The Model class creates objects that store imported Unity asset data. These essentially just represent the imported objects that you create by loading AssetBundles. To create one of these, use the loadBundleAsset method or the loadModelAsset method if you're insane.

## SpawnedObject
SpawnObjects are essentiially a code hook that connects to a physically spawned object in game.

Please don't try to construct these yourself unless you fully understand what's going on in the class, just use EZBuild.spawnObject to make one.
```C#
setCartesianPosition(x, y, z)
```
This method just sets the position of the object to (x, y, z) in [cartesian space](#cartesian-coordinates). 

```C#
setPlanetaryPosition(x, y, z)
```
This method sets the object's position to (x, y, z) in [planetary mapping coordinate space](#planetary-coordinates) relative to its [parent](#parent) [Planet](#planet)
```C#
setParentPlanet(planet)
```
This method sets the object's [parent](#parent) planet.
```C#
setPositionToOrigin()
```
This is the same as calling setCartesianPosition(0, 0, 0)
```C#
setParent(object)
setParent(transform)
```
This sets the object's parent to a GameObject or Transform (good for attaching object to characters or such.

### Planet
Planet objects represent a pre-existing planet or one generated by New Horizons.

Constructor is as follows
```C#
new Planet(String nhPlanetName, float radius)
new Planet(AstroObject.Name name)
```
Providing a string name and radius will generate a code hook for the New Horizons planet with that name and set it's sea level to the radius.
Providing an AstroObject.Name will grab a pre-existing planet. Typing AstroObject.Name. will make visual studio pull up all available planets to grab.
```C#
attachSpawnedObject(SpawnedObject obj)
```
This method will make the parameterized SpawnedObject into a child of this planet. This method will take said object away from its parents, so be sure that you're ready to transfer it over to this planet when you call it.
```C#
getTransform()
```
Take a guess.
<details>
	<summary>No really, guess.</summary>
	It returns the transform of the planet.
</details>
##Concepts
This section is dedicated to helping you if you need help understanding the concepts as to how certain parts of this mod, and game work, like how Outer Wilds' coordinate systems work, or how my custom coordinate systems work, or how the physics of the game work. I'll be periodically updating this as time goes on.
###Coordinate Systems
There are a ton of different Coordinate systems, from cartesian, to polar, to spherical, and even some weird, multi-dimensional ones.
I would highly recommend either taking a class in, or watching some KhanAcademy videos on how 3 or more dimensional space works, and then going and researching how Vectors in 3 dimensions act. It may seem tedious at first, but it will make understanding Outer Wilds' physics **much** easier.
####Cartesian Coordinates
Cartesian Coordinates are the simplest type of coordinates. A lot of time, these are called "rectangular coordinates" or "cubic coordinates", because they go along straight lines. These are the normal coordinates that you're used to, with an X, Y, and Z axes, and each coordinate of a point corresponds to displacement from zero along each axis.
####Polar Coordinates
These are a little less common, but are still incredibly useful. These are sometimes called "circular coordinates" or in 3 dimensions, "spherical coordinates". Polar coordinates are, in effect, just a direction and a magnitude (just like a vector). We usually express these coordinates in 2d as (m, Θ), where theta is the angle that the vector bounded by your point and the origin makes with the X axis, and m is the distance from the origin. In 3 dimensions, this gets a little more complex, being expressed as (m, Θ, Φ), with m being, again, the distance to the origin, but this time, theta is the angle made with the X axis inside of the XZ plane, and phi is the angle the line makes with the X axis in the XY plane.
####Planetary Coordinates
These coordinates are not something you'll see very often, but they're incredibly useful when we want to place an object on a relatively large spherical object in space. In effect, these work like latitude and longitude, with (0, 0) being placed on the north pole, and as you walk across the surface of the planet, the x axis goes from North-South, and the y axis goes from East-West. the Z axis is the relative vertical distance from the sea level of the planet. Thus, the north pole on the surface at sea level would be (0, 0, 0), and the south pole would be (πr, πr, 0). Methods in this mod that set planetary position also automatically cause your object to rotate to sit level on the ground.
