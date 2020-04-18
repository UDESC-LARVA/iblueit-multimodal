using Ibit.Core.Data;
using Ibit.Core.Util;
using UnityEngine;
using Ibit.Core.Serial;

namespace Ibit.MainMenu.UI
{
    public class PlayerMenuUI : MonoBehaviour
    {
        [SerializeField]
        private SerialControllerPitaco serialControllerPitaco;
        [SerializeField]
        private SerialControllerMano serialControllerMano;
        [SerializeField]
        private SerialControllerCinta serialControllerCinta;

        private bool PitacoPrecisaCalib;
        private bool ManoPrecisaCalib;
        private bool CintaPrecisaCalib;

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

            if(Pacient.Loaded.Name != null)
            {

                if (serialControllerPitaco.IsConnected && !Pacient.Loaded.IsCalibrationPitacoDone)
                {
                    PitacoPrecisaCalib = true;
                }

                if (serialControllerMano.IsConnected && !Pacient.Loaded.IsCalibrationManoDone)
                {
                    ManoPrecisaCalib = true;
                }

                if (serialControllerCinta.IsConnected && !Pacient.Loaded.IsCalibrationCintaDone)
                {
                    CintaPrecisaCalib = true;
                }

                if (PitacoPrecisaCalib == true || ManoPrecisaCalib == true || CintaPrecisaCalib == true)
                {
                    SysMessage.Info("Para começar a jogar, você precisa \ncalibrar todos os dispositivos conectados!");
                }
            }
        }
    }
}
