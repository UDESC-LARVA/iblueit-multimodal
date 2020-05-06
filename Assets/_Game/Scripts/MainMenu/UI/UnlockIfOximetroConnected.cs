using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Serial;

namespace Ibit.MainMenu.UI
{
    public class UnlockIfOximetroConnected : MonoBehaviour
    {

        [SerializeField]
        private SerialControllerOximetro serialControllerOximetro;
   

        private bool OximetroConnected = false;
   

        private void OnEnable()
        {

            if (serialControllerOximetro == null)
                serialControllerOximetro = FindObjectOfType<SerialControllerOximetro>();


            if (serialControllerOximetro.IsConnected)
                OximetroConnected = true;



            this.GetComponent<Button>().interactable = Pacient.Loaded != null && (OximetroConnected == true);
        }
    }
}