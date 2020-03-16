using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BSS.Extension {
    public static class ResourcesEx{
        public static T Load<T>(string path,string name) where T : UnityEngine.Object {
            return Resources.LoadAll<T>(path).ToList().Find(x => x.name == name);
        }
    }
}
