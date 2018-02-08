using System.Collections.Generic;
using UnityEngine;

namespace Depth
{
    public class Taggable : MonoBehaviour
    {
        public Rigidbody Rigidbody { get; private set; }

        public int[] Layers { get { return layers; } }

        public Material TagMaterial;

        private readonly int[] layers =
        {
            8, 9, 10, 11
        };

        private IList<TaggableRenderer> subRenderers;
        private int lastLayer;

        private void Awake()
        {
            subRenderers = new List<TaggableRenderer>();
            Rigidbody = GetComponentInChildren<Rigidbody>();
        }

        private void Start ()
        {
            TaggableManager.Instance.Add(this);

            foreach (var childMeshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                var subRenderer = childMeshRenderer.gameObject.AddComponent<TaggableRenderer>();
                subRenderer.TagMaterial = TagMaterial;
                subRenderers.Add(subRenderer);
            }
        }

        public void OnDestroy()
        {
            TaggableManager.Instance.Remove(this);
        }

        private void Update ()
        {
		
        }

        private void SetLayer(int player)
        {
            var layer = Layers[player];
            foreach (var renderable in GetComponentsInChildren<MeshFilter>())
            {
                if (renderable.gameObject.layer == 0)
                {
                    renderable.gameObject.layer = layer;
                }
            }
            lastLayer = layer;
        }

        private void ResetLayer()
        {
            foreach (var renderable in GetComponentsInChildren<MeshFilter>())
            {
                if ( renderable.gameObject.layer == lastLayer )
                    renderable.gameObject.layer = 0;
            }
        }

        private void ResetMaterials()
        {
            foreach (var subRenderer in subRenderers)
            {
                subRenderer.Reset();
            }
        }

        private void OverwriteMaterials(float opacity)
        {
            foreach (var subRenderer in subRenderers)
            {
                subRenderer.SetTag(opacity);
            }
        }

        public void Reset(TaggableManager.TagInfo info)
        {
            ResetLayer();
            ResetMaterials();
        }

        public void Prepare(TaggableManager.TagInfo info)
        {
            SetLayer(info.Player);

            var opacity = 1.0f - Mathf.Clamp01((Time.time - info.StartTime) / info.Duration);

            OverwriteMaterials(opacity);
        }
    }
}
