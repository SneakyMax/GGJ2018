using UnityEngine;
using XInputDotNetPure;

namespace Depth
{
    public class SuperRadarAbility : Ability
    {
        public float TagTime = 10;

        private Sub sub;
        private float lastTagTime;

        public void Awake()
        {
            sub = GetComponentInParent<Sub>();
        }

        public void Update()
        {
            if (GameplayManager.Instance.AllowInput == false)
                return;

            var percentCooledDown = (Time.time - lastTagTime) / Cooldown;
            sub.Panel.AbilityIndicator.Radial.Percent = 1.0f - percentCooledDown;

            if (sub.InputState.Buttons.X == ButtonState.Pressed && percentCooledDown >= 1)
            {
                foreach (var taggable in TaggableManager.Instance.AllTaggable)
                {
                    if (taggable == sub.Taggable)
                        continue;
                    TaggableManager.Instance.Tag(taggable, sub.Player, TagTime);
                }
                lastTagTime = Time.time;
            }
        }
    }
}
