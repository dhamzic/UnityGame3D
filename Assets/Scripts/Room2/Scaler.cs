using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    private float mSize = 0.0f;

    public void DropRope()
    {
        //Svakih 0.04s pomakni za 1 blend shape 
        InvokeRepeating("Scale", 0.0f, 0.04f);
    }

    void Scale()
    {
        if (mSize >= 100.0f)
        {
            CancelInvoke("Scale");
        }
        GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, mSize++);
    }
}
