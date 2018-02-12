using System;
using UnityEngine;
using XInputDotNetPure;

namespace Depth
{
    public class BoostAbility : Ability
    {
        public GameObject NotificationTextPrefab;
        public ParticleGroup BoostParticles;

        public float BoostForce = 1;

        public float BoostTime = 1;

        private Sub sub;
        private Rigidbody subBody;
        private float lastBoostTime;
        private bool isBoosting;

        public void Awake()
        {
            sub = GetComponentInParent<Sub>();
            subBody = sub.GetComponentInChildren<Rigidbody>();
        }

        public void Update()
        {
            if (GameplayManager.Instance.AllowInput == false)
                return;

            var percentCooledDown = (Time.time - lastBoostTime) / Cooldown;
            sub.Panel.AbilityIndicator.Radial.Percent = 1.0f - percentCooledDown;

            var percentBoosted = (Time.time - lastBoostTime) / BoostTime;

            if (percentBoosted >= 1)
            {
                isBoosting = false;
            }

            if (sub.InputState.Buttons.X == ButtonState.Pressed && percentCooledDown >= 1)
            {
                if (!String.IsNullOrEmpty(SoundName))
                    SoundManager.PlaySound(SoundName);
                var textInstance = Instantiate(NotificationTextPrefab, sub.Panel.transform, false);
                textInstance.GetComponent<FormattableText>().Format("Boost");
                isBoosting = true;
                lastBoostTime = Time.time;
            }

            if (BoostParticles != null)
                BoostParticles.Emitting = isBoosting;
        }

        public void FixedUpdate()
        {
            if (isBoosting)
            {
                subBody.AddForce(sub.transform.forward * BoostForce, ForceMode.Acceleration);
            }
        }
    }
}
