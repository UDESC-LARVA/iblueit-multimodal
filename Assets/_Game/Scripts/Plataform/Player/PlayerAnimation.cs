using Ibit.Core.Data;
using Ibit.Core.Util;

namespace Ibit.Plataform
{
    public partial class Player
    {
        private void Animate(string msg)
        {
            if (msg.Length < 1)
                return;

            var f = Parsers.Float(msg);

            f = f < -Pacient.Loaded.PitacoThreshold || f > Pacient.Loaded.PitacoThreshold ? f : 0f;

            this.animator.Play(f < 0 ? "Dolphin-Jump" : "Dolphin-Move");
        }
    }
}