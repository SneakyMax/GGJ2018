using System.Collections.Generic;
using UnityEngine;

namespace Depth.ChooseSubScreen
{
    public class CameraRenderers : MonoBehaviour
    {
        public static CameraRenderers Instance { get; private set; }

        public float Distance;

        private IList<Camera> cameras;
        private IList<GameObject> currentObjects;

        public void Awake()
        {
            Instance = this;

            cameras = GetComponentsInChildren<Camera>();

            currentObjects = new List<GameObject> {null, null, null, null};
        }

        public void Show(GameObject obj, int player)
        {
            if (currentObjects[player] != null)
            {
                Destroy(currentObjects[player]);
            }

            var instance = Instantiate(obj,
                cameras[player].transform.position + (cameras[player].transform.forward * Distance),
                Quaternion.identity);

            currentObjects[player] = instance;
        }
    }
}
