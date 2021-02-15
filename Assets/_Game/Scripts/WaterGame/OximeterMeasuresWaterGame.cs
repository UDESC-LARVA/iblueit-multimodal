using UnityEngine;
using UnityEngine.UI;

namespace Ibit.WaterGame
{
    public class OximeterMeasuresWaterGame : MonoBehaviour
    {
        private Player plr;
        private void Awake()
        {
            plr = FindObjectOfType<Player>();
        }

        private void Update()
        {   
            if (plr.oxiActive)
            { // Se o oxímetro estiver conectado, exibe seus valores
                GetComponent<Text>().text = $"HR: {plr.HRValue} bpm\nSPO2: {plr.SPO2Value} %";
            } else {
                GetComponent<Text>().text = "";
            }
        }
    }
}