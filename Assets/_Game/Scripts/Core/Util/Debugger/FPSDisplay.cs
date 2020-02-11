/*
 * Adaptation from:
 * http://wiki.unity3d.com/index.php?title=FramesPerSecond
 */

using UnityEngine;

namespace Ibit.Core.Util
{
    public partial class Debugger
    {
        private float fpsTimer;
        private readonly GUIStyle _style = new GUIStyle();
        private Rect _rect;

        private void AlignFpsDisplayBox()
        {
            int w = Screen.width, h = Screen.height;
            _style.alignment = TextAnchor.UpperLeft;
            _style.fontSize = h * 1 / 40;
            _style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            _rect = new Rect(0, 0, w, h * 2 / 100);
        }

        private void UpdateFpsDisplayTimer()
        {
            fpsTimer += (Time.deltaTime - fpsTimer) * 0.1f;
        }

        private void DisplayFramesPerSecond()
        {
            var msec = fpsTimer * 1000.0f;
            var fps = 1.0f / fpsTimer;
            var text = $"{msec:0.0} ms ({fps:0.} fps)";
            GUI.Label(_rect, text, _style);
        }
    }
}