using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Serial;

namespace Ibit.MainMenu.UI
{
    public class UnlockIfManoConnected : MonoBehaviour
    {

        [SerializeField]
        private SerialControllerMano serialControllerMano;
  

        private bool ManoConnected = false;

        private void OnEnable()
        {

            if (serialControllerMano == null)
                serialControllerMano = FindObjectOfType<SerialControllerMano>();


            if (serialControllerMano.IsConnected)
                ManoConnected = true;



            this.GetComponent<Button>().interactable = Pacient.Loaded != null && (ManoConnected == true);
        }
    }
}