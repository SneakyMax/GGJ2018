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
        private float lastBoostTime;
        private bool isShielding;

        public void Awake()
        {
            sub = GetComponentInParent<Sub>();
        }

        public void Update()
        {
            if (GameplayManager.Instance.AllowInput == false)
                return;

            var percentCooledDown = (Time.time - lastBoostTime) / Cooldown;
            sub.Panel.AbilityIndicator.Radial.Percent = 1.0f - percentCooledDown;

            var percentShielded = (Time.time - lastBoostTime) / ShieldTime;

            if (percentShielded >= 1)
            {
                isShielding = false;
            }

            if (sub.InputState.Buttons.X == ButtonState.Pressed && percentCooledDown >= 1)
            {
                isShielding = true;
                lastBoostTime = Time.time;
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
