using UnityEngine;
using UnityEngine.UI;

namespace Depth
{
    public class PlayerCamRender : MonoBehaviour
    {
        public int Player;

        private Image thisImage;
        private Material thisMaterial;

        public void Awake()
        {
            thisImage = GetComponent<Image>();
            thisMaterial = new Material(Shader.Find("Unlit/Texture"));
        }

        public void Update()
        {
            if (thisImage.material == thisMaterial)
                return;

            var texture = RenderController.Instance.GetPlayerCam(Player);
            if (texture != null)
            {
                thisMaterial.mainTexture = texture;
                thisImage.material = thisMaterial;
            }
        }
    }
}
