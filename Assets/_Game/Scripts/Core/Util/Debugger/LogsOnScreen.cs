using UnityEngine;

namespace Ibit.Core.Util
{
    public partial class Debugger
    {
        private string _messageOutput;

        private void DisplayLogMessages()
        {
            GUI.Label(new Rect(0, 20, Screen.width / 2f, 50f), _messageOutput);
        }
    }
}