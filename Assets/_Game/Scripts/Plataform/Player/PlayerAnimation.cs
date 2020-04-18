using Ibit.Core.Data;
using Ibit.Core.Util;

namespace Ibit.Plataform
{
    public partial class Player
    {
        private void AnimatePitaco(string msg)
        {
            if (msg.Length < 1)
                return;
            
           // Se o Pitaco estiver conectado
            if (scp.IsConnected)
            {

                var f = Parsers.Float(msg);

                f = f < -Pacient.Loaded.PitacoThreshold || f > Pacient.Loaded.PitacoThreshold ? f : 0f;

                this.animator.Play(f < 0 ? "Dolphin-Jump" : "Dolphin-Move");
            }
        }

        private void AnimateMano(string msg)
        {
            if (msg.Length < 1)
                return;

            // Se o Pitaco não estiver conectado e Mano sim...
            if (!scp.IsConnected && scm.IsConnected)
            {

                var f = Parsers.Float(msg);

                f = f < -Pacient.Loaded.ManoThreshold || f > Pacient.Loaded.ManoThreshold ? f : 0f;

                this.animator.Play(f < 0 ? "Dolphin-Jump" : "Dolphin-Move");
            }
        }

        private void AnimateCinta(string msg)
        {
            if (msg.Length < 1)
                return;

            // Se o Pitaco não estiver conectado, o Mano não estiver conectado e a Cinta sim...
            if (!scp.IsConnected && !scm.IsConnected && scc.IsConnected)
            {
                var f = Parsers.Float(msg) + Pacient.Loaded.CapacitiesCinta.ExpPeakFlow;

                f = f < -Pacient.Loaded.CintaThreshold || f > Pacient.Loaded.CintaThreshold ? f : 0f;

                this.animator.Play(f < 0 ? "Dolphin-Jump" : "Dolphin-Move");
            }
        }
    }
}