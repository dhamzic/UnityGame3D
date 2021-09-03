using Assets.Scripts.SelectableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Room2
{
    public class SelectionManager : MonoBehaviour
    {
        public AudioSource morseCodeSound;
        [SerializeField] private string selectableTag = "ObjectSelectable";

        [SerializeField] private Material highlightMaterial;
        //[SerializeField] private Material defaultMaterial;

        [SerializeField] private UiInventory uiInventory;

        private Inventory inventory;

        private bool DoorUnLocked = false;

        private bool KeyHoleUnlocked = false;
        private ObjectManipulationText objectManipulation;

        //Provjera je li trenutno miš preko objekta koji služi za interakciju
        bool isHovering = false;
        private Transform _selection;

        ////Provjera je li PREFAB uključen
        //bool objectInfoTurnedOn = false;

        private GameObject UiInventoryCanvas;
        private GameObject UiInventoryRead;
        private GameObject MorseButton;

        public GameObject trenutniRoditelj;
        public Transform trenutniRoditeljManevriranje;

        private GameObject RemoteControlCanvas;
        private GameObject RemoteControl;

        private bool DrawerUnlocked = false;

        #region Safe
        string safePassword = "";
        bool safeIsOpened = false;
        #endregion

        int inventoryItemId = 1;


        bool switchTurnedOn = false;

        bool objectRaised = false;
        private GameObject raisedObject = null;

        private bool ropeDroped = false;
        public GameObject ropeWithoutKey;

        public Text remoteText;


        private void Awake()
        {
            ropeWithoutKey = GameObject.Find("RopeWithoutKey");
            ropeWithoutKey.SetActive(false);

            inventory = new Inventory();
            uiInventory.SetInventory(inventory);

            UiInventoryCanvas = GameObject.Find("UiInventory");
            UiInventoryCanvas.SetActive(false);

            RemoteControlCanvas = GameObject.Find("UiRemoteControllerCanvas");
            RemoteControlCanvas.SetActive(false);

            RemoteControl = GameObject.Find("RemoteControl");

            MorseButton = GameObject.Find("Button");
        }
        private void Start()
        {
            objectManipulation = GetComponent<ObjectManipulationText>();

            //ItemWorld.SpawnItemWorld(new Vector3(436.0165f, -0.1f, -445.9609f), new Item { itemType = Item.ItemType.Key, amount = 1 });

            //Dohvati roditelja. Služi za prijenos objekta. Razmak između igrača i objekta prilikom premještanja
            trenutniRoditelj = GameObject.Find("FPC_ObjectHolder");
            trenutniRoditeljManevriranje = trenutniRoditelj.GetComponent<Transform>();

            #region Cylinders
            GameObject c1 = GameObject.Find("Cylinder_Red");
            cylinder_1 = c1.GetComponent<Cylinder>();
            GameObject c2 = GameObject.Find("Cylinder_Orange");
            cylinder_2 = c2.GetComponent<Cylinder>();
            GameObject c3 = GameObject.Find("Cylinder_Blue");
            cylinder_3 = c3.GetComponent<Cylinder>();
            GameObject c4 = GameObject.Find("Cylinder_Green");
            cylinder_4 = c4.GetComponent<Cylinder>();
            #endregion

            //TODO
            //InvokeRepeating("PositionCylinder", 0.0f, 0.00002f);
        }
        public IEnumerator WaitCubeDrop()
        {
            yield return new WaitForSeconds(1f);
            raisedObject.tag = "ObjectSelectable_Cube";
        }
        public IEnumerator WaitCubeRise()
        {
            yield return new WaitForSeconds(0.1f);
            raisedObject.GetComponent<BoxCollider>().enabled = false;
        }

        #region Cylinders
        Cylinder cylinder_1 = null;
        Cylinder cylinder_2 = null;
        Cylinder cylinder_3 = null;
        Cylinder cylinder_4 = null;
        #endregion

        bool shelfOpened = false;

        private void ThrowCube()
        {

        }
        void Position()
        {
            GameObject cube = GameObject.Find("Cube");
            if (cube.transform.position.x > -14.75f)
            {
                cube.transform.position = new Vector3(cube.transform.position.x - 0.003f, cube.transform.position.y, cube.transform.position.z);
            }
            else
            {
                CancelInvoke("Position");
            }
        }
        void PositionCylinder()
        {
            GameObject cylinder = GameObject.Find("Cylinder");
            if (cylinder.transform.position.y <= 1.56f)
            {
                cylinder.transform.position = new Vector3(cylinder.transform.position.x, cylinder.transform.position.y + 0.003f, cylinder.transform.position.z);
            }
            else
            {
                CancelInvoke("PositionCylinder");
            }
        }

        private void Update()
        {
            //Provjerava je li došlo do pogotka između
            //Bačenih kocki i ležećih cilindra
            if (ropeDroped == false)
            {
                CubeCylinderMatch();
            }

            //Vrijedi za korištenje kocki.
            //Ako se kocka trenutno nosi i slijedi
            //njeno bacanje, potrebno je ponovno uključiti
            //opcije BoxCollider-a, Rigidbody-a.
            if (objectRaised == true)
            {
                if (Input.GetKeyDown("e"))
                {
                    raisedObject.tag = "Untagged";
                    raisedObject.GetComponent<BoxCollider>().enabled = true;
                    raisedObject.GetComponent<Rigidbody>().useGravity = true;
                    raisedObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                    raisedObject.transform.parent = null;
                    objectRaised = false;
                    //Pričekaj da kocka padne kako bi ponovno mogla biti
                    //selektirana jer ne želimo da je korisniku tijekom
                    //nošenja stalno highlight-ana kocka
                    StartCoroutine("WaitCubeDrop");
                }
            }

            //Korištenje Inventory-a
            if (Input.GetKeyDown("i"))
            {
                if (UiInventoryCanvas.activeInHierarchy == true)
                {
                    UiInventoryCanvas.SetActive(false);
                }
                else
                {
                    UiInventoryCanvas.SetActive(true);
                    objectManipulation.currentManipulationText.text = "Select Inventory Item By Entering Required ID";
                }
            }

            //Ako korisnik nije selektirao niti jedan objekt
            //sakri ponuđeni tekst za manipulaciju.
            if (isHovering == false)
            {
                //objectInfoTurnedOn = false;
                if (UiInventoryCanvas.activeInHierarchy == false)
                {
                    objectManipulation.currentManipulationText.text = "";
                }
            }

            //Vraća prvotne teksture selektiranih objekata
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

                _selection = null;
            }

            //Ciljnik za selektiranje točno na vertikalnoj i horizontalnoj sredini
            var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Ukoliko je fokusiran objekt koji ima box collider
            if (Physics.Raycast(ray, out hit))
            {
                //Udaljenost od fokusiranog objekta
                if (hit.distance <= 6)
                {
                    //Debug.Log("Objekt je fokusiran: " + hit.transform.name);

                    //Prikaži tekst manipulacije objekta
                    if (UiInventoryCanvas.activeInHierarchy == false)
                    {
                        objectManipulation.ShowFloatingText(hit.transform.name, hit.transform.tag);
                    }
                    var selection = hit.transform;

                    //Ukoliko je fokusirani objekt kategorije manipulacije
                    if (selection.tag.Contains(selectableTag))
                    {
                        isHovering = true;

                        //Fokusirani objekt dobiva žuti material
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            Material[] materials = new Material[selectionRenderer.materials.Length];
                            for (int i = 0; i < selectionRenderer.materials.Length; i++)
                            {
                                materials[i] = highlightMaterial;
                            }
                            selectionRenderer.sharedMaterials = materials;
                        }
                        _selection = selection;

                        //Ukoliko je za fokusirani objekt pritisnuta tipka e 
                        if (Input.GetKeyDown("e"))
                        {
                            switch (hit.transform.tag)
                            {
                                case "ObjectSelectable_Switch":
                                    {
                                        AnimController ac = GameObject.Find("Switch").GetComponent<AnimController>();
                                        ac.StartSwitchAnimation(this.switchTurnedOn);
                                        InvokeRepeating("Position", 0.0f, 0.00002f);
                                        hit.transform.tag = "Untagged";
                                        GameObject cube = GameObject.Find("Cube_Red");
                                        cube.tag = "ObjectSelectable_Cube";
                                        cube = GameObject.Find("Cube_Blue");
                                        cube.tag = "ObjectSelectable_Cube";
                                        cube = GameObject.Find("Cube_Green");
                                        cube.tag = "ObjectSelectable_Cube";
                                        cube = GameObject.Find("Cube_Orange");
                                        cube.tag = "ObjectSelectable_Cube";
                                        break;
                                    }
                                case "ObjectSelectable_Keyhole":
                                    {
                                        if (KeyHoleUnlocked)
                                        {

                                        }
                                        break;
                                    }
                                //Objekti koji se skupljaju u Inventory
                                case "ObjectSelectable_Inventory":
                                    {
                                        inventory.AddItem(new Item
                                        {
                                            itemType = hit.transform.GetComponent<ItemWorld>().itemType,
                                            actionKey = this.inventoryItemId.ToString(),
                                            inventoryImage = hit.transform.GetComponent<ItemWorld>().inventoryImage,
                                            id = hit.transform.GetComponent<ItemWorld>().id
                                        });
                                        //Nakon što je objekt prikupljen potrebno ga je uništiti iz scene
                                        Destroy(hit.transform.gameObject);

                                        if (hit.transform.name == "Rope")
                                        {
                                            this.ropeWithoutKey.SetActive(true);
                                        }

                                        //Osvježi inventory kako bi prikazao novo prikupljeni objekt
                                        //uiInventory.RefreshInventoryItems();
                                        this.inventoryItemId++;
                                        //Ugasi svjetlo u ladici
                                        if (hit.transform.name == "SvahiliNote")
                                        {
                                            Light sl = GameObject.Find("DrawerCubeChildLight").GetComponent<Light>();
                                            sl.intensity = 0;
                                        }
                                        break;
                                    }
                                case "ObjectSelectable_Cube":
                                    {
                                        GameObject selectedCube = GameObject.Find(hit.transform.name);
                                        //Ako kocka nije podignut, dopusti podizanje
                                        //Sprijeci podizanje kocke ukoliko je već podignuta
                                        if (objectRaised == false)
                                        {
                                            //Podigni objekt
                                            //Debug.Log("Objekt podignut");
                                            //if (selectedCube.GetComponent<Rigidbody>()== null) {
                                            //    Rigidbody gameObjectsRigidBody = selectedCube.AddComponent<Rigidbody>();
                                            //}

                                            //Ugasi detektiranje kolizije kako ne bi zapinjao sa kockom prilikom nošenja
                                            selectedCube.GetComponent<Rigidbody>().useGravity = false;
                                            selectedCube.GetComponent<Rigidbody>().detectCollisions = true;
                                            selectedCube.transform.parent = trenutniRoditelj.transform;
                                            selectedCube.transform.position = trenutniRoditeljManevriranje.transform.position;

                                            selectedCube.transform.localEulerAngles = new Vector3(0, 0, 0);
                                            //selectedCube.GetComponent<BoxCollider>().enabled = false;
                                            objectRaised = true;
                                            raisedObject = selectedCube;

                                            //Pričekaj da se kocka digne pa tek onda ugasi collider
                                            //Razlog tome je što cilindar za detektiranje kolizije
                                            //ne prepoznaje da je objekt maknut sa istog ukoliko ne postoji collider
                                            //Zato se čeka 0.1s da se kocka digne pa se tek onda collider ugasi
                                            StartCoroutine("WaitCubeRise");

                                        }
                                        else
                                        {

                                            ////TODO: Ne treba biti označen kad ga bacam
                                            //selectedCube.GetComponent<Rigidbody>().useGravity = true;
                                            //selectedCube.transform.localEulerAngles = new Vector3(0, 0, 0);
                                            //selectedCube.transform.parent = null;
                                            //objectRaised = false;
                                            ////Light sl = GameObject.Find("Point Light_1").GetComponent<Light>();
                                            ////sl.intensity = 4;
                                        }
                                        break;
                                    }
                                case "ObjectSelectable_Readable":
                                    {
                                        UiInventoryRead.SetActive(true);
                                        Transform rawImage = UiInventoryRead.transform.GetChild(0).GetChild(0);
                                        ItemWorld iw = hit.transform.GetComponent<ItemWorld>();
                                        rawImage.GetComponent<RawImage>().texture = iw.inventoryImage;
                                        break;
                                    }
                                case "ObjectSelectable_Button":
                                    {
                                        AnimController ac = GameObject.Find("Shelf").GetComponent<AnimController>();
                                        ac.PlayMorseCode();
                                        morseCodeSound.PlayDelayed(0.7f);
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        //Detekcija lijevog klika miša
                        else if (Input.GetMouseButtonDown(0))
                        {
                            switch (hit.transform.tag)
                            {
                                //Manipulacija tipkama sefa.
                                case "ObjectSelectable_SafeButton":
                                    {
                                        if (safeIsOpened == false)
                                        {
                                            //Pokreni animaciju tipke sefa
                                            AnimController ac = GameObject.Find("PassMachine").GetComponent<AnimController>();
                                            ac.PressButton(hit.transform.name[hit.transform.name.Length - 1].ToString());


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
                                                bool validPass = PassMachine.CheckPassword(this.safePassword);
                                                //Ako je ispravna lozinka, upali zeleno svjetlo
                                                if (validPass)
                                                {
                                                    Light l = GameObject.Find("SuccessLight").GetComponent<Light>();
                                                    l.intensity = 3;

                                                    Debug.Log("Pass is valid: " + this.safePassword);
                                                    AnimController acd = GameObject.Find("DrawerCube").GetComponent<AnimController>();
                                                    acd.OpenCubeDrawerRoom2();
                                                    Light sl = GameObject.Find("DrawerCubeChildLight").GetComponent<Light>();
                                                    sl.intensity = 1.2f;
                                                    RemoteControl.tag = "ObjectSelectable_Inventory";

                                                }
                                                //Ako lozinka nije ispravna, upali crveno svjetlo
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


            #region Inventory korištenje
            if (Input.GetKeyDown("1"))
            {
                //GameObject mainCamera = GameObject.FindWithTag("MainCamera");
                //GameObject player = GameObject.Find("FPC");
                //FPController D = player.GetComponent<FPController>();
                //D.gameEnded = true;
                //D.enabled = false;

                //PlayerControls pc = mainCamera.GetComponent<PlayerControls>();
                //pc.enabled = false;

                //RemoteControlCanvas.SetActive(true);

                //Cursor.visible = true;
                //Cursor.lockState = CursorLockMode.None;

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
                RemoveRemoteCanvas();
            }
            #endregion
        }

        private void RemoveRemoteCanvas()
        {
            GameObject mainCamera = GameObject.FindWithTag("MainCamera");
            GameObject player = GameObject.Find("FPC");
            FPController D = player.GetComponent<FPController>();
            D.enabled = true;

            PlayerControls pc = mainCamera.GetComponent<PlayerControls>();
            pc.enabled = true;

            RemoteControlCanvas.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }

        private void CubeCylinderMatch()
        {

            if (cylinder_1.match && cylinder_2.match && cylinder_3.match && cylinder_4.match)
            {
                Debug.Log("FULL MATCH");
                GameObject rope = GameObject.Find("Rope");
                Scaler ropeScript = rope.GetComponent<Scaler>();
                ropeScript.DropRope();
                rope.tag = "ObjectSelectable_Inventory";

                ropeDroped = true;

                //shelfOpened = true;

                GameObject cr = GameObject.Find("Cube_Red");
                cr.tag = "Untagged";
                GameObject cb = GameObject.Find("Cube_Blue");
                cb.tag = "Untagged";
                GameObject cg = GameObject.Find("Cube_Green");
                cg.tag = "Untagged";
                GameObject co = GameObject.Find("Cube_Orange");
                co.tag = "Untagged";
            }

        }

        static string remotePassword = "385";
        static string currentRemotePassword = "";
        private bool CheckRemotePass(string insertedPassword)
        {
            bool correct = false;

            if (insertedPassword == remotePassword)
            {
                correct = true;
            }
            return correct;
        }
        public void PressButtonOpenEvent()
        {
            bool validPass = CheckRemotePass(currentRemotePassword);

            if (validPass)
            {
                Debug.Log("Ispravna lozinka");
                InvokeRepeating("PositionCylinder", 0.0f, 0.001f);
                RemoveRemoteCanvas();
                inventory.RemoveItem(this.inventory.GetItemList().Where(id => id.itemType == Item.ItemType.RemoteControl).FirstOrDefault());
            }
            else
            {
                StartCoroutine(objectManipulation.ShowWarningText("Password Is Incorrect!"));
            }

            currentRemotePassword = "";
            remoteText.text = currentRemotePassword;
        }
        public void PressButtonEvent(int number)
        {
            if (currentRemotePassword.Length < 3)
            {
                currentRemotePassword = currentRemotePassword + number.ToString();
                remoteText.text = currentRemotePassword;
            }
        }

        private void InventoryKeyManipulation(string number)
        {
            Item selectedItem = this.inventory.GetItemList().Where(id => id.actionKey == number).FirstOrDefault();

            if (selectedItem != null)
            {
                if (selectedItem.itemType == Item.ItemType.GoldenKey || selectedItem.itemType == Item.ItemType.SilverKey)
                {
                    if (_selection != null)
                    {
                        if (_selection.name == "Keyhole")
                        {
                            if (selectedItem.id == 11)
                            {
                                this.KeyHoleUnlocked = true;
                                Debug.Log("Keyhole is unlocked.");
                                inventory.RemoveItem(selectedItem);
                                objectManipulation.ShowFloatingText(_selection.name, _selection.tag);


                                AnimController ac = GameObject.Find("Shelf").GetComponent<AnimController>();
                                ac.OpenShelfAnimation();

                                MorseButton.tag = "ObjectSelectable_Button";

                                GameObject keyhole = GameObject.Find("Keyhole");
                                keyhole.tag = "Untagged";
                            }
                            else
                            {
                                StartCoroutine(objectManipulation.ShowWarningText("Wrong Key!"));
                            }
                        }
                        else if (_selection.name == "Door")
                        {
                            if (selectedItem.id == 22)
                            {
                                this.DoorUnLocked = true;
                                SelectableObject selectedObject = GameObject.Find("Door").GetComponent<SelectableObject>();
                                selectedObject.locked = false;
                                Debug.Log("Door is unlocked.");
                                inventory.RemoveItem(selectedItem);
                                StartCoroutine(objectManipulation.ShowWarningText("Get Out!"));
                                AnimController ac1 = GameObject.Find("WallWestNorth").GetComponent<AnimController>();
                                ac1.StartDoorAnimation();
                            }
                            else
                            {
                                StartCoroutine(objectManipulation.ShowWarningText("Wrong Key!"));
                            }
                        }
                    }
                }
                else if (selectedItem.itemType == Item.ItemType.RemoteControl)
                {

                    GameObject mainCamera = GameObject.FindWithTag("MainCamera");
                    GameObject player = GameObject.Find("FPC");
                    FPController D = player.GetComponent<FPController>();
                    D.enabled = false;

                    PlayerControls pc = mainCamera.GetComponent<PlayerControls>();
                    pc.enabled = false;

                    RemoteControlCanvas.SetActive(true);

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }

            }
        }
    }
}
