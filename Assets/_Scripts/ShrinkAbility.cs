using System;
using UnityEngine;
using XInputDotNetPure;

namespace Depth
{
    public class ShrinkAbility : Ability
    {

        public GameObject NotificationTextPrefab;
 
        public enum ScaleState {Normal, Shrinking, Shrunk, Unshrinking};
        private Sub sub;
        private Rigidbody subBody;
        private float lastShrinkTime;
        public float shrinkSize;
        private float currentSize = 1;
        private float shrinkTimer;
        public float shrinkTime;
        public float changeSpeed;
        private ScaleState isShrunk = ScaleState.Normal;
        private Vector3 normalScale;
        
 




        public void Awake()
        {
            sub = GetComponentInParent<Sub>();
            subBody = sub.GetComponentInChildren<Rigidbody>();
            normalScale = transform.localScale;
            lastShrinkTime = -500;
        }

        public void Update()
        {
            if (GameplayManager.Instance.AllowInput == false)
            {
                return;
            }

            var percentCooledDown = (Time.time - lastShrinkTime) / Cooldown;
            sub.Panel.AbilityIndicator.Radial.Percent = 1.0f - percentCooledDown;





            if (sub.InputState.Buttons.X == ButtonState.Pressed && percentCooledDown >= 1)
            {
                if (!String.IsNullOrEmpty(SoundName))
                    SoundManager.PlaySound(SoundName);
                var textInstance = Instantiate(NotificationTextPrefab, sub.Panel.transform, false);
                textInstance.GetComponent<FormattableText>().Format("Shrink");
                lastShrinkTime = Time.time;
                isShrunk = ScaleState.Shrinking;
                shrinkTimer = 0;


            }

            if (isShrunk == ScaleState.Shrinking)
            {
                Shrink();
            }

            if (isShrunk == ScaleState.Shrunk || isShrunk == ScaleState.Shrinking)
            {
                shrinkTimer += Time.deltaTime;
                if (shrinkTimer >= shrinkTime)
                {
                    isShrunk = ScaleState.Unshrinking;
                }
            }

            if (isShrunk == ScaleState.Unshrinking)
            {
                unShrink();
            }
            


        }
        



        void Shrink()
        {
            
            transform.localScale = new Vector3(normalScale.x * currentSize, normalScale.y * currentSize, normalScale.z * currentSize);
            currentSize -= changeSpeed;
            if (currentSize <= shrinkSize)
            {
                isShrunk = ScaleState.Shrunk;
            }

        }
        void unShrink()
        {
            transform.localScale = new Vector3(normalScale.x * currentSize, normalScale.y * currentSize, normalScale.z * currentSize);
            currentSize += changeSpeed;
            if (currentSize >= 1)
            {
                isShrunk = ScaleState.Normal;
            }
        }
    }
}
