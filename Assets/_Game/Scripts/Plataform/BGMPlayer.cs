using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ibit.Core.Audio;
using Ibit.Plataform.Data;
using UnityEngine;

namespace Ibit.Plataform
{
    public class BGMPlayer : MonoBehaviour
    {
        private int numDay, numAfternoon, numNight;
        private IEnumerable<string> musicsInStreamingAssets;

        private void Awake()
        {
            musicsInStreamingAssets = Directory.GetFiles(Application.streamingAssetsPath + @"/Music").Where(filename => filename.EndsWith(".ogg"));

            numDay = SoundManager.Instance.Sounds.Count(x => x.name.Contains("BGM_Day"));
            numAfternoon = SoundManager.Instance.Sounds.Count(x => x.name.Contains("BGM_Afternoon"));
            numNight = SoundManager.Instance.Sounds.Count(x => x.name.Contains("BGM_Night"));
        }

        private void PlayBGM()
        {
            switch (StageModel.Loaded.Phase)
            {
                case 1:
                    SoundManager.Instance.PlaySound($"BGM_Day{Random.Range(1, numDay)}");
                    break;

                case 2:
                    SoundManager.Instance.PlaySound($"BGM_Afternoon{Random.Range(1, numAfternoon)}");
                    break;

                case 3:
                    SoundManager.Instance.PlaySound($"BGM_Night{Random.Range(1, numNight)}");
                    break;

                default:
                    SoundManager.Instance.PlayAnotherBgm();
                    break;
            }
        }

        private void PickRandomFromStreamingAssets()
        {
            var element = musicsInStreamingAssets.ElementAt(Random.Range(0, musicsInStreamingAssets.Count() - 1));
            var www = new WWW("file://" + element);

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = true;
            audioSource.loop = true;
            audioSource.clip = www.GetAudioClip(false, false);
            audioSource.Play();
        }

        private void Start()
        {
            if (musicsInStreamingAssets.Any())
            {
                if (Random.Range(0, 1) == 0)
                {
                    PlayBGM();
                }
                else
                {
                    PickRandomFromStreamingAssets();
                }
            }
            else
            {
                PlayBGM();
            }
        }
    }
}