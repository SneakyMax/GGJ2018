using UnityEngine;
using XInputDotNetPure;

namespace Depth
{
    public class ShieldAbility : Ability
    {
        public MeshRenderer ShieldMesh;

        public float ShieldTime = 1;

        public float ShieldAnimateSpeed = 1;

        private Sub sub;
        private float lastShieldTime;
        private bool isShielding;

        public bool IsActive { get { return isShielding; } }

        public void Awake()
        {
            sub = GetComponentInParent<Sub>();
            lastShieldTime = -500;
        }

        public void Update()
        {
            if (GameplayManager.Instance.AllowInput == false)
                return;

            var percentCooledDown = (Time.time - lastShieldTime) / Cooldown;
            sub.Panel.AbilityIndicator.Radial.Percent = 1.0f - percentCooledDown;

            var percentShielded = (Time.time - lastShieldTime) / ShieldTime;

            if (percentShielded >= 1)
            {
                isShielding = false;
            }

            if (sub.InputState.Buttons.X == ButtonState.Pressed && percentCooledDown >= 1)
            {
                isShielding = true;
                lastShieldTime = Time.time;
            }

            ShieldMesh.enabled = isShielding;

            if (isShielding)
            {
                ShieldMesh.material.mainTextureOffset += new Vector2(ShieldAnimateSpeed, ShieldAnimateSpeed) *
                                                         Time.deltaTime;
            }
        }
    }
}
