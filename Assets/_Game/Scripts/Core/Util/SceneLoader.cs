using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ibit.Core.Util
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;

        private IEnumerator LoadSceneAsync(int sceneIndex)
        {
            var operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone)
            {
                var progress = Mathf.Clamp01(operation.progress / 0.9f);

                loadingScreen.GetComponentInChildren<Slider>().value = progress; //Progresso da barra de carregamento

                //Debug.Log($"LoadingScene - sceneIndex:{sceneIndex} progress:{progress}");

                yield return null;
            }
        }

        public void LoadScene(int sceneIndex)
        {
            Instantiate(loadingScreen);
            StartCoroutine(LoadSceneAsync(sceneIndex));
        }
    }
}