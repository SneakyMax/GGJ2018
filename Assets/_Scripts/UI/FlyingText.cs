using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Depth.Assets._Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class FlyingText : MonoBehaviour
    {
        public float Time;

        public float Distance;

        public void Start()
        {
            var rect = (RectTransform) transform;
            rect.DOAnchorPosY(rect.anchoredPosition.y + Distance, Time);
            var text = GetComponent<Text>();
            text.DOFade(0, Time - 1).SetDelay(1);
            Destroy(gameObject, Time);
        }
    }
}
