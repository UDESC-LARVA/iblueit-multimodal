/*
    Autor: Jhonatan Thallisson Cabral Néry
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressedOrNot : MonoBehaviour
{

// Variável que representa o conteúdo do tratamento de sinais
    GameObject TratSignal;

// Variável que representa o Botão de tratamento de sinais
    GameObject TratSignalButton;

// Componente de imagem do botão
    private Image imgTratSignal;

    public bool pressed;


// Imagens de fundo que aparecem no botão
    public Sprite sprite3; // Drag your second sprite here
    public Sprite sprite4; // Drag your second sprite here
    
// Start é chamado no início do frame inicial
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(TaskOnClick); // Ouvindo o botão

        TratSignal = GameObject.Find("Canvas").transform.Find("Signal Treatment Menu").gameObject;

        TratSignalButton = GameObject.Find("Signal Treatment Button");

        pressed = false;

        imgTratSignal = TratSignalButton.GetComponent<Image>();

    }

    void Update()
    {
        if (!TratSignal.activeSelf)
        {
            imgTratSignal.sprite = sprite4;
            pressed = false;
        }
    }

    void TaskOnClick(){  // Ações quando o botão for clicado
		pressed = !pressed;

        if (TratSignal.activeSelf && pressed == true)
        {
            imgTratSignal.sprite = sprite3;
        }

        if (TratSignal.activeSelf && pressed == false)
        {
            imgTratSignal.sprite = sprite4;
            TratSignal.SetActive(false);
        }

	}

}
