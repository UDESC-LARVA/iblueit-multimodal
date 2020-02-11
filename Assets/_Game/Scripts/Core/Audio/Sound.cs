using UnityEngine;

namespace Ibit.Core.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1.5f)]
        public float volume = 0.7f;

        [Range(0f, 1.5f)]
        public float pitch = 1f;

        [Range(0f, 0.5f)]
        public float randomVolume = 0.1f;

        [Range(0f, 0.5f)]
        public float randomPitch = 0.1f;

        public bool loop;
        public bool playOnAwake;

        private AudioSource audioSource;

        public void SetSource(AudioSource source)
        {
            audioSource = source;
            audioSource.clip = clip;
        }

        public void Play(bool oneshot = false)
        {
            audioSource.volume = volume * (1f + Random.Range(-randomVolume / 2f, randomVolume / 2f));
            audioSource.pitch = pitch * (1f + Random.Range(-randomPitch / 2f, randomPitch / 2f));
            audioSource.loop = loop;

            if (oneshot)
                audioSource.PlayOneShot(this.clip);
            else
                audioSource.Play();
        }

        public void Pause()
        {
            if (!audioSource.isPlaying)
                return;

            audioSource.Pause();
        }

        public void Resume()
        {
            if (audioSource.isPlaying)
                return;

            audioSource.UnPause();
        }
    }
}