using Ibit.Core.Data;
using UnityEngine;
using UnityEngine.UI;
using Ibit.Core.Serial;

namespace Ibit.MainMenu.UI
{
    public class UnlockOnCalibration : MonoBehaviour
    {

        [SerializeField]
        private SerialControllerPitaco serialControllerPitaco;
        [SerializeField]
        private SerialControllerMano serialControllerMano;
        [SerializeField]
        private SerialControllerCinta serialControllerCinta;

        public bool PitacoPrecisaCalib;
        public bool ManoPrecisaCalib;
        public bool CintaPrecisaCalib;

        private void OnEnable()
        {
            PitacoPrecisaCalib = false;
            ManoPrecisaCalib = false;
            CintaPrecisaCalib = false;

            if (serialControllerPitaco == null)
                serialControllerPitaco = FindObjectOfType<SerialControllerPitaco>();

            if (serialControllerMano == null)
                serialControllerMano = FindObjectOfType<SerialControllerMano>();

            if (serialControllerCinta == null)
                serialControllerCinta = FindObjectOfType<SerialControllerCinta>();

            if (serialControllerPitaco.IsConnected && !Pacient.Loaded.IsCalibrationPitacoDone)
                PitacoPrecisaCalib = true;

            if (serialControllerMano.IsConnected && !Pacient.Loaded.IsCalibrationManoDone)
                ManoPrecisaCalib = true;

            if (serialControllerCinta.IsConnected && !Pacient.Loaded.IsCalibrationCintaDone)
                CintaPrecisaCalib = true;


            this.GetComponent<Button>().interactable = Pacient.Loaded != null && (PitacoPrecisaCalib == false && ManoPrecisaCalib == false && CintaPrecisaCalib == false);
        }
    }
}