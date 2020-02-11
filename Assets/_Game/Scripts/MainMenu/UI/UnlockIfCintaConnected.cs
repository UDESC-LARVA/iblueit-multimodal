using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Serial;

namespace Ibit.MainMenu.UI
{
    public class UnlockIfCintaConnected : MonoBehaviour
    {


        [SerializeField]
        private SerialControllerCinta serialControllerCinta;

        private bool CintaConnected = false;

        private void OnEnable()
        {

            if (serialControllerCinta == null)
                serialControllerCinta = FindObjectOfType<SerialControllerCinta>();


            if (serialControllerCinta.IsConnected)
                CintaConnected = true;



            this.GetComponent<Button>().interactable = Pacient.Loaded != null && (CintaConnected == true);
        }
    }
}