using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    public GameObject OpenCloseTextParent;
    public bool PrefabInstantiated = false;

    GameObject instantiatedPrefab = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowFloatingText()
    {
        if (PrefabInstantiated == false)
        {
            instantiatedPrefab = Instantiate(OpenCloseTextParent);
            PrefabInstantiated = true;
            Debug.Log("INSTANTIATE");
        }

        if (PrefabInstantiated == true)
        {
            if (OpenCloseTextParent.activeInHierarchy == false)
            {
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
