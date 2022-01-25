using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;

namespace EZbuild
{
    public class Planet
    {

        //Finish event queueing and event calling on scene load for planet and then do SpawnedObject.

        private AstroObject planet;
        private GameObject nhPlanet;

        private AstroObject.Name name;
        private String nhPlanetName;
            
        private OWScene scene;

        public float radius;

        private bool active = false;

        private delegate void onUnfreeze();

        private static Dictionary<String, Planet> activeNHPlanets = new Dictionary<String, Planet>();
        private static Dictionary<AstroObject, Planet> activePlanets = new Dictionary<AstroObject, Planet>();

        private bool planetFlag;

        //Need to return old planet if it already exists when created.

        public Planet(AstroObject.Name name)
        {
            this.name = name;
            this.scene = OWScene.SolarSystem;
            EZBuild.EZBuild.loadPlanetCollection.Add(this);
            if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
            if (active)
            {
                EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("Constructed planet");
                ConstructorHelperA(name);
            } else
            {
                EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("fired planet event");
                EZBuild.EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("fired planet event");
                        ConstructorHelperA(name);
                        EZBuild.EZBuild.inst.ModHelper.Console.WriteLine(nhPlanet.transform.position.ToString());
                    }
                    else EZBuild.EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { name });
                    };
                };
            }
        }

        private void ConstructorHelperA(AstroObject.Name name)
        {
            if (!activePlanets.ContainsKey(planet))
            {
                this.planet = Locator.GetAstroObject(name);
                if (this.planet != null) active = true; else active = false;
                planetFlag = false;
                activePlanets.Add(planet, this);
            }
        }

        public Planet(String NHPlanetName, float radius, OWScene scene = OWScene.SolarSystem)
        {
            EZBuild.EZBuild.loadPlanetCollection.Add(this);
            this.nhPlanetName = NHPlanetName;
            this.scene = scene;
            if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
            if (active)
            {
                EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("Constructed planet");
                ConstructorHelperBEventQueuer(NHPlanetName, radius, scene);
            }  else
            {
                EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("fired planet event");
                EZBuild.EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("fired planet event");
                        ConstructorHelperBEventQueuer(NHPlanetName, radius, scene);
                        EZBuild.EZBuild.inst.ModHelper.Console.WriteLine(nhPlanet.transform.position.ToString());
                    }
                    else EZBuild.EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { NHPlanetName, radius, scene });
                    };
                };
            }
        }

        private void ConstructorHelperBEventQueuer(String NHPlanetName, float radius, OWScene scene)
        {
            if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
            if (active)
            {
                if (!activeNHPlanets.ContainsKey(NHPlanetName))
                {
                    this.scene = scene;
                    this.nhPlanet = GameObject.Find(NHPlanetName + "_Body").gameObject;
                    if (this.nhPlanet == null) { EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("Planet could not be found. Make sure your New Horizons planet was generated before creating any kind of planet objects."); return; }
                    this.radius = radius;
                    planetFlag = true;
                    activeNHPlanets.Add(NHPlanetName, this);
                }
            }
            else EZBuild.EZBuild.inst.helper.Console.WriteLine("This planet does not yet exit. Please call the method after it has loaded by checking to ensure that the loaded scene is, in fact the correct one.");
        }

        public void attachSpawnedObject(EZBuild.SpawnedObject obj)
        {
            if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
            if (active)
            {
                    if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
                    EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("fired planet event");
                    attachSpawnedObjectHelper(obj);
            }
            else
            {
                EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("added planet event");
                EZBuild.EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("fired planet event");
                        attachSpawnedObjectHelper(obj);
                        EZBuild.EZBuild.inst.ModHelper.Console.WriteLine(nhPlanet.transform.position.ToString());
                    }
                    else EZBuild.EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { obj });
                    };
                };
            }
        }

        private void attachSpawnedObjectHelper(EZBuild.SpawnedObject obj)
        {
            if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
            if (active)
            {
                obj.setParent((planetFlag) ? nhPlanet.transform : planet.transform);
            }
            else
            {
                EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("fired planet event");
                EZBuild.EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        obj.setParent((planetFlag) ? nhPlanet.transform : planet.transform);
                        EZBuild.EZBuild.inst.ModHelper.Console.WriteLine(nhPlanet.transform.position.ToString());
                    }
                    else EZBuild.EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { obj });
                    };
                };
            }
        }


        //Needs to be load-safe
        public Transform GetTransform()
        {
            if (scene == EZBuild.EZBuild.inst.scene && EZBuild.EZBuild.nhReady) active = true; else active = false;
            if (active)
            {
                return (planetFlag) ? nhPlanet.transform : planet.transform;
            }
            else EZBuild.EZBuild.inst.helper.Console.WriteLine("This planet does not yet exit. Please call the method after it has loaded by checking to ensure that the loaded scene is, in fact the correct one.");
            return null;
        }

        /*public void check()
        {
            EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("Planets are " + active);
            if (EZBuild.EZBuild.nhReady)
            {
                EZBuild.EZBuild.inst.loadQueue.Invoke();
                EZBuild.EZBuild.inst.loadQueue = EZBuild.EZBuild.inst.tempQueue;
                EZBuild.EZBuild.inst.tempQueue = null;
            }
        }*/
    }
}