using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOptionsDevicesAdaption : MonoBehaviour
{

// Variáveis que representam os campos selecionáveis
    GameObject PitacoToggle;
    GameObject ManoToggle;
    GameObject CintaToggle;
    GameObject OxiToggle;

// Status de seleção dos campos de dispositivos: true / false
    public bool statusPitacoToggle;
    public bool statusManoToggle;
    public bool statusCintaToggle;
    public bool statusOxiToggle;

// Grupos de Menus ativados de acordo com a combinação selecionada de dispositivos.
    GameObject optionGroupP;
    GameObject optionGroupM;
    GameObject optionGroupC;
    GameObject optionGroupPC;
    GameObject optionGroupMC;
    GameObject optionGroupPO;
    GameObject optionGroupMO;
    GameObject optionGroupCO;
    GameObject optionGroupPCO;
    GameObject optionGroupMCO;
    GameObject optionGroupInc;
    


    // Start is called before the first frame update
    void Start()
    {
        PitacoToggle = GameObject.Find("TogglePitaco");
        ManoToggle = GameObject.Find("ToggleMano");
        CintaToggle = GameObject.Find("ToggleCinta");
        OxiToggle = GameObject.Find("ToggleOxi");

        optionGroupP = GameObject.Find("Options Group P");
        optionGroupM = GameObject.Find("Options Group M");
        optionGroupC = GameObject.Find("Options Group C");
        optionGroupPC = GameObject.Find("Options Group P+C");
        optionGroupMC = GameObject.Find("Options Group M+C");
        optionGroupPO = GameObject.Find("Options Group P+O");
        optionGroupMO = GameObject.Find("Options Group M+O");
        optionGroupCO = GameObject.Find("Options Group C+O");
        optionGroupPCO = GameObject.Find("Options Group P+C+O");
        optionGroupMCO = GameObject.Find("Options Group M+C+O");
        optionGroupInc = GameObject.Find("Options Group Incorrect");

        optionGroupP.SetActive(false);
        optionGroupM.SetActive(false);
        optionGroupC.SetActive(false);
        optionGroupPC.SetActive(false);
        optionGroupMC.SetActive(false);
        optionGroupPO.SetActive(false);
        optionGroupMO.SetActive(false);
        optionGroupCO.SetActive(false);
        optionGroupPCO.SetActive(false);
        optionGroupMCO.SetActive(false);
        optionGroupInc.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        statusPitacoToggle = PitacoToggle.GetComponent<Toggle>().isOn;
        statusManoToggle = ManoToggle.GetComponent<Toggle>().isOn;
        statusCintaToggle = CintaToggle.GetComponent<Toggle>().isOn;
        statusOxiToggle = OxiToggle.GetComponent<Toggle>().isOn;

        
        // Nenhum dispositivo selecionado
        if (statusPitacoToggle == false && statusManoToggle == false && statusCintaToggle == false && statusOxiToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // P
        if (statusPitacoToggle == true && statusManoToggle == false && statusCintaToggle == false && statusOxiToggle == false)
        {
            optionGroupP.SetActive(true);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // M
        if (statusManoToggle == true && statusPitacoToggle == false && statusCintaToggle == false && statusOxiToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(true);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // C
        if (statusCintaToggle == true && statusManoToggle == false && statusPitacoToggle == false && statusOxiToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(true);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // P+C
        if (statusPitacoToggle == true && statusCintaToggle == true && statusManoToggle == false && statusOxiToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(true);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // M+C
        if (statusManoToggle == true && statusCintaToggle == true && statusPitacoToggle == false && statusOxiToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(true);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // P+O
        if (statusPitacoToggle == true && statusOxiToggle == true && statusManoToggle == false && statusCintaToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(true);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // M+O
        if (statusManoToggle == true && statusOxiToggle == true && statusPitacoToggle == false && statusCintaToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(true);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // C+O
        if (statusCintaToggle == true && statusOxiToggle == true && statusPitacoToggle == false && statusManoToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(true);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // P+C+O
        if (statusPitacoToggle == true && statusCintaToggle == true && statusOxiToggle == true && statusManoToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(true);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(false);
        }

        // M+C+O
        if (statusManoToggle == true && statusCintaToggle == true && statusOxiToggle == true && statusPitacoToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(true);
            optionGroupInc.SetActive(false);
        }

        // COMBINAÇÕES INCORRETAS
        // O
        if (statusOxiToggle == true && statusPitacoToggle == false && statusManoToggle == false && statusCintaToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(true);
        }

        // P+M
        if (statusPitacoToggle == true && statusManoToggle == true && statusCintaToggle == false && statusOxiToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(true);
        }

        // P+M+C
        if (statusPitacoToggle == true && statusManoToggle == true && statusCintaToggle == true && statusOxiToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(true);
        }

        // P+M+O
        if (statusPitacoToggle == true && statusManoToggle == true && statusOxiToggle == true && statusCintaToggle == false)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(true);
        }

        // P+M+C+O
        if (statusPitacoToggle == true && statusManoToggle == true && statusCintaToggle == true && statusOxiToggle == true)
        {
            optionGroupP.SetActive(false);
            optionGroupM.SetActive(false);
            optionGroupC.SetActive(false);
            optionGroupPC.SetActive(false);
            optionGroupMC.SetActive(false);
            optionGroupPO.SetActive(false);
            optionGroupMO.SetActive(false);
            optionGroupCO.SetActive(false);
            optionGroupPCO.SetActive(false);
            optionGroupMCO.SetActive(false);
            optionGroupInc.SetActive(true);
        }


    }
}
