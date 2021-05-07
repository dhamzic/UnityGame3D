using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    bool AnimatorIsPlaying(string animationName)
    {
        if (
            this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1
            &&
            this.anim.GetCurrentAnimatorStateInfo(0).IsName(animationName)
            )
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
        Debug.Log(objectName+" Z position: "+ GameObject.Find(objectName).transform.localPosition.z);
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
        if (Input.GetKeyDown("1"))
        {
            if (
                !AnimatorIsPlaying("Drawer4_Open")
                &&
                !AnimatorIsPlaying("Drawer2_Open")
                &&
                !AnimatorIsPlaying("Drawer3_Open")
                &&
                StartingPosition("Drawer1")
                )
            {
                anim.Play("Drawer1_Open");
            }

        }
        if (Input.GetKeyDown("2"))
        {
            if (
                !AnimatorIsPlaying("Drawer1_Open")
                &&
                !AnimatorIsPlaying("Drawer4_Open")
                &&
                !AnimatorIsPlaying("Drawer3_Open")
                &&
                StartingPosition("Drawer2")
                )
            {
                anim.Play("Drawer2_Open");
            }

        }
        if (Input.GetKeyDown("3"))
        {
            if (
                !AnimatorIsPlaying("Drawer1_Open")
                &&
                !AnimatorIsPlaying("Drawer2_Open")
                &&
                !AnimatorIsPlaying("Drawer4_Open")
                &&
                StartingPosition("Drawer3")
                )
            {
                anim.Play("Drawer3_Open");
            }

        }
        if (Input.GetKeyDown("4"))
        {
            if (
                !AnimatorIsPlaying("Drawer1_Open")
                &&
                !AnimatorIsPlaying("Drawer2_Open")
                &&
                !AnimatorIsPlaying("Drawer3_Open")
                &&
                StartingPosition("Drawer4")
                )
            {
                anim.Play("Drawer4_Open");
            }

        }
    }
}
