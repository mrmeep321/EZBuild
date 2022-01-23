using System;
using UnityEngine;
using EZbuild;
using System.Collections.Generic;
using System.Threading;

namespace EZbuild
{
    public class Planet
    {
        private AstroObject planet;
        private GameObject nhPlanet;
        public int radius;

        private static Dictionary<String, Planet> activeNHPlanets = new Dictionary<String, Planet>();
        private static Dictionary<AstroObject, Planet> activePlanets = new Dictionary<AstroObject, Planet>();

        private bool planetFlag;

        //Need to return old planet if it already exists when created.

        public Planet(AstroObject.Name name)
        {
            if (!activePlanets.ContainsKey(planet))
            {
                this.planet = Locator.GetAstroObject(name);
                planetFlag = false;
                activePlanets.Add(planet, this);
            }
        }

        public Planet(String NHPlanetName, int radius)
        {
            if (!EZBuild.EZBuild.nhReady) EZBuild.EZBuild.inst.LoadQueue += () =>
             {
                 ConstructorHelperB(NHPlanetName, radius);
             };
            else ConstructorHelperB(NHPlanetName, radius);
        }

        private void ConstructorHelperB(String NHPlanetName, int radius)
        {
            if (!activeNHPlanets.ContainsKey(NHPlanetName))
            {
                this.nhPlanet = GameObject.Find(NHPlanetName + "_Body").gameObject;
                if (this.nhPlanet == null) { EZBuild.EZBuild.inst.ModHelper.Console.WriteLine("Planet could not be found. Make sure your New Horizons planet was generated before creating any kind of planet objects."); return; }
                this.radius = radius;
                planetFlag = true;
                activeNHPlanets.Add(NHPlanetName, this);
            }
        }

        public void attachSpawnedObject(EZBuild.SpawnedObject obj)
        {
            if (!EZBuild.EZBuild.nhReady) EZBuild.EZBuild.inst.LoadQueue += () =>
            {
                attachSpawnedObjectHelper(obj);
            };
            else attachSpawnedObjectHelper(obj);
        }

        private void attachSpawnedObjectHelper(EZBuild.SpawnedObject obj)
        {
            obj.setParent((planetFlag) ? nhPlanet.transform : planet.transform);
        }


        //Needs to be load-safe
        public Transform GetTransform()
        {
                return (planetFlag) ? nhPlanet.transform : planet.transform;
        }
    }
}