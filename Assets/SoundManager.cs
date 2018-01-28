using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
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
                sounds.Add(source.clip.name, source);
            }
        }

        public static void PlaySound(string sound)
        {
            Instance.PlaySoundInternal(sound);
        }

        private void PlaySoundInternal(string soundName)
        {
            AudioSource sound;
            var found = sounds.TryGetValue(soundName, out sound);
            if (found)
            {
                sound.Play();
            }
        }
    }
}
