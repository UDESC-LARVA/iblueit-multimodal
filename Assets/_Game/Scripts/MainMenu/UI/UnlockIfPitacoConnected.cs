using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Serial;

namespace Ibit.MainMenu.UI
{
    public class UnlockIfPitacoConnected : MonoBehaviour
    {

        [SerializeField]
        private SerialControllerPitaco serialControllerPitaco;
   

        private bool PitacoConnected = false;
   

        private void OnEnable()
        {

            if (serialControllerPitaco == null)
                serialControllerPitaco = FindObjectOfType<SerialControllerPitaco>();


            if (serialControllerPitaco.IsConnected)
                PitacoConnected = true;



            this.GetComponent<Button>().interactable = Pacient.Loaded != null && (PitacoConnected == true);
        }
    }
}