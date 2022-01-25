using OWML.ModHelper;
using OWML.Common;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using EZbuild;
using System.Collections.ObjectModel;

namespace EZBuild
{
    public class EZBuild : ModBehaviour
    {

        //Anything that can be in another system or not currently loaded needs functionality to check if loaded and disable the object if it isn't loaded currently. This includes planets and SpawnedObjects

        //Objects are properly spawning in-game but are not visible. They show up under unity with the proper coordinates, and can interact with players - but they do not appear visually


        public static bool hasNewHorizons;
        public static INewHorizons nh;
        public static EZBuild inst;
        public static bool nhReady = false;

        public static Collection<Planet> loadPlanetCollection = new Collection<Planet>();
        public static Collection<SpawnedObject> loadSpawnedObjectCollection = new Collection<SpawnedObject>();

        public delegate void loadEvent();
        public event loadEvent loadQueue;
        public event loadEvent tempQueue;

        public IModHelper helper;

        public OWScene scene;

        private static Dictionary<String, Model> dict = new Dictionary<String, Model>();

        public EZBuild()
        {
            if (inst == null) inst = this;
        }

        private void Awake()
        {
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        private void Start()
        {
            //remove helper thing and fix crashing
            helper = ModHelper;

            helper.Console.WriteLine($"My mod {nameof(EZBuild)} is loaded!", MessageType.Success);

            

            try
            {
                nh = helper.Interaction.GetModApi<INewHorizons>("xen.NewHorizons");
                helper.Console.WriteLine("New Horizons found");
                try
                {
                    nh.LoadConfigs(this);
                } catch
                {
                    helper.Console.WriteLine("No planet folder found inside of EZBuild");
                }
                hasNewHorizons = true;
            }
            catch (Exception e)
            {
                inst.helper.Console.WriteLine("New Horizons not found. The EZBuild.NewHorizons class will be unavailable.");
                hasNewHorizons = false;
            }

            LoadManager.OnCompleteSceneLoad += (scene, loadScene) =>
            {
                this.scene = loadScene;
                ModHelper.Console.WriteLine(this.scene.ToString() + " is the scene");
                ModHelper.Console.WriteLine(this.scene == OWScene.SolarSystem);
                Thread t = new Thread(new ThreadStart(threader));
                t.Start();
            };
        }

        private void threader()
        {
            var playerBody = FindObjectOfType<PlayerBody>();
            Thread.Sleep(1000);
            nhReady = true;
            if(hasNewHorizons) inst.helper.Console.WriteLine("New Horizons is now fully loaded.");
            loadQueue.Invoke();
            loadQueue = tempQueue;
            tempQueue = null;
        }

        /*
         * bundlePath goes to the bundle in the project folder, prefabPath goes to the prefab inside of the bundle.
         */
        public Model loadBundleAsset(String bundlePath, String prefabPath, String modelName)
        {
            var bundle = inst.helper.Assets.LoadBundle(bundlePath);
            GameObject obj = bundle.LoadAsset<GameObject>(prefabPath);
            Model g = new Model(obj);
            dict.Add(modelName, g);
            return g;
        }

        [Obsolete("loadModelAsset is obselete. Please use loadBundleAsset instead. Models generated with loadModelAsset are very unstable and often times lack the ability to parent or transform on anything but the player unless you provide them with custom Unity kinematics components.")]
        public Model loadModelAsset(String modelPath, String texturePath, String modelName = "")
        {
            GameObject obj = inst.helper.Assets.Get3DObject(modelPath, texturePath);
            Model g = new Model(obj);
            dict.Add(modelName, g);
            return g;
        }

        public Model getLoadedModel(String name)
        {
            return dict[name];
        }

        public SpawnedObject spawnObject(Model model, int x, int y, int z, OWScene scene)
        {
            SpawnedObject newobj = new SpawnedObject(model, x, y, z, scene);
            return newobj;
        }

        public SpawnedObject spawnObject(Model model, GameObject parent)
        {
            SpawnedObject newobj = new SpawnedObject(model, parent, scene);
            newobj.setParent(parent);
            return newobj;
        }

        public SpawnedObject spawnObject(Model model, Transform parent, OWScene scene)
        {
            SpawnedObject newobj = new SpawnedObject(model, parent, scene);
            newobj.setParent(parent);
            return newobj;
        }

        public SpawnedObject spawnObject(Model model, OWScene scene = OWScene.SolarSystem)
        {
            SpawnedObject newobj = new SpawnedObject(model, scene);
            return newobj;
        }

        public Planet getNewHorizonsPlanet(String name, double radius)
        {
            return new Planet(name, 90, OWScene.SolarSystem);
        }

        public EZBuild getInstance()
        {
            return inst;
        }

        public OWScene getScene()
        {
            return scene;
        }
    }
}
