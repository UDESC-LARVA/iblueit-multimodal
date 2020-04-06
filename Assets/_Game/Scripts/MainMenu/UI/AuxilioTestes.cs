/*
    Autor: Jhonatan Thallisson Cabral Néry
    Script feito para facilitar os testes, ele inativa as telas modificadas e deixa apenas a tela inicial ativa
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuxilioTestes : MonoBehaviour
{

// Variáveis que representam as telas
    
    GameObject StartPanel;
    GameObject SettingsMenu;
    GameObject CreditsPanel;
    GameObject LoadMenu;
    //GameObject Parameters;
    // GameObject TratSignal;
    // GameObject Fusion;
    // GameObject Adaptation;
    GameObject NewMenu;
    GameObject PlayerMenu;
    GameObject CalibDevices;
    GameObject CalibrationMenuPitaco;
    GameObject CalibrationMenuMano;
    GameObject CalibrationMenuCinta;
    GameObject PlataformMenu;
    GameObject MinigamesMenu;


    
// Start é chamado no início do primeiro frame
    void Start()
    {
        StartPanel = GameObject.Find("Canvas").transform.Find("Start Panel").gameObject;
        SettingsMenu = GameObject.Find("Canvas").transform.Find("Settings Menu").gameObject;
        CreditsPanel = GameObject.Find("Canvas").transform.Find("Credits Panel").gameObject;
        LoadMenu = GameObject.Find("Canvas").transform.Find("Load Menu").gameObject;
        //Parameters = GameObject.Find("Canvas").transform.Find("Parameters Menu").gameObject;
        // TratSignal = GameObject.Find("Canvas").transform.Find("Signal Treatment Menu").gameObject;
        // Fusion = GameObject.Find("Canvas").transform.Find("Fusion Menu").gameObject;
        // Adaptation = GameObject.Find("Canvas").transform.Find("Adaptation Menu").gameObject;
        NewMenu = GameObject.Find("Canvas").transform.Find("New Menu").gameObject;
        PlayerMenu = GameObject.Find("Canvas").transform.Find("Player Menu").gameObject;
        CalibDevices = GameObject.Find("Canvas").transform.Find("CalibDevices").gameObject;
        CalibrationMenuPitaco = GameObject.Find("Canvas").transform.Find("Calibration Menu Pitaco").gameObject;
        CalibrationMenuMano = GameObject.Find("Canvas").transform.Find("Calibration Menu Mano").gameObject;
        CalibrationMenuCinta = GameObject.Find("Canvas").transform.Find("Calibration Menu Cinta").gameObject;
        PlataformMenu = GameObject.Find("Canvas").transform.Find("Plataform Menu").gameObject;
        MinigamesMenu = GameObject.Find("Canvas").transform.Find("Minigames Menu").gameObject;

        StartPanel.SetActive(true);
        SettingsMenu.SetActive(false);
        CreditsPanel.SetActive(false);
        LoadMenu.SetActive(false);
        //Parameters.SetActive(false);
        // TratSignal.SetActive(false);
        // Fusion.SetActive(false);
        // Adaptation.SetActive(false);
        NewMenu.SetActive(false);
        PlayerMenu.SetActive(false);
        CalibDevices.SetActive(false);
        CalibrationMenuPitaco.SetActive(false);
        CalibrationMenuMano.SetActive(false);
        CalibrationMenuCinta.SetActive(false);
        PlataformMenu.SetActive(false);
        MinigamesMenu.SetActive(false);


    }


}
