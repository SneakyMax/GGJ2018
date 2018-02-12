using Depth.ChooseSubScreen;
using UnityEngine;

namespace Depth
{
    public class PlayerSubChoices : MonoBehaviour
    {
        public static PlayerSubChoices Instance { get; private set; }

        public SubInfo[] Choices { get; private set; }

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Choices = new SubInfo[4];
            Instance = this;
        }

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Set(int player, SubInfo sub)
        {
            Choices[player] = sub;
        }
    }
}
