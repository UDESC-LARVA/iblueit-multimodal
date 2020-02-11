using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Data;

public class PacientName : MonoBehaviour
{
     Text myText;
    
    // void Start()
    // {
    //     myText = GameObject.Find("Placeholder").GetComponent<Text>();
    //     myText.text = Pacient.Loaded.Name;
    // }
    
    void Update()
    {
        myText = GameObject.Find("Placeholder").GetComponent<Text>();
        myText.text = Pacient.Loaded.Name;
    }
}
