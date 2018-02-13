using UnityEngine;

namespace Depth
{
    public class EveryScene : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}