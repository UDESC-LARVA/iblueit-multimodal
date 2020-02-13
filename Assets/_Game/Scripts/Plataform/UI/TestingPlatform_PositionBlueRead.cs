using UnityEngine;
using UnityEngine.UI;

namespace Ibit.Plataform.UI
{
    public class TestingPlatform_PositionBlueRead : MonoBehaviour
    {
        
        private Player plr;
        private void Awake()
        {
            plr = FindObjectOfType<Player>();
        } 

        private void Update()
        {
            GetComponent<Text>().text = plr.TestPositionValue.ToString();
        }
    }
}