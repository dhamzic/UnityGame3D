using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    public GameObject cylinder;
    public char ExpectedCube;
    public bool match = false;
    public AudioSource ImpactSound;
    bool soundPlayed = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ObjectSelectable_Cube")
        {
            if (soundPlayed == false) {
                ImpactSound.Play();
                soundPlayed = true;
            }
            Light sl = cylinder.transform.Find("Point Light").GetComponent<Light>();
            sl.intensity = 4;
            if (ExpectedCube == other.name[5])
            {
                this.match = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        //Debug.Log("OnTriggerExit: " + other.name);
        if (other.tag == "ObjectSelectable_Cube")
        {
            Light sl = cylinder.transform.Find("Point Light").GetComponent<Light>();
            sl.intensity = 0;
            this.match = false;
            soundPlayed = false;
        }
    }
}
