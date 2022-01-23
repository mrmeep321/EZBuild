using OWML.ModHelper;
using OWML.Common;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using EZbuild;

namespace EZBuild
{
    public class EZBuild : ModBehaviour
    {
        public static bool hasNewHorizons;
        private static INewHorizons nh;
        public static EZBuild inst;
        public static bool nhReady = false;

        public IModHelper helper;

        public delegate void loadQueue();
        public event loadQueue LoadQueue;

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
                if (loadScene != OWScene.SolarSystem) return;
                var playerBody = FindObjectOfType<PlayerBody>();
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
            LoadQueue.Invoke();
        }

        /*
         * bundlePath goes to the bundle in the project folder, prefabPath goes to the prefab inside of the bundle.
         */
        public Model loadBundleAsset(String bundlePath, String prefabPath, String modelName)
        {
            var bundle = inst.helper.Assets.LoadBundle(bundlePath);
            GameObject obj = LoadPrefab(bundle, prefabPath);
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

        public SpawnedObject spawnObject(Model model, int x, int y, int z)
        {
            GameObject obj = Instantiate(model.obj);
            obj.transform.position = new Vector3(x, y, z);
            SpawnedObject newobj = new SpawnedObject(obj);
            return newobj;
        }

        public SpawnedObject spawnObject(Model model, GameObject parent)
        {
            GameObject obj = Instantiate(model.obj, parent.transform);
            SpawnedObject newobj = new SpawnedObject(obj);
            newobj.setParent(parent);
            return newobj;
        }

        public SpawnedObject spawnObject(Model model, Transform parent)
        {
            GameObject obj = Instantiate(model.obj, parent);
            SpawnedObject newobj = new SpawnedObject(obj);
            newobj.setParent(parent);
            return newobj;
        }

        public SpawnedObject spawnObject(Model model)
        {
            GameObject obj = Instantiate(model.obj);
            SpawnedObject newobj = new SpawnedObject(obj);
            return newobj;
        }

        public Planet getNewHorizonsPlanet(String name)
        {
            return new Planet(name, 90);
        }

        public EZBuild getInstance()
        {
            return inst;
        }

        private static readonly Shader standardShader = Shader.Find("Standard");

        private static GameObject LoadPrefab(AssetBundle bundle, string path)
        {
            var prefab = bundle.LoadAsset<GameObject>(path);

            // Repair materials             
            foreach (var renderer in prefab.GetComponentsInChildren<MeshRenderer>())
            {
                foreach (var mat in renderer.materials)
                {
                    mat.shader = standardShader;
                    mat.renderQueue = 2000;
                }
            }
            foreach (Transform child in prefab.transform)
            {
                foreach (var renderer in child.GetComponentsInChildren<MeshRenderer>())
                {
                    foreach (var mat in renderer.materials)
                    {
                        mat.shader = standardShader;
                        mat.renderQueue = 2000;
                    }
                }
            }

            prefab.SetActive(false);

            return prefab;
        }
    }
}
