using Assets.Scripts.SelectableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] private string selectableTag = "ObjectSelectable";

        [SerializeField] private Material highlightMaterial;
        //[SerializeField] private Material defaultMaterial;

        [SerializeField] private UiInventory uiInventory;

        private Inventory inventory;


        private bool CurvedDrawerUnLocked = false;
        private DrawerController drawerScript;
        private DrawerController doorScript;

        //Provjera je li trenutno miš preko objekta koji služi za interakciju
        bool isHovering = false;
        private Transform _selection;

        //Provjera je li PREFAB uključen
        bool objectInfoTurnedOn = false;

        private GameObject UiInventoryCanvas;
        private GameObject UiInventoryRead;

        public GameObject trenutniRoditelj;
        public Transform trenutniRoditeljManevriranje;
        float udaljenost;
        float snaga = 600;

        #region Safe
        string safePassword = "";
        bool safeIsOpened = false;
        #endregion

        int inventoryItemId = 1;

        GameObject objektSlike = null;

        GameObject objektKocke = null;

        private GameObject istocniZid = null;

        bool switchTurnedOn = false;

        bool objectRaised = false;

        private void Awake()
        {
            inventory = new Inventory();
            uiInventory.SetInventory(inventory);

            UiInventoryCanvas = GameObject.Find("UiInventory");
            UiInventoryCanvas.SetActive(false);

            UiInventoryRead = GameObject.Find("UiInventoryRead");
            UiInventoryRead.SetActive(false);

            this.objektSlike = GameObject.Find("Painting");

            this.objektKocke = GameObject.Find("Cube_A");
            //objektSlike.GetComponent<Rigidbody>().detectCollisions = false;

            istocniZid = GameObject.Find("WallEast");
        }
        private void Start()
        {
            drawerScript = GetComponent<DrawerController>();

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

                SelectableObject selectedObject = _selection.GetComponent<SelectableObject>();


                Material[] materials = new Material[selectionRenderer.materials.Length];
                for (int i = 0; i < selectionRenderer.materials.Length; i++)
                {
                    materials[i] = selectedObject.GetDefaultMaterials()[i];
                }
                selectionRenderer.sharedMaterials = materials;

                //Material[] materials = new Material[selectionRenderer.materials.Length];
                //for (int i = 0; i < selectionRenderer.materials.Length; i++)
                //{
                //    materials[i] = defaultMaterial;
                //}
                //selectionRenderer.sharedMaterials = materials;




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
                                        //AnimController ac = GameObject.Find("Door").GetComponent<AnimController>();
                                        //ac.StartDoorAnimation(hit.transform.name);
                                        //break;

                                        AnimController ac = GameObject.Find("Door").GetComponent<AnimController>();
                                        ac.StartDoorAnimation();
                                        break;
                                    }
                                case "ObjectSelectable_Switch":
                                    {
                                        AnimController ac = GameObject.Find("Switch").GetComponent<AnimController>();
                                        ac.StartSwitchAnimation(this.switchTurnedOn);

                                        if (this.switchTurnedOn)
                                        {
                                            this.switchTurnedOn = false;
                                            var wallRenderer = this.istocniZid.GetComponent<Renderer>();
                                            WallWithHidenInfo wall = this.istocniZid.GetComponent<WallWithHidenInfo>();
                                            if (wallRenderer != null)
                                            {
                                                Material[] materials = new Material[wallRenderer.materials.Length];
                                                materials[0] = wall.GetDefaultMaterial();
                                                materials[1] = wall.GetDefaultMaterial();
                                                wallRenderer.sharedMaterials = materials;

                                                Light sl = GameObject.Find("UvLamp").GetComponent<Light>();
                                                sl.intensity = 0;
                                            }
                                        }
                                        else
                                        {
                                            this.switchTurnedOn = true;
                                            var wallRenderer = this.istocniZid.GetComponent<Renderer>();
                                            WallWithHidenInfo wall = this.istocniZid.GetComponent<WallWithHidenInfo>();
                                            if (wallRenderer != null)
                                            {
                                                Material[] materials = new Material[wallRenderer.materials.Length];
                                                materials[0] = wall.GetDefaultMaterial();
                                                materials[1] = wall.hiddenMessageMaterial;
                                                wallRenderer.sharedMaterials = materials;

                                                Light sl = GameObject.Find("UvLamp").GetComponent<Light>();
                                                sl.intensity = 1.2f;
                                            }
                                        }
                                        break;
                                    }
                                case "ObjectSelectable_Drawer":
                                    {
                                        if (hit.transform.name.Contains("Cube"))
                                        {
                                            AnimController ac = GameObject.Find("DrawerCube").GetComponent<AnimController>();
                                            ac.StartDrawerAnimation(hit.transform.name);
                                            Light sl = GameObject.Find("DrawerCubeChildLight").GetComponent<Light>();
                                            sl.intensity = 1.5f;
                                            break;
                                        }
                                        if (CurvedDrawerUnLocked)
                                        {
                                            AnimController ac = GameObject.Find("CurvedDrawer").GetComponent<AnimController>();
                                            ac.StartDrawerAnimation(hit.transform.name);
                                        }
                                        else
                                        {
                                            Debug.Log("Drawer is locked.");
                                        }
                                        break;
                                    }
                                case "ObjectSelectable_Inventory":
                                    {
                                        inventory.AddItem(new Item { itemType = hit.transform.GetComponent<ItemWorld>().itemType, description = this.inventoryItemId.ToString(), inventoryImage = hit.transform.GetComponent<ItemWorld>().inventoryImage, id = hit.transform.GetComponent<ItemWorld>().id });
                                        Destroy(hit.transform.gameObject);
                                        uiInventory.RefreshInventoryItems();
                                        this.inventoryItemId++;
                                        if (hit.transform.name == "SvahiliNote")
                                        {
                                            Light sl = GameObject.Find("DrawerCubeChildLight").GetComponent<Light>();
                                            sl.intensity = 0;
                                        }
                                        break;
                                    }
                                case "ObjectSelectable_Painting":
                                    {
                                        //Podigni objekt
                                        Debug.Log("Objekt podignut");
                                        objektSlike.GetComponent<Rigidbody>().useGravity = false;
                                        objektSlike.GetComponent<Rigidbody>().detectCollisions = true;
                                        objektSlike.transform.parent = trenutniRoditelj.transform;
                                        objektSlike.transform.position = trenutniRoditeljManevriranje.transform.position;

                                        objektSlike.transform.localEulerAngles = new Vector3(0, 0, 0);
                                        break;
                                    }
                                case "ObjectSelectable_Cube":
                                    {
                                        if (objectRaised == false)
                                        {
                                            //Podigni objekt
                                            Debug.Log("Objekt podignut");
                                            objektKocke.GetComponent<Rigidbody>().useGravity = false;
                                            objektKocke.GetComponent<Rigidbody>().detectCollisions = true;
                                            objektKocke.transform.parent = trenutniRoditelj.transform;
                                            objektKocke.transform.position = trenutniRoditeljManevriranje.transform.position;

                                            objektKocke.transform.localEulerAngles = new Vector3(0, 0, 0);
                                            objectRaised = true;
                                        }
                                        else {

                                            //TODO: Ne treba biti označen kad ga bacam
                                            objektKocke.GetComponent<Rigidbody>().useGravity = true;
                                            objektKocke.transform.localEulerAngles = new Vector3(0, 0, 0);
                                            objektKocke.transform.parent = null;
                                            objectRaised = false;
                                            //Light sl = GameObject.Find("Point Light_1").GetComponent<Light>();
                                            //sl.intensity = 4;
                                        }
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


            if (Input.GetKeyDown("1"))
            {
                if (UiInventoryCanvas.activeInHierarchy == true)
                {
                    InventoryKeyManipulation("1");
                }
            }
            else if (Input.GetKeyDown("2"))
            {
                if (UiInventoryCanvas.activeInHierarchy == true)
                {
                    InventoryKeyManipulation("2");
                }
            }
            else if (Input.GetKeyDown("3"))
            {
                if (UiInventoryCanvas.activeInHierarchy == true)
                {
                    InventoryKeyManipulation("3");
                }
            }
            else if (Input.GetKeyDown("4"))
            {
                if (UiInventoryCanvas.activeInHierarchy == true)
                {
                    InventoryKeyManipulation("4");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                UiInventoryRead.SetActive(false);
            }


            if (Input.GetKeyDown("t"))
            {
                objektSlike.GetComponent<Rigidbody>().useGravity = true;
                objektSlike.transform.localEulerAngles = new Vector3(0, 0, 0);
                objektSlike.transform.parent = null;
            }
        }
        private void InventoryKeyManipulation(string number)
        {
            Item selectedItem = this.inventory.GetItemList().Where(id => id.description == number).FirstOrDefault();

            if (selectedItem != null)
            {
                if (selectedItem.itemType == Item.ItemType.Key)
                {
                    if (_selection != null)
                    {
                        if (_selection.name == "CurvedDrawer")
                        {
                            switch (selectedItem.id)
                            {
                                case 11:
                                    _selection.GetComponent<BoxCollider>().enabled = false;
                                    this.CurvedDrawerUnLocked = true;
                                    Debug.Log("Curved drawer is unlocked.");
                                    inventory.RemoveItem(selectedItem);
                                    //uiInventory.RefreshInventoryItems();
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    UiInventoryRead.SetActive(true);
                    Transform rawImage = UiInventoryRead.transform.GetChild(0).GetChild(0);
                    rawImage.GetComponent<RawImage>().texture = selectedItem.inventoryImage;
                }

            }
        }
    }
}
