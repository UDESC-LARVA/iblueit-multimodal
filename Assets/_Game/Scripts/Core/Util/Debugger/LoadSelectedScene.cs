using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ibit.Core.Util
{
    public partial class Debugger
    {
        [SerializeField] private string sceneToLoad;

        [ContextMenu("Load Selected Scene")]
        private void LoadScene()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}