using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Depth.UI
{
    public class LockedOnWarning : MonoBehaviour
    {
        public float FlashRate = 0.5f;

        private Image image;

        public void Awake()
        {
            image = GetComponent<Image>();
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            StartCoroutine(Loop());
        }

        private IEnumerator Loop()
        {
            image.enabled = true;
            while (true)
            {
                yield return new WaitForSeconds(FlashRate);
                image.enabled = false;
                yield return new WaitForSeconds(FlashRate);
                image.enabled = true;
            }
        }
    }
}