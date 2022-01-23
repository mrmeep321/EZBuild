using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWML.ModHelper;
using OWML.Common;
using UnityEngine;

namespace EZBuild
{
    public interface INewHorizons
    {
        void Create(Dictionary<string, object> config, IModBehaviour mod);

        void LoadConfigs(IModBehaviour mod);

        GameObject GetPlanet(string name);
    }
}
