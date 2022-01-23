using System;
using EZbuild;
using UnityEngine;

namespace EZBuild
{
    public class SpawnedObject
    {
        public GameObject obj;
        public Transform parent;

        public Planet parentPlanet;

        public SpawnedObject(GameObject obj)
        {
            this.obj = obj;
        }

        public void setCartesianPosition(int x, int y, int z)
        {
            obj.transform.position = new Vector3(x, y, z);
        }

        public void setPlanetaryPosition(int a, int b, int c)
        {
            if (!EZBuild.nhReady) EZBuild.inst.LoadQueue += () =>
            {
                setPlanetaryPositionHelper(a, b, c);
            };
            else setPlanetaryPositionHelper(a, b, c);
        }

        private void setPlanetaryPositionHelper(int a, int b, int c)
        {
            if (parentPlanet != null)
            {
                int r = parentPlanet.radius;
                double x = (r + c) * Math.Cos((a / r) + (Math.PI / 2)) * Math.Sin((b / r) + (Math.PI / 2));
                double y = (r + c) * Math.Sin((a / r) + (Math.PI / 2)) * Math.Sin((b / r) + (Math.PI / 2));
                double z = (r + c) * Math.Cos((b / r) + (Math.PI / 2));

                this.obj.transform.localPosition = new Vector3((float)x, (float)y, (float)z);
                //this.obj.transform.eulerAngles = new Vector3((float)((a / r) + (Math.PI / 2)), (float)((b / r) + (Math.PI / 2)), 0);
                this.obj.transform.rotation = Quaternion.LookRotation(parentPlanet.GetTransform().position - this.obj.transform.position).normalized;
                this.obj.SetActive(true);
            }
            else
            {
                EZBuild.inst.ModHelper.Console.WriteLine("Object is not attached to planet, cannot use Planetary coordinates.");
            }
        }

        public void setParentPlanet(Planet planet)
        {
            planet.attachSpawnedObject(this);
            parentPlanet = planet;
        }

        public void setPositionToOrigin()
        {
            if(parent != null)
            {
                obj.transform.position = parent.transform.position;
                obj.transform.localPosition = Vector3.zero;
            } else
            {
                obj.transform.position = Vector3.zero;
            }
        }

        public void setParent(GameObject obj)
        {
            parent = this.obj.transform.parent = obj.transform;
            obj.SetActive(true);
        }

        public void setParent(Transform obj)
        {
            parent = this.obj.transform.parent = obj;
            this.obj.SetActive(true);
        }
    }
}
