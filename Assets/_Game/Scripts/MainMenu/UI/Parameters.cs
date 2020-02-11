/*
    Autor: Jhonatan Thallisson Cabral Néry
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parameters : MonoBehaviour
{

// Variáveis que representam o conteúdo dos parâmetros
    GameObject Fusion;
    GameObject Adaptation;

// Variáveis que representam os Botões que levam ao conteúdo de cada parâmetro
    GameObject FusionButton;
    GameObject AdaptationButton;

// Componente de imagem de cada botão
    private Image imgFusion;
    private Image imgAdaptation;

// Imagens de fundo que aparecem nos botões
    public Sprite sprite1; // Drag your first sprite here
    public Sprite sprite2; // Drag your second sprite here
    
    
// Start é chamado antes do primeiro Update
    void Start()
    {

        Fusion = GameObject.Find("Canvas").transform.Find("Fusion Menu").gameObject;
        Adaptation = GameObject.Find("Canvas").transform.Find("Adaptation Menu").gameObject;

        FusionButton = GameObject.Find("Fusion Button");
        AdaptationButton = GameObject.Find("Adaptation Button");

        imgFusion = FusionButton.GetComponent<Image>();
        imgAdaptation = AdaptationButton.GetComponent<Image>();

    }

// Update é chamado uma vez por frame
    void Update()
    {

        if (Fusion.activeSelf)
        {
            imgFusion.sprite = sprite1;
        } else
        {
            imgFusion.sprite = sprite2;
        }

        if (Adaptation.activeSelf)
        {
            imgAdaptation.sprite = sprite1;
        } else
        {
            imgAdaptation.sprite = sprite2;
        }

    }
}
