using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Serial;
using Ibit.Core.Data;

public class CalibrationReturn : MonoBehaviour
{

// Variáveis que representam as telas
    
    GameObject StartPanel;
    GameObject SettingsMenu;
    GameObject CreditsPanel;
    GameObject LoadMenu;
    GameObject NewMenu;
    GameObject PlayerMenu;
    GameObject CalibDevices;
    GameObject CalibrationMenuPitaco;
    GameObject CalibrationMenuMano;
    GameObject CalibrationMenuCinta;
    GameObject PlataformMenu;
    GameObject MinigamesMenu;
    // GameObject CalibrationReturning;

    [SerializeField]
    private SerialControllerPitaco serialControllerPitaco;
    [SerializeField]
    private SerialControllerMano serialControllerMano;
    [SerializeField]
    private SerialControllerCinta serialControllerCinta;

    private bool PitacoConnected = false;
    private bool ManoConnected = false;
    private bool CintaConnected = false;

    // Start é chamado no início do primeiro frame
    private void OnEnable()
    {

        if (serialControllerPitaco == null)
            serialControllerPitaco = FindObjectOfType<SerialControllerPitaco>();

        if (serialControllerMano == null)
            serialControllerMano = FindObjectOfType<SerialControllerMano>();

        if (serialControllerCinta == null)
            serialControllerCinta = FindObjectOfType<SerialControllerCinta>();


        if (serialControllerPitaco.IsConnected)
            PitacoConnected = true;

        if (serialControllerMano.IsConnected)
            ManoConnected = true;

        if (serialControllerCinta.IsConnected)
            CintaConnected = true;


        if(Pacient.Loaded != null)
        {
            if((PitacoConnected && !Pacient.Loaded.CalibrationPitacoDone)||(ManoConnected && !Pacient.Loaded.CalibrationManoDone)||(CintaConnected && !Pacient.Loaded.CalibrationCintaDone))
                StartCoroutine(ExampleCoroutine());
        }
    }


    IEnumerator ExampleCoroutine()
    {

        //yield on a new YieldInstruction that waits for 3 seconds.
        yield return new WaitForSeconds(3);

        StartPanel = GameObject.Find("Canvas").transform.Find("Start Panel").gameObject;
        SettingsMenu = GameObject.Find("Canvas").transform.Find("Settings Menu").gameObject;
        CreditsPanel = GameObject.Find("Canvas").transform.Find("Credits Panel").gameObject;
        LoadMenu = GameObject.Find("Canvas").transform.Find("Load Menu").gameObject;
        NewMenu = GameObject.Find("Canvas").transform.Find("New Menu").gameObject;
        PlayerMenu = GameObject.Find("Canvas").transform.Find("Player Menu").gameObject;
        CalibDevices = GameObject.Find("Canvas").transform.Find("CalibDevices").gameObject;
        CalibrationMenuPitaco = GameObject.Find("Canvas").transform.Find("Calibration Menu Pitaco").gameObject;
        CalibrationMenuMano = GameObject.Find("Canvas").transform.Find("Calibration Menu Mano").gameObject;
        CalibrationMenuCinta = GameObject.Find("Canvas").transform.Find("Calibration Menu Cinta").gameObject;
        PlataformMenu = GameObject.Find("Canvas").transform.Find("Plataform Menu").gameObject;
        MinigamesMenu = GameObject.Find("Canvas").transform.Find("Minigames Menu").gameObject;
        // CalibrationReturning = GameObject.Find("Canvas").transform.Find("Calibration Returning").gameObject;

        StartPanel.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsPanel.SetActive(false);
        LoadMenu.SetActive(false);
        NewMenu.SetActive(false);
        PlayerMenu.SetActive(false);
        CalibDevices.SetActive(true);
        CalibrationMenuPitaco.SetActive(false);
        CalibrationMenuMano.SetActive(false);
        CalibrationMenuCinta.SetActive(false);
        PlataformMenu.SetActive(false);
        MinigamesMenu.SetActive(false);
        // CalibrationReturning.SetActive(false);
    }

}
