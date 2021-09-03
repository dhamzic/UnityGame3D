using Assets.Scripts.SelectableObjects;
using System;
using System.Collections;
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
        public AudioSource InventoryPickUp;
        public AudioSource UnlockSound;
        public AudioSource DrawerSound;
        public AudioSource DoorOpenSound;
        public AudioSource InventorySound;
        public AudioSource ButtonSound;
        public AudioSource SafeOpenSound;
        public AudioSource WoodImpactSound;
        public AudioSource BookshelfSound;
        public AudioSource SwitchSound;
        public AudioSource CubeRaiseSound;




        [SerializeField] private string selectableTag = "ObjectSelectable";

        [SerializeField] private Material highlightMaterial;
        //[SerializeField] private Material defaultMaterial;

        [SerializeField] private UiInventory uiInventory;

        private Inventory inventory;

        private bool DoorUnLocked = false;

        private bool CurvedDrawerUnLocked = false;
        private ObjectManipulationText objectManipulation;

        //Provjera je li trenutno miš preko objekta koji služi za interakciju
        bool isHovering = false;
        private Transform _selection;

        ////Provjera je li PREFAB uključen
        //bool objectInfoTurnedOn = false;

        private GameObject UiInventoryCanvas;
        private GameObject UiInventoryRead;

        public GameObject trenutniRoditelj;
        public Transform trenutniRoditeljManevriranje;


        #region Safe
        string safePassword = "";
        bool safeIsOpened = false;
        #endregion

        int inventoryItemId = 1;

        GameObject objektSlike = null;


        private GameObject istocniZid = null;

        bool switchTurnedOn = false;

        bool objectRaised = false;
        private GameObject raisedObject = null;

        bool cubeDrawerOpened = false;
        private void Awake()
        {
            inventory = new Inventory();
            uiInventory.SetInventory(inventory);

            UiInventoryCanvas = GameObject.Find("UiInventory");
            UiInventoryCanvas.SetActive(false);

            UiInventoryRead = GameObject.Find("UiInventoryRead");
            UiInventoryRead.SetActive(false);

            objektSlike = GameObject.Find("Painting");

            istocniZid = GameObject.Find("WallEast");
        }
        private void Start()
        {
            objectManipulation = GetComponent<ObjectManipulationText>();

            //ItemWorld.SpawnItemWorld(new Vector3(436.0165f, -0.1f, -445.9609f), new Item { itemType = Item.ItemType.Key, amount = 1 });

            //Dohvati roditelja. Služi za prijenos objekta. Razmak između igrača i objekta prilikom premještanja
            trenutniRoditelj = GameObject.Find("FPC_ObjectHolder");
            trenutniRoditeljManevriranje = trenutniRoditelj.GetComponent<Transform>();

            #region Cylinders
            GameObject c1 = GameObject.Find("Cylinder_1");
            cylinder_1 = c1.GetComponent<Cylinder>();
            GameObject c2 = GameObject.Find("Cylinder_2");
            cylinder_2 = c2.GetComponent<Cylinder>();
            GameObject c3 = GameObject.Find("Cylinder_3");
            cylinder_3 = c3.GetComponent<Cylinder>();
            GameObject c4 = GameObject.Find("Cylinder_4");
            cylinder_4 = c4.GetComponent<Cylinder>();
            GameObject c5 = GameObject.Find("Cylinder_5");
            cylinder_5 = c5.GetComponent<Cylinder>();
            GameObject c6 = GameObject.Find("Cylinder_6");
            cylinder_6 = c6.GetComponent<Cylinder>();
            #endregion


            //PlayerPrefs.SetString("Rezultat","Ovo je prvi rezultat");
            //PlayerPrefs.Save();


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
        Cylinder cylinder_5 = null;
        Cylinder cylinder_6 = null;
        #endregion

        bool bookShelfOpened = false;

        private void ThrowCube()
        {

        }

        private void Update()
        {
            //Provjerava je li došlo do pogotka između
            //Bačenih kocki i ležećih cilindra
            CubeCylinderMatch();

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
                InventorySound.Play();
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
                                        SwitchSound.Play();

                                        //Ukoliko je prekidač pritisnut tj. svjetlo upaljeno
                                        if (this.switchTurnedOn)
                                        {
                                            //Ugasi prekidač
                                            this.switchTurnedOn = false;
                                            var wallRenderer = this.istocniZid.GetComponent<Renderer>();
                                            WallWithHidenInfo wall = this.istocniZid.GetComponent<WallWithHidenInfo>();
                                            //Vrati zidu stari materijal (sakrij skrivenu poruku) i ugasi UV lampu
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
                                            //Paljenje UV lampe i pokazivanje skrivene poruke
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
                                                sl.intensity = 0.5f;
                                            }
                                        }
                                        break;
                                    }
                                case "ObjectSelectable_Drawer":
                                    {
                                        if (hit.transform.name.Contains("Cube"))
                                        {

                                            if (cubeDrawerOpened==false)
                                            {
                                                //Upali svjetlo u ladici da se bolje vidi papirić koji se tamo nalazi
                                                AnimController ac = GameObject.Find("DrawerCube").GetComponent<AnimController>();
                                                ac.StartDrawerAnimation(hit.transform.name);
                                                DrawerSound.Play();
                                                Light sl = GameObject.Find("DrawerCubeChildLight").GetComponent<Light>();
                                                sl.intensity = 1.2f;
                                                cubeDrawerOpened = true; 
                                            }
                                            break;
                                        }
                                        //Provjera je li ormarić zaključan
                                        if (CurvedDrawerUnLocked)
                                        {
                                            AnimController ac = GameObject.Find("CurvedDrawer").GetComponent<AnimController>();
                                            ac.StartDrawerAnimation(hit.transform.name);
                                            DrawerSound.Play();
                                        }
                                        else
                                        {
                                            Debug.Log("Drawer is locked.");
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
                                        InventoryPickUp.Play();
                                        //Nakon što je objekt prikupljen potrebno ga je uništiti iz scene
                                        Destroy(hit.transform.gameObject);
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
                                case "ObjectSelectable_Painting":
                                    {
                                        ////Podigni objekt
                                        //Debug.Log("Objekt podignut");
                                        //objektSlike.GetComponent<Rigidbody>().useGravity = false;
                                        //objektSlike.GetComponent<Rigidbody>().detectCollisions = true;
                                        //objektSlike.transform.parent = trenutniRoditelj.transform;
                                        //objektSlike.transform.position = trenutniRoditeljManevriranje.transform.position;

                                        //objektSlike.transform.localEulerAngles = new Vector3(0, 0, 0);

                                        //Baci sliku na pod
                                        objektSlike.GetComponent<Rigidbody>().useGravity = true;
                                        //objektSlike.transform.localEulerAngles = new Vector3(0, 0, 30);
                                        objektSlike.transform.parent = null;
                                        objektSlike.tag = "Untagged";
                                        WoodImpactSound.PlayDelayed(1);
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
                                            CubeRaiseSound.Play();
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
                                            ButtonSound.Play();
                                            AnimController ac = GameObject.Find("WallWithSafe").GetComponent<AnimController>();
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
                                                bool validPass = Safe.CheckPassword(this.safePassword);
                                                //Ako je ispravna lozinka, upali zeleno svjetlo
                                                if (validPass)
                                                {
                                                    Light l = GameObject.Find("SuccessLight").GetComponent<Light>();
                                                    l.intensity = 11;

                                                    Debug.Log("Pass is valid: " + this.safePassword);
                                                    SafeOpenSound.Play();
                                                    ac.OpenSafe();
                                                    safeIsOpened = true;
                                                    Light sl = GameObject.Find("SafeLight").GetComponent<Light>();
                                                    sl.intensity = 3;
                                                }
                                                //Ako lozinka nije ispravna, upali crveno svjetlo
                                                else
                                                {
                                                    ul.intensity = 11;

                                                    StartCoroutine(objectManipulation.ShowWarningText("Invalid Password!"));

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
                Cursor.visible = false;
                UiInventoryRead.SetActive(false);
            }
            #endregion
        }

        private void CubeCylinderMatch()
        {
            if (bookShelfOpened == false)
            {
                if (cylinder_1.match && cylinder_2.match && cylinder_3.match && cylinder_4.match && cylinder_5.match && cylinder_6.match)
                {
                    Debug.Log("FULL MATCH");
                    bookShelfOpened = true;


                    AnimController bsa = GameObject.Find("Bookshelf").GetComponent<AnimController>();
                    bsa.StartBookShelfAnimation();
                    BookshelfSound.PlayDelayed(0.5f);
                    GameObject d = GameObject.Find("Door");
                    d.tag = "ObjectSelectable_Door";
                    //TODO: Provjera ključa za izlaz

                    //StartCoroutine("WaitBookShelfOpen");
                }
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
                        if (_selection.name == "CurvedDrawer" || _selection.name == "Drawer1" || _selection.name == "Drawer2" || _selection.name == "Drawer3" || _selection.name == "Drawer4")
                        {
                            if (selectedItem.id == 11)
                            {
                                UnlockSound.Play();
                                //_selection.GetComponent<BoxCollider>().enabled = false;
                                this.CurvedDrawerUnLocked = true;
                                SelectableObject selectedObject = GameObject.Find("CurvedDrawer").GetComponent<SelectableObject>();
                                selectedObject.locked = false;
                                Debug.Log("Curved drawer is unlocked.");
                                inventory.RemoveItem(selectedItem);
                                objectManipulation.ShowFloatingText(_selection.name, _selection.tag);
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
                                UnlockSound.Play();
                                this.DoorUnLocked = true;
                                SelectableObject selectedObject = GameObject.Find("Door").GetComponent<SelectableObject>();
                                selectedObject.locked = false;
                                Debug.Log("Door is unlocked.");
                                inventory.RemoveItem(selectedItem);
                                StartCoroutine(objectManipulation.ShowWarningText("Get Out!"));
                                AnimController ac1 = GameObject.Find("WallNorth").GetComponent<AnimController>();
                                ac1.StartDoorAnimation();
                                DoorOpenSound.Play();
                            }
                            else
                            {
                                StartCoroutine(objectManipulation.ShowWarningText("Wrong Key!"));
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
