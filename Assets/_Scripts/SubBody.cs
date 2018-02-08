using System.Collections;
using UnityEngine;

public class SubBody : MonoBehaviour
{
    public float FlashDuration;

    public Color FlashColor = Color.white;

    private Material bodyMaterial;

    public void Start()
    {
        bodyMaterial = GetComponentInChildren<MeshRenderer>().material;
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    public IEnumerator FlashCoroutine()
    {
        var startTime = Time.time;
        bodyMaterial.EnableKeyword("_EMISSION");
        while (Time.time - startTime < FlashDuration)
        {
            var opacity = 1.0f - ((Time.time - startTime) / FlashDuration);
            bodyMaterial.SetColor("_EmissionColor", FlashColor * opacity);
            yield return new WaitForEndOfFrame();
        }

        bodyMaterial.SetColor("_EmissionColor", FlashColor * 0);
    }
}