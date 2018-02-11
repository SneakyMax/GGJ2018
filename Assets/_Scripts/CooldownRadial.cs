using UnityEngine;
using UnityEngine.UI;

namespace Depth
{
    [RequireComponent(typeof(Image))]
    public class CooldownRadial : MonoBehaviour
    {
        public float Percent
        {
            get { return percent; }
            set
            {
                percent = Mathf.Clamp01(value); 
                thisImage.material.SetFloat("_Percent", percent);
            }
        }

        private float percent;
        
        public bool Flip;

        private Image thisImage;
        private Material thisMaterial;

        public void Awake()
        {
            thisImage = GetComponent<Image>();
            thisMaterial = new Material(thisImage.material);
            thisImage.material = thisMaterial;
            thisImage.material.SetFloat("_FlipRadial", Flip ? 1 : 0);
        }

        public void Start()
        {
            Percent = 0;
        }
    }
}