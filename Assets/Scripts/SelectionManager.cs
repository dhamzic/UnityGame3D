using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    //Provjera je li trenutno miš preko objekta koji služi za interakciju
    bool isHovering = false;

    //Provjera je li PREFAB uključen
    bool objectInfoTurnedOn = false;

    public AnimController animationScript;
    private DrawerController drawerScript;

    private Transform _selection;


    // Start is called before the first frame update
    void Start()
    {
        drawerScript = GetComponent<DrawerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHovering == false)
        {
            drawerScript.DeleteFloatingText();
            objectInfoTurnedOn = false;
        }

        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }

        var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance <= 4)
            {
                var selection = hit.transform;
                if (selection.CompareTag(selectableTag))
                {
                    isHovering = true;
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        selectionRenderer.material = highlightMaterial;
                    }
                    _selection = selection;


                    if (objectInfoTurnedOn == false)
                    {
                        drawerScript.ShowFloatingText();
                    }
                    objectInfoTurnedOn = true;


                    if (Input.GetKeyDown("e"))
                    {
                        animationScript.StartAnimation(hit.transform.name);
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
    }
}
