using System;
using EZbuild;
using UnityEngine;
using OWML.ModHelper;
using OWML.Common;
using System.Reflection;

namespace EZBuild
{
    public class SpawnedObject
    {
        public GameObject obj;
        public Transform parent;

        private OWScene scene;

        private bool active = false;

        private delegate void onUnfreeze();

        public Planet parentPlanet;
        private Model model;

        //Make sure that the current scene matches the scene of the SpawnedObject before creating it or doing anything with it. I need to put the instantiate part in the load check

        
        public SpawnedObject(Model m, OWScene scene = OWScene.SolarSystem)
        {
            this.scene = scene;
            if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
            EZBuild.loadSpawnedObjectCollection.Add(this);
            if (active)
            {
                ConstructorHelperA(m);
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                        ConstructorHelperA(m);
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { m, scene });
                    };
                };
            }
        }

        public SpawnedObject(Model model, Transform parent, OWScene scene = OWScene.SolarSystem)
        {
            this.scene = scene;
            if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
            EZBuild.loadSpawnedObjectCollection.Add(this);
            if (active)
            {
                EZBuild.inst.ModHelper.Console.WriteLine("Constructed obj");
                ConstructorHelperB(model, parent);
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                        ConstructorHelperB(model, parent);
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { model, parent, scene });
                    };
                };
            }
        }

        public SpawnedObject(Model m, GameObject parent, OWScene scene = OWScene.SolarSystem)
        {
            this.scene = scene;
            if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
            EZBuild.loadSpawnedObjectCollection.Add(this);
            if (active)
            {
                EZBuild.inst.ModHelper.Console.WriteLine("Constructed obj");
                ConstructorHelperC(m, parent);
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                        ConstructorHelperC(m, parent);
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { m, parent, scene });
                    };
                };
            }
        }

        public SpawnedObject(Model m, float x, float y, float z, OWScene scene = OWScene.SolarSystem)
        {
            this.scene = scene;
            if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
            EZBuild.loadSpawnedObjectCollection.Add(this);
            if (active)
            {
                EZBuild.inst.ModHelper.Console.WriteLine("Constructed obj");
                ConstructorHelperD(m, x, y, z);
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                        ConstructorHelperD(m, x, y, z);
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { m, x, y, z });
                    };
                };
            }
        }

        private void ConstructorHelperA(Model m)
        {
            GameObject obj = GameObject.Instantiate(m.obj);
            this.obj = obj;
        }

        private void ConstructorHelperB(Model m, Transform parent)
        {
            GameObject obj = GameObject.Instantiate(model.obj, parent);
            this.obj = obj;
        }
        private void ConstructorHelperC(Model m, GameObject parent)
        {
            GameObject obj = GameObject.Instantiate(model.obj, parent.transform);
            this.obj = obj;
        }
        private void ConstructorHelperD(Model m, float x, float y, float z)
        {
            GameObject obj = GameObject.Instantiate(model.obj);
            obj.transform.position = new Vector3(x, y, z);
            this.obj = obj;
        }

        public void setCartesianPosition(float x, float y, float z)
        {
            if (active)
            {
                EZBuild.inst.ModHelper.Console.WriteLine("positioned obj");
                obj.transform.position = new Vector3(x, y, z);
            }
        }

        public void setPlanetaryPosition(int a, int b, int c)
        {
            if (active)
            {
                setPlanetaryPositionHelper(a, b, c);
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        setPlanetaryPositionHelper(a, b, c);
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { a, b, c });
                    };
                };
            }
        }

        private void setPlanetaryPositionHelper(float a, float b, float c)
        {
            if (active)
            {
                if (parentPlanet != null)
                {
                    EZBuild.inst.ModHelper.Console.WriteLine(parentPlanet.GetTransform().position.ToString());
                    float r = parentPlanet.radius;
                    float x = (r + c) * (float)Math.Cos((a / r) + ((float)Math.PI / 2)) * (float)Math.Sin((b / r) + ((float)Math.PI / 2));
                    float y = (r + c) * (float)Math.Sin((a / r) + ((float)Math.PI / 2)) * (float)Math.Sin((b / r) + ((float)Math.PI / 2));
                    float z = (r + c) * (float)Math.Cos((b / r) + ((float)Math.PI / 2));

                    this.obj.transform.localPosition = new Vector3((float)x, (float)y, (float)z);
                    EZBuild.inst.ModHelper.Console.WriteLine(x + " " + y + " " + z);
                    this.obj.transform.eulerAngles = Vector3.Cross(Quaternion.LookRotation(parentPlanet.GetTransform().position - this.obj.transform.position).eulerAngles, Vector3.right);
                    this.obj.SetActive(true);
                    EZBuild.inst.ModHelper.Console.WriteLine(this.obj.transform.position.ToString());
                }
                else
                {
                    EZBuild.inst.ModHelper.Console.WriteLine("Object is not attached to planet, cannot use Planetary coordinates.");
                }
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                        if (parentPlanet != null)
                        {
                            float r = parentPlanet.radius;
                            float x = (r + c) * (float)Math.Cos((a / r) + ((float)Math.PI / 2)) * (float)Math.Sin((b / r) + ((float)Math.PI / 2));
                            float y = (r + c) * (float)Math.Sin((a / r) + ((float)Math.PI / 2)) * (float)Math.Sin((b / r) + ((float)Math.PI / 2));
                            float z = (r + c) * (float)Math.Cos((b / r) + ((float)Math.PI / 2));

                            this.obj.transform.localPosition = new Vector3((float)x, (float)y, (float)z);
                            //this.obj.transform.eulerAngles = new Vector3((float)((a / r) + (Math.PI / 2)), (float)((b / r) + (Math.PI / 2)), 0);
                            this.obj.transform.eulerAngles = Vector3.Cross(Quaternion.LookRotation(parentPlanet.GetTransform().position - this.obj.transform.position).eulerAngles, Vector3.right);
                            this.obj.SetActive(true);
                            EZBuild.inst.ModHelper.Console.WriteLine(this.obj.transform.position.ToString());
                        }
                        else
                        {
                            EZBuild.inst.ModHelper.Console.WriteLine("Object is not attached to planet, cannot use Planetary coordinates.");
                        }
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { a, b, c });
                    };
                };
            }
        }

        public void setParentPlanet(Planet planet)
        {
            if (active)
            {
                EZBuild.inst.ModHelper.Console.WriteLine("obj parent");
                planet.attachSpawnedObject(this);
                parentPlanet = planet;
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                        planet.attachSpawnedObject(this);
                        parentPlanet = planet;
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { planet });
                    };
                };
            }
        }

        public void setPositionToOrigin()
        {
            if (active)
            {
                if (parent != null)
                {
                    obj.transform.position = parent.transform.position;
                    obj.transform.localPosition = Vector3.zero;
                }
                else
                {
                    obj.transform.position = Vector3.zero;
                }
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                        if (parent != null)
                        {
                            obj.transform.position = parent.transform.position;
                            obj.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            obj.transform.position = Vector3.zero;
                        }
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] {});
                    };
                };
            }
        }

        public void setEulerRotation(float pitch, float yaw, float roll)
        {
            if (active)
            {
                this.obj.transform.eulerAngles = new Vector3(pitch, yaw, roll);
            }
            else
            {
                EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        this.obj.transform.eulerAngles = new Vector3(pitch, yaw, roll);
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { pitch, yaw, roll });
                    };
                };
            }
        }

        public void setParent(GameObject obj)
        {
            if (active)
            {
                parent = this.obj.transform.parent = obj.transform;
                obj.SetActive(true);
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                        parent = this.obj.transform.parent = obj.transform;
                        obj.SetActive(true);
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { obj });
                    };
                };
            }
        }

        public void setParent(Transform obj)
        {
            if (active)
            {
                parent = this.obj.transform.parent = obj;
                this.obj.SetActive(true);
            }
            else
            {
                EZBuild.inst.loadQueue += () =>
                {
                    if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
                    if (active)
                    {
                        EZBuild.inst.ModHelper.Console.WriteLine("fired obj event");
                        parent = this.obj.transform.parent = obj;
                        this.obj.SetActive(true);
                    }
                    else EZBuild.inst.tempQueue += () =>
                    {
                        MethodBase.GetCurrentMethod().Invoke(this, new System.Object[] { obj });
                    };
                };
            }
        }

        public Vector3 getCartesianPosition()
        {
            return this.obj.transform.position;
        }

        /*public void check()
        {
            if (scene == EZBuild.inst.scene && EZBuild.nhReady) active = true; else active = false;
            EZBuild.inst.ModHelper.Console.WriteLine("Objs are " + active);
            if (active)
            {
                EZBuild.inst.loadQueue.Invoke();
                EZBuild.inst.loadQueue = EZBuild.inst.tempQueue;
                EZBuild.inst.tempQueue = null;
            }
        }*/
    }
}
