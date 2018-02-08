using System.Collections.Generic;
using UnityEngine;

namespace Depth
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private IDictionary<string, AudioSource> sounds;

        public void Awake()
        {
            Instance = this;

            sounds = new Dictionary<string, AudioSource>();
            foreach (var source in GetComponentsInChildren<AudioSource>())
            {
                sounds[source.clip.name] = source;
            }
        }

        public static void PlaySound(string sound, float volume = 1.0f)
        {
            Instance.PlaySoundInternal(sound, volume);
        }

        private void PlaySoundInternal(string soundName, float volume)
        {
            AudioSource sound;
            var found = sounds.TryGetValue(soundName, out sound);
            if (found)
            {
                sound.PlayOneShot(sound.clip, volume);
            }
        }
    }
}
