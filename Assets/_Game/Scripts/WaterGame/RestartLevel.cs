using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ibit.WaterGame
{
    public class RestartLevel : MonoBehaviour
    {
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
