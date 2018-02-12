using UnityEngine;

namespace Depth
{
    public class SubInstantiator : MonoBehaviour
    {
        public void Awake()
        {
            var subChoice = PlayerSubChoices.Instance.Choices[GetComponent<Sub>().Player];
            Instantiate(subChoice.InGameSub, transform, false);
        }
    }
}
