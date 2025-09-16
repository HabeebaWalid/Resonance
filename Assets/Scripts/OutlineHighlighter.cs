using UnityEngine;
using System.Collections;

public class OutlineHighlighter : MonoBehaviour
{
    [Header ("Target To Highlight")]
    public GameObject targetObject;

    [Header ("Materials")]
    public Material glowMaterial;
    public Material fullscreenMaterial;

    [Header ("Outline Settings")]
    public float targetIntensity = 5f;
    public float duration = 1f;

    [Header ("Vignette Settings")]
    public string vignetteProperty = "_VignettePower";
    public float vignetteMax = 6f;
    public float vignetteMin = 0f;

    private float originalIntensity;
    private Coroutine highlightCoroutine;
    private Coroutine vignetteCoroutine;
    private int outlineLayer;

    void Start()
    {
        outlineLayer = LayerMask.NameToLayer ("OutlineObject");
        if (outlineLayer == -1)
            Debug.LogError ("'OutlineObject' Does Not Exist.");

        if (glowMaterial != null)
        {
            originalIntensity = glowMaterial.GetFloat ("_Intensity");
            ResetOutline();
        }
        else
        {
            Debug.LogWarning ("No Material Assigned For Glow Material.");
        }

        if (fullscreenMaterial != null)
        {
            fullscreenMaterial.SetFloat (vignetteProperty, vignetteMax);
        }
        else
        {
            Debug.LogWarning ("No Fullscreen Material Assigned.");
        }

        if (targetObject == null)
            targetObject = this.gameObject;
    }


    public void GlowTrigger (bool enable)
    {
        if (enable)
        {
            SetOutlineLayer(true);
            Highlight();
            FadeVignette (vignetteMax, vignetteMin);
        }
        else
        {
            ResetOutline();
            ResetVignette();
            SetOutlineLayer(false);
        }
    }

    private void SetOutlineLayer (bool enable)
    {
        if (targetObject == null) return;

        int targetLayer = enable ? outlineLayer : 0;
        SetLayerRecursively (targetObject, targetLayer);
    }

    private void SetLayerRecursively (GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively (child.gameObject, layer);
        }
    }

    public void Highlight()
    {
        if (glowMaterial == null) return;

        glowMaterial.SetFloat ("_Size", 0f);
        glowMaterial.SetFloat ("_Intensity", 0f);

        if (highlightCoroutine != null)
            StopCoroutine (highlightCoroutine);

        highlightCoroutine = StartCoroutine (IncreaseIntensityAndSize());
    }

    private IEnumerator IncreaseIntensityAndSize()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01 (elapsed / duration);
            t = Mathf.Pow (t, 3);

            glowMaterial.SetFloat ("_Intensity", Mathf.Lerp (0f, targetIntensity, t));
            glowMaterial.SetFloat ("_Size", Mathf.Lerp (0.00f, 0.06f, t));

            yield return null;
        }
    }

    private IEnumerator FadeOutOutline()
    {
        float startIntensity = glowMaterial.GetFloat ("_Intensity");
        float startSize = glowMaterial.GetFloat ("_Size");
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01 (elapsed / duration);

            glowMaterial.SetFloat ("_Intensity", Mathf.Lerp (startIntensity, 0f, t));
            glowMaterial.SetFloat ("_Size", Mathf.Lerp (startSize, 0f, t));

            yield return null;
        }

        ResetOutline();
    }

    public void ResetOutline()
    {
        if (glowMaterial != null)
        {
            glowMaterial.SetFloat ("_Size", 0f);
            glowMaterial.SetFloat ("_Intensity", 0f);
        }
    }

    private IEnumerator ResetAfterFade()
    {
        if (highlightCoroutine != null)
            StopCoroutine (highlightCoroutine);

        yield return StartCoroutine (FadeOutOutline());
        SetOutlineLayer(false);
        ResetVignette();
    }

    public void FadeVignette (float from, float to)
    {
        if (fullscreenMaterial == null) return;

        if (vignetteCoroutine != null)
            StopCoroutine (vignetteCoroutine);

        vignetteCoroutine = StartCoroutine (ChangeVignette (from, to));
    }

    private IEnumerator ChangeVignette (float startValue, float endValue)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fullscreenMaterial.SetFloat (vignetteProperty,
                Mathf.Lerp (startValue, endValue, elapsed / duration));
            yield return null;
        }
    }

    public void ResetVignette()
    {
        if (fullscreenMaterial != null)
            FadeVignette (fullscreenMaterial.GetFloat (vignetteProperty), vignetteMax);
    }
}
