using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SelectableObjects
{
    public class ObjectManipulationText : MonoBehaviour
    {
        public Text currentManipulationText;
        public Text warningText;
        void Start()
        {
        }
        
        public void ShowFloatingText(string objectName, string objectTag)
        {
            string text = "";
            switch (objectTag)
            {
                case "ObjectSelectable_Drawer":
                    {
                        text = "Press E To Open/Close Drawer";
                        if (objectName == "Drawer1" || objectName == "Drawer2" || objectName == "Drawer3" || objectName == "Drawer4")
                        {
                            GameObject drawer = GameObject.Find(objectName);


                            GameObject curvedDrawer = GameObject.Find("CurvedDrawer");
                            SelectableObject selectedObject = curvedDrawer.GetComponent<SelectableObject>();
                            if (selectedObject.locked == true)
                            {
                                text = "Drawer Is Locked. Press I To Check Your Inventory";
                            }
                        }
                        break;
                    }
                case "ObjectSelectable_Painting":
                    {
                        text = "Press E To Throw Painting";
                        break;
                    }
                case "ObjectSelectable_Switch":
                    {
                        text = "Press E To Turn On/Off Switch";
                        break;
                    }
                case "ObjectSelectable_Inventory":
                    {
                        text = "Press E To Pick Up";
                        if (objectName == "Book")
                        {
                            text = "Press E To Read";
                        }
                        break;
                    }
                default:
                    break;
            }
            currentManipulationText.text = text;
        }
        public IEnumerator ShowWarningText(string text)
        {
            warningText.text = text;
            yield return new WaitForSeconds(1f);
            StartCoroutine(FadeTextToZeroAlpha());
            yield return new WaitForSeconds(1f);
            this.warningText.text = "";
            this.warningText.color = new Color(this.warningText.color.r, this.warningText.color.g, this.warningText.color.b, 1);

            //Debug.Log("Tekst je izbrisan");
            //StartCoroutine(DeleteWarningText(2.0f));
        }
        public IEnumerator FadeTextToZeroAlpha()
        {
            //Alpha==1
            this.warningText.color = new Color(this.warningText.color.r, this.warningText.color.g, this.warningText.color.b, 1);

            //Smanjuje do 0
            while (this.warningText.color.a > 0.0f)
            {
                this.warningText.color = new Color(this.warningText.color.r, this.warningText.color.g, this.warningText.color.b, this.warningText.color.a - (Time.deltaTime / 1));
                yield return null;
            }
        }
    }
}
