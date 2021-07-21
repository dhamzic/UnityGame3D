using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] private string selectableTag = "ObjectSelectable";
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private UiInventory uiInventory;

        private Inventory inventory;



        private DrawerController drawerScript;
        private DrawerController doorScript;

        //Provjera je li trenutno miš preko objekta koji služi za interakciju
        bool isHovering = false;
        private Transform _selection;

        //Provjera je li PREFAB uključen
        bool objectInfoTurnedOn = false;

        private GameObject UiInventoryCanvas;

        public GameObject trenutniRoditelj;
        public Transform trenutniRoditeljManevriranje;
        float udaljenost;
        float snaga = 600;

        #region Safe
        string safePassword = "";
        bool safeIsOpened = false;
        #endregion

        private void Awake()
        {
            UiInventoryCanvas = GameObject.Find("UiInventory");
            UiInventoryCanvas.SetActive(false);
        }
        private void Start()
        {
            drawerScript = GetComponent<DrawerController>();
            inventory = new Inventory();
            uiInventory.SetInventory(inventory);
            //ItemWorld.SpawnItemWorld(new Vector3(436.0165f, -0.1f, -445.9609f), new Item { itemType = Item.ItemType.Key, amount = 1 });

            //Dohvati roditelja. Služi za prijenos objekta. Razmak između igrača i objekta prilikom premještanja
            trenutniRoditelj = GameObject.Find("FPC_ObjectHolder");
            trenutniRoditeljManevriranje = trenutniRoditelj.GetComponent<Transform>();

        }
        private void Update()
        {
            if (Input.GetKeyDown("i"))
            {
                if (UiInventoryCanvas.activeInHierarchy == true)
                {
                    UiInventoryCanvas.SetActive(false);
                }
                else
                {
                    UiInventoryCanvas.SetActive(true);
                }
            }
            if (isHovering == false)
            {
                //drawerScript.DeleteFloatingText();
                objectInfoTurnedOn = false;
            }

            if (_selection != null)
            {
                var selectionRenderer = _selection.GetComponent<Renderer>();

                Material[] materials = new Material[selectionRenderer.materials.Length];
                for (int i = 0; i < selectionRenderer.materials.Length; i++)
                {
                    materials[i] = defaultMaterial;
                }
                selectionRenderer.sharedMaterials = materials;


                
                //selectionRenderer.material = defaultMaterial;
                _selection = null;
            }

            var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.distance <= 6)
                {
                    var selection = hit.transform;
                    //if (selection.CompareTag(selectableTag))
                    if (selection.tag.Contains(selectableTag))
                    {
                        isHovering = true;

                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            Material[] materials = new Material[selectionRenderer.materials.Length];
                            for (int i = 0; i < selectionRenderer.materials.Length; i++)
                            {
                                materials[i] = highlightMaterial;
                            }
                            selectionRenderer.sharedMaterials = materials;
                            //selectionRenderer.material = highlightMaterial;

                        }
                        _selection = selection;


                        if (objectInfoTurnedOn == false)
                        {
                            //drawerScript.ShowFloatingText();
                        }
                        objectInfoTurnedOn = true;


                        //Debug.Log("Object hit: " + hit.transform.name);
                        if (Input.GetKeyDown("e"))
                        {
                            switch (hit.transform.tag)
                            {
                                case "ObjectSelectable_Door":
                                    {
                                        AnimController ac = GameObject.Find("Door").GetComponent<AnimController>();
                                        ac.StartDoorAnimation(hit.transform.name);
                                        break;
                                    }
                                case "ObjectSelectable_Drawer":
                                    {
                                        AnimController ac = GameObject.Find("Drawer").GetComponent<AnimController>();
                                        ac.StartDrawerAnimation(hit.transform.name);
                                        break;
                                    }
                                case "ObjectSelectable_Inventory":
                                    {
                                        inventory.AddItem(new Item { itemType = hit.transform.GetComponent<ItemWorld>().itemType, description = hit.transform.GetComponent<ItemWorld>().itemDescription });
                                        Destroy(hit.transform.gameObject);
                                        uiInventory.RefreshInventoryItems();
                                        break;
                                    }
                                case "ObjectSelectable_Painting":
                                    {
                                        //Podigni objekt
                                        Debug.Log("Objekt podignut");
                                        GameObject objektSlike = GameObject.Find("Painting");
                                        objektSlike.GetComponent<Rigidbody>().useGravity = false;
                                        objektSlike.GetComponent<Rigidbody>().detectCollisions = true;
                                        objektSlike.transform.parent = trenutniRoditelj.transform;
                                        objektSlike.transform.position = trenutniRoditeljManevriranje.transform.position;

                                        objektSlike.transform.localEulerAngles = new Vector3(0, 0, 0);
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        else if (Input.GetMouseButtonDown(0))
                        {
                            switch (hit.transform.tag)
                            {
                                case "ObjectSelectable_SafeButton":
                                    {
                                        if (safeIsOpened == false)
                                        {
                                            AnimController ac = GameObject.Find("WallWithSafe").GetComponent<AnimController>();
                                            ac.PressButton(hit.transform.name[hit.transform.name.Length - 1].ToString());
                                            //Animacija gumba

                                            Light ul = GameObject.Find("UnsuccessLight").GetComponent<Light>();
                                            if (ul.intensity > 0)
                                            {
                                                ul.intensity = 0;
                                            }
                                            if (this.safePassword.Length < 4)
                                            {
                                                this.safePassword = this.safePassword + hit.transform.name[hit.transform.name.Length - 1].ToString();
                                            }
                                            if (this.safePassword.Length == 4)
                                            {
                                                bool validPass = Safe.CheckPassword(this.safePassword);
                                                if (validPass)
                                                {
                                                    Light l = GameObject.Find("SuccessLight").GetComponent<Light>();
                                                    l.intensity = 3;

                                                    Debug.Log("Pass is valid: " + this.safePassword);
                                                    
                                                    ac.OpenSafe();
                                                    safeIsOpened = true;
                                                    Light sl = GameObject.Find("SafeLight").GetComponent<Light>();
                                                    sl.intensity = 3;
                                                }
                                                else
                                                {
                                                    ul.intensity = 3;

                                                    Debug.Log("Pass is invalid: " + this.safePassword);
                                                }
                                                this.safePassword = "";
                                            }
                                        }


                                        break;
                                    }
                            }
                        }

                    }
                    else
                    {
                        isHovering = false;
                    }
                }
                else
                {
                    isHovering = false;

                }
            }
            else
            {
                isHovering = false;
            }




            if (Input.GetKeyDown("t"))
            {
                GameObject objektSlike = GameObject.Find("Painting");
                objektSlike.GetComponent<Rigidbody>().useGravity = true;
                objektSlike.transform.localEulerAngles = new Vector3(3, 0, 0);
                objektSlike.transform.parent = null;
            }
        }
    }
}
