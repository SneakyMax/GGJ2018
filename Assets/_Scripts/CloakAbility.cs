using System;
using UnityEngine;
using XInputDotNetPure;

namespace Depth
{
    public class CloakAbility : Ability
    {
        public float CloakTime = 1;

        private Sub sub;
        private float lastCloakTime;
        private bool isCloaked;

        public GameObject NotificationTextPrefab;

        public void Awake()
        {
            sub = GetComponentInParent<Sub>();
            lastCloakTime = -500;
        }

        public void Update()
        {
            if (GameplayManager.Instance.AllowInput == false)
                return;

            var percentCooledDown = (Time.time - lastCloakTime) / Cooldown;
            sub.Panel.AbilityIndicator.Radial.Percent = 1.0f - percentCooledDown;

            var percentCloaked = (Time.time - lastCloakTime) / CloakTime;

            if (percentCloaked >= 1)
            {
                isCloaked = false;
            }

            if (sub.InputState.Buttons.X == ButtonState.Pressed && percentCooledDown >= 1)
            {
                if (!String.IsNullOrEmpty(SoundName))
                    SoundManager.PlaySound(SoundName);
                isCloaked = true;
                var textInstance = Instantiate(NotificationTextPrefab, sub.Panel.transform, false);
                textInstance.GetComponent<FormattableText>().Format("Cloak");
                lastCloakTime = Time.time;
            }

            sub.Taggable.Active = !isCloaked;
            sub.IsForceHidden = isCloaked;
        }
    }
}
