using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ibit.Core.Util
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        

        public void LoadScene(int sceneIndex)
        {
            Instantiate(loadingScreen);
            StartCoroutine(LoadSceneAsync(sceneIndex));
        }

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



        public void LoadSceneName(string sceneName)
        {
            Instantiate(loadingScreen);
            StartCoroutine(LoadSceneNameAsync(sceneName));
        }

        private IEnumerator LoadSceneNameAsync(string sceneName)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                var progress = Mathf.Clamp01(operation.progress / 0.9f);

                loadingScreen.GetComponentInChildren<Slider>().value = progress; //Progresso da barra de carregamento
                yield return null;
            }
        }
    }
}