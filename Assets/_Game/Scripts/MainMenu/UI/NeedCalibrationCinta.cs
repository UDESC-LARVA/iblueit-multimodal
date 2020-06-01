using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Serial;
using Ibit.Core.Data;


public class NeedCalibrationCinta : MonoBehaviour
{

// Variáveis que representam as telas
    GameObject PlayerMenu;
    GameObject CalibCintatoPlaY;    

    [SerializeField]
    private SerialControllerCinta serialControllerCinta;

    private Button btn;

    private void OnEnable()
    {
        if (serialControllerCinta == null)
            serialControllerCinta = FindObjectOfType<SerialControllerCinta>();

        PlayerMenu = GameObject.Find("Canvas").transform.Find("Player Menu").gameObject;
        CalibCintatoPlaY = GameObject.Find("Canvas").transform.Find("CalibCintatoPlaY").gameObject;

        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }


    private void TaskOnClick()
    {
        if (serialControllerCinta.IsConnected)
        {   
            PlayerMenu.SetActive(false);
            CalibCintatoPlaY.SetActive(true);
        }
    }
}