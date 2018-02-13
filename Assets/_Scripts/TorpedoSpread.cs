using System;
using UnityEngine;
using XInputDotNetPure;

namespace Depth
{
    public class TorpedoSpread : Ability
    {
      

 

        private Sub sub;
        private Rigidbody subBody;
        private float lastFireTime;
        public GameObject TorpedoPrefab;
        public Transform torpedoPoint1;
        public Transform torpedoPoint2;
        public Transform torpedoPoint3;




        public void Awake()
        {
            sub = GetComponentInParent<Sub>();
            subBody = sub.GetComponentInChildren<Rigidbody>();
        }

        public void Update()
        {
            if (GameplayManager.Instance.AllowInput == false)
            {
                return;
            }

            var percentCooledDown = (Time.time - lastFireTime) / Cooldown;
            sub.Panel.AbilityIndicator.Radial.Percent = 1.0f - percentCooledDown;





            if (sub.InputState.Buttons.X == ButtonState.Pressed && percentCooledDown >= 1)
            {
                if (!String.IsNullOrEmpty(SoundName))
                    SoundManager.PlaySound(SoundName);

                SoundManager.PlaySound("burstshot_ability");

                SoundManager.PlaySound("TorpedoFire_2,5sec", 0.6f);

                lastFireTime = Time.time;
                FireTorpedo(torpedoPoint1);
                FireTorpedo(torpedoPoint2);
                FireTorpedo(torpedoPoint3);

            }

        }


        void FireTorpedo(Transform port)
        {
            var newTorpedoObj = Instantiate(TorpedoPrefab, port.position, port.rotation);
            var torpedo = newTorpedoObj.GetComponent<Torpedo>();
            torpedo.Parent = sub;
    
    
            TaggableManager.Instance.TagForAllBut(sub.Taggable, sub.Player, 2);
        }
    }
}
