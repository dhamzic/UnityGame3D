using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SelectableObjects
{
    public class SelectableObject : MonoBehaviour
    {
        private List<Material> defaultMaterials = new List<Material>();
        private void Awake()
        {
            Material[] materials = GetComponent<Renderer>().materials;
            for (int i = 0; i < materials.Length; i++)
            {
                defaultMaterials.Add(materials[i]);
            }
        }
        public List<Material> GetDefaultMaterials() {
            return this.defaultMaterials;
        }
    }
}
