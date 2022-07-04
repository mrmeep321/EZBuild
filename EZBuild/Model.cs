using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZBuild
{
    public class Model
    {
        public GameObject obj;

        public static Dictionary<String, AssetBundle> existingModels = new Dictionary<String, AssetBundle>();

        public Model(GameObject obj, String s, AssetBundle bundle)
        {
            this.obj = obj;
            existingModels.Add(s, bundle);
        }

        public Model(GameObject obj, String s)
        {
            this.obj = obj;
        }
    }
}