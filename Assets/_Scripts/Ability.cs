﻿using UnityEngine;

namespace Depth
{
    public abstract class Ability : MonoBehaviour
    {
        public Sprite Icon;

        public float Cooldown;

        public string Name;

        public string SoundName;
    }
}