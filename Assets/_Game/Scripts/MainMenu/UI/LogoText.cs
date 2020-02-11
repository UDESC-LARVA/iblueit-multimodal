using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LogoText : MonoBehaviour
{

    public GameObject logoText;
    // Start is called before the first frame update
    public void Start()
    {
        logoText.SetActive(false);
        //print("False");
    }

    public void OnPointerOver()
    {
        logoText.SetActive(true);
        print("OnMouseOver");
    }

    public void OnPointerExit()
    {
        logoText.SetActive(true);
        print("OnMouseExit");
    }

}
