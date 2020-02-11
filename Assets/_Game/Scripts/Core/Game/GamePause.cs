using System;
using Ibit.Core.Audio;
using UnityEngine;

namespace Ibit.Core.Game
{
    public partial class GameManager
    {
        public static Action OnGamePause;
        public static Action OnGameUnPause;

        public static bool GameIsPaused { get; private set; }

        public static void PauseGame()
        {
            if (GameIsPaused)
                return;

            SoundManager.Instance?.PlaySound("GamePause");

            Time.timeScale = 0f;
            GameIsPaused = true;
            OnGamePause?.Invoke();
        }

        public static void UnPauseGame()
        {
            if (!GameIsPaused)
                return;

            SoundManager.Instance?.PlaySound("GameUnPause");

            Time.timeScale = 1f;
            GameIsPaused = false;
            OnGameUnPause?.Invoke();
        }
    }
}