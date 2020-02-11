using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOptionsDevicesSignal : MonoBehaviour
{

    GameObject PitacoToggle;
    GameObject ManoToggle;
    GameObject CintaToggle;

    public bool statusPitacoToggle;
    public bool statusManoToggle;
    public bool statusCintaToggle;

    GameObject optionPGroup;
    GameObject optionMGroup;
    GameObject optionCGroup;


    // Start is called before the first frame update
    void Start()
    {
        PitacoToggle = GameObject.Find("TogglePitaco");
        ManoToggle = GameObject.Find("ToggleMano");
        CintaToggle = GameObject.Find("ToggleCinta");

        optionPGroup = GameObject.Find("Options Pitaco Group");
        optionMGroup = GameObject.Find("Options Mano Group");
        optionCGroup = GameObject.Find("Options Cinta Group");

        optionPGroup.SetActive(false);
        optionMGroup.SetActive(false);
        optionCGroup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        statusPitacoToggle = PitacoToggle.GetComponent<Toggle>().isOn;
        statusManoToggle = ManoToggle.GetComponent<Toggle>().isOn;
        statusCintaToggle = CintaToggle.GetComponent<Toggle>().isOn;

        // print(statusPitacoToggle);
        // print(statusManoToggle);
        // print(statusCintaToggle);
        
        if (statusPitacoToggle == true)
        {
            optionPGroup.SetActive(true);
            optionMGroup.SetActive(false);
            optionCGroup.SetActive(false);
        }

        if (statusManoToggle == true)
        {
            optionPGroup.SetActive(false);
            optionMGroup.SetActive(true);
            optionCGroup.SetActive(false);
        }

        if (statusCintaToggle == true)
        {
            optionPGroup.SetActive(false);
            optionMGroup.SetActive(false);
            optionCGroup.SetActive(true);
        }

    }
}
