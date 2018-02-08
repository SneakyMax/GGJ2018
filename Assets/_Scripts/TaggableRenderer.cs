using UnityEngine;

namespace Depth
{
    [RequireComponent(typeof(MeshRenderer))]
    public class TaggableRenderer : MonoBehaviour
    {
        public Material TagMaterial;

        private Material originalMaterial;
        private MeshRenderer meshRenderer;
        private Material myMaterial;

        public void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Start()
        {
            myMaterial = new Material(TagMaterial);
            originalMaterial = meshRenderer.material;
        }

        public void SetTag(float opacity)
        {
            meshRenderer.material = myMaterial;
            myMaterial.SetFloat("_Opacity", opacity);
        }

        public void Reset()
        {
            meshRenderer.material = originalMaterial;
        }
    }
}