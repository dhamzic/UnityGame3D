using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    public GameObject DrawerObject;

    public GameObject OpenCloseTextParent;
    public bool PrefabInstantiated = false;

    GameObject instantiatedPrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        DrawerObject = GameObject.Find("CurvedDrawer");
    }

    // Update is called once per frame
    void Update()
    {
    }
    public Vector3 GetDrawerPosition()
    {
        return new Vector3(DrawerObject.transform.localPosition.x, DrawerObject.transform.localPosition.y + 1.319f, DrawerObject.transform.localPosition.z);
    }
    public Vector3 GetDrawerRotation()
    {
        return DrawerObject.transform.eulerAngles;
    }

    public void ShowFloatingText()
    {
        if (PrefabInstantiated == false)
        {
            instantiatedPrefab = Instantiate(OpenCloseTextParent, GetDrawerPosition(), Quaternion.identity);
            instantiatedPrefab.transform.eulerAngles = new Vector3(90, GetDrawerRotation().y, GetDrawerRotation().z);
            PrefabInstantiated = true;
            Debug.Log("INSTANTIATE");
        }

        if (PrefabInstantiated == true)
        {
            if (OpenCloseTextParent.activeInHierarchy == false)
            {
                instantiatedPrefab.transform.position = GetDrawerPosition();
                instantiatedPrefab.transform.eulerAngles = new Vector3(90, GetDrawerRotation().y, GetDrawerRotation().z);
                instantiatedPrefab.SetActive(true);
            }
        }
    }
    public void DeleteFloatingText()
    {
        if (instantiatedPrefab != null)
        {
            instantiatedPrefab.SetActive(false);
        }
    }

}
