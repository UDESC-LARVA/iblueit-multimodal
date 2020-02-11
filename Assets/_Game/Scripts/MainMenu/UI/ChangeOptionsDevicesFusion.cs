using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOptionsDevicesFusion : MonoBehaviour
{

// Variáveis que representam os campos selecionáveis
    GameObject PredominanciaToggle;
    GameObject SomaToggle;
    GameObject SubtracaoToggle;
    GameObject ComplementoToggle;

// Status de seleção dos tipos de fusão: true / false
    public bool statusPredominanciaToggle;
    public bool statusSomaToggle;
    public bool statusSubtracaoToggle;
    public bool statusComplementoToggle;

// Grupos de Menus ativados de acordo com a opção de fusão selecionada.
    GameObject optionGroupPredom;
    GameObject optionGroupSoma;
    GameObject optionGroupSub;
    GameObject optionGroupCompl;
    


    // Start is called before the first frame update
    void Start()
    {
        PredominanciaToggle = GameObject.Find("TogglePredominancia");
        SomaToggle = GameObject.Find("ToggleSoma");
        SubtracaoToggle = GameObject.Find("ToggleSubtracao");
        ComplementoToggle = GameObject.Find("ToggleComplemento");

        optionGroupPredom = GameObject.Find("Options Group Predominancia");
        optionGroupSoma = GameObject.Find("Options Group Soma");
        optionGroupSub = GameObject.Find("Options Group Subtracao");
        optionGroupCompl = GameObject.Find("Options Group Complemento");

        optionGroupPredom.SetActive(false);
        optionGroupSoma.SetActive(false);
        optionGroupSub.SetActive(false);
        optionGroupCompl.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        statusPredominanciaToggle = PredominanciaToggle.GetComponent<Toggle>().isOn;
        statusSomaToggle = SomaToggle.GetComponent<Toggle>().isOn;
        statusSubtracaoToggle = SubtracaoToggle.GetComponent<Toggle>().isOn;
        statusComplementoToggle = ComplementoToggle.GetComponent<Toggle>().isOn;

        if (statusPredominanciaToggle == true)
        {
            optionGroupPredom.SetActive(true);
            optionGroupSoma.SetActive(false);
            optionGroupSub.SetActive(false);
            optionGroupCompl.SetActive(false);
        }

        if (statusSomaToggle == true)
        {
            optionGroupPredom.SetActive(false);
            optionGroupSoma.SetActive(true);
            optionGroupSub.SetActive(false);
            optionGroupCompl.SetActive(false);
        }
        
        if (statusSubtracaoToggle == true)
        {
            optionGroupPredom.SetActive(false);
            optionGroupSoma.SetActive(false);
            optionGroupSub.SetActive(true);
            optionGroupCompl.SetActive(false);
        }

        if (statusComplementoToggle == true)
        {
            optionGroupPredom.SetActive(false);
            optionGroupSoma.SetActive(false);
            optionGroupSub.SetActive(false);
            optionGroupCompl.SetActive(true);
        }
       


    }
}
