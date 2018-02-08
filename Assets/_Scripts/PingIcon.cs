using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Depth
{
    public class PingIcon : MonoBehaviour
    {
        public float Duration = 2;

        public float BlinkRate = 0.5f;

        private Image image;

        public void Start()
        {
            image = GetComponentInChildren<Image>();
            StartCoroutine(Coroutine());
        }

        private IEnumerator Coroutine()
        {
            var startTime = Time.time;
            while (Time.time - startTime < Duration)
            {
                image.enabled = true;
                yield return new WaitForSeconds(BlinkRate);
                image.enabled = false;
                yield return new WaitForSeconds(BlinkRate);
            }
            Destroy(gameObject);
        }
    }
}
