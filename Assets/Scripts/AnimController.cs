using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private Animator anim;

    private Dictionary<string, bool> IsDrawerOpen = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        FillDrawerState();
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            Debug.Log("Anim info: " + anim.name);

        }
        //anim = GameObject.Find("aname").GetComponent<Animator>();
    }

    void FillDrawerState()
    {
        IsDrawerOpen.Add("Drawer1", false);
        IsDrawerOpen.Add("Drawer2", false);
        IsDrawerOpen.Add("Drawer3", false);
        IsDrawerOpen.Add("Drawer4", false);
    }

    bool AnimatorIsPlaying(string animationName)
    {
        if (this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool StartingPosition(string objectName)
    {
        Debug.Log(objectName + " Z position: " + GameObject.Find(objectName).transform.localPosition.z);
        if (GameObject.Find(objectName).transform.localPosition.z >= 1.25)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void CloseAnim(string drawerName)
    {
        anim.Play("Drawer4_Close");
    }

    public void StartBookShelfAnimation()
    {
        anim.Play("BookShelfOpen");
        Debug.Log("BookShelf Animation");
    }

    public void StartDoorAnimation()
    {
        //anim.Play("DoorHandleAction");
        anim.Play("Door_Open");
        Debug.Log("DoorHandleOpen");
    }
    public void OpenShelfAnimation()
    {
        anim.Play("OpenShelf");
        Debug.Log("OpenShelf");
    }
    public void PlayMorseCode()
    {
        anim.Play("PlayButton");
        Debug.Log("PlayMorseCode");
    }
    public void OpenSafe()
    {
        anim.Play("Safe_HandleOpen");
        Debug.Log("Safe is opened." + anim.name);
    }
    public void PressButton(string buttonNumber)
    {
        anim.Play("PressButton_" + buttonNumber);
    }
    public void OpenCubeDrawerRoom2()
    {
        anim.Play("DrawerCube_Open_1");
    }
    public void StartDrawerAnimation(string drawerName)
    {
        if (drawerName == "Drawer1")
        {
            if (
                !AnimatorIsPlaying("Drawer4_Open")
                &&
                !AnimatorIsPlaying("Drawer2_Open")
                &&
                !AnimatorIsPlaying("Drawer3_Open")
                )
            {
                if (IsDrawerOpen[drawerName] == false)
                {
                    anim.Play("Drawer1_Open");
                    IsDrawerOpen[drawerName] = true;
                }
                else
                {
                    if (!AnimatorIsPlaying("Drawer1__Open"))
                    {
                        anim.Play("Drawer1_Close");
                        IsDrawerOpen[drawerName] = false;
                    }
                }
            }

        }
        else if (drawerName == "Drawer2")
        {
            if (
                !AnimatorIsPlaying("Drawer1_Open")
                &&
                !AnimatorIsPlaying("Drawer4_Open")
                &&
                !AnimatorIsPlaying("Drawer3_Open")
                )
            {
                if (IsDrawerOpen[drawerName] == false)
                {
                    anim.Play("Drawer2_Open");
                    IsDrawerOpen[drawerName] = true;
                }
                else
                {
                    anim.Play("Drawer2_Close");
                    IsDrawerOpen[drawerName] = false;
                }
            }

        }
        else if (drawerName == "Drawer3")
        {
            if (
                !AnimatorIsPlaying("Drawer1_Open")
                &&
                !AnimatorIsPlaying("Drawer2_Open")
                &&
                !AnimatorIsPlaying("Drawer4_Open")
                )
            {
                if (IsDrawerOpen[drawerName] == false)
                {
                    anim.Play("Drawer3_Open");
                    IsDrawerOpen[drawerName] = true;
                }
                else
                {
                    anim.Play("Drawer3_Close");
                    IsDrawerOpen[drawerName] = false;
                }
            }

        }
        else if (drawerName == "Drawer4")
        {
            if (
                !AnimatorIsPlaying("Drawer1_Open")
                &&
                !AnimatorIsPlaying("Drawer2_Open")
                &&
                !AnimatorIsPlaying("Drawer3_Open")
                )
            {
                if (IsDrawerOpen[drawerName] == false)
                {
                    anim.Play("Drawer4_Open");
                    IsDrawerOpen[drawerName] = true;
                }
                else
                {
                    anim.Play("Drawer4_Close");
                    IsDrawerOpen[drawerName] = false;
                }
            }

        }
        else if (drawerName == "DrawerCubeChild")
        {
            anim.Play("DrawerCube_Open");
        }

    }
    public void StartSwitchAnimation(bool turnedOn)
    {
        if (turnedOn)
        {
            anim.Play("TurnOff");
        }
        else
        {
            anim.Play("TurnOn");

        }
    }

}
