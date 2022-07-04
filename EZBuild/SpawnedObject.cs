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

        public void setCartesianPosition(float x, float y, float z)
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

        private void setPlanetaryPositionHelper(float a, float b, float c)
        {
            if (parentPlanet != null)
            {
                float r = parentPlanet.radius;
                float x = (r + c) * (float)Math.Cos((a / r) + ((float)Math.PI / 2)) * (float)Math.Sin((b / r) + ((float)Math.PI / 2));
                float y = (r + c) * (float)Math.Sin((a / r) + ((float)Math.PI / 2)) * (float)Math.Sin((b / r) + ((float)Math.PI / 2));
                float z = (r + c) * (float)Math.Cos((b / r) + ((float)Math.PI / 2));

                this.obj.transform.localPosition = new Vector3((float)x, (float)y, (float)z);
                this.obj.transform.eulerAngles = Vector3.Cross(Quaternion.LookRotation(parentPlanet.GetTransform().position - this.obj.transform.position).eulerAngles, Vector3.right);
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
            this.obj.transform.parent = obj.transform;
            obj.SetActive(true);
        }

        public void setParent(Transform obj)
        {
            parent = this.obj.transform.parent = obj;
            this.obj.SetActive(true);
        }
    }
}
