using Ibit.Core.Audio;
using UnityEngine;

namespace Ibit.Calibration
{
    public partial class CalibrationManagerMano
    {
        private void DudeTalk(string msg)
        {
            _dialogText.text = msg;
            _dudeObject.GetComponent<Animator>().SetBool("Talking", true);
        }

        private void DudeClearMessage()
        {
            _dialogText.text = "";
            _dudeObject.GetComponent<Animator>().SetBool("Talking", false);
        }

        private void DudeCongratulate()
        {
            SoundManager.Instance.PlaySound("Success");
            DudeTalk("Muito bem! Pressione (Enter) para continuar.");
        }

        private void DudeWarnUnknownFlow()
        {
            SoundManager.Instance.PlaySound("Failure");
            DudeTalk("Não consegui medir seu exercício. Vamos tentar novamente? Pressione (Enter) para continuar.");
        }

        private void DudeWarnManoDisconnected()
        {
            DudeTalk("O Manovacuômetro não está conectado. Conecte-o ao computador! Pressione (Enter) para continuar.");
        }
    }
}