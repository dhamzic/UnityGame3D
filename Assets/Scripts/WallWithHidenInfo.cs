using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class WallWithHidenInfo : MonoBehaviour
    {
        private Material defaultMaterial;
        public  Material hiddenMessageMaterial;
        private void Awake()
        {
            Material[] materials = GetComponent<Renderer>().materials;
            defaultMaterial = materials[0];
        }
        public Material GetDefaultMaterial()
        {
            return this.defaultMaterial;
        }
    }
}
