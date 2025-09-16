using UnityEngine;
using System.Collections;

public class TimelineController : MonoBehaviour
{
    public GameObject character;
    public GameObject tree;
    public GameObject dragonFlies;
    public GameObject bench;
    public GameObject forestTrees;
    public GameObject creditsPanel;

    private CanvasGroup creditsCanvasGroup;
    public float fadeDuration = 1.5f;

    private void Start()
    {
        if (tree != null) tree.SetActive(false);
        if (dragonFlies != null) dragonFlies.SetActive(false);

        if (creditsPanel != null)
        {
            creditsCanvasGroup = creditsPanel.GetComponent <CanvasGroup>();
            if (creditsCanvasGroup == null)
                creditsCanvasGroup = creditsPanel.AddComponent <CanvasGroup>();

            creditsPanel.SetActive(false);
            creditsCanvasGroup.alpha = 0f;
        }
    }

    public void EnableCharacter() { if (character != null) character.SetActive(true); }
    public void DisableCharacter() { if (character != null) character.SetActive(false); }

    public void EnableTree() { if (tree != null) tree.SetActive(true); }
    public void DisableTree() { if (tree != null) tree.SetActive(false); }

    public void EnableDragonFlies() { if (dragonFlies != null) dragonFlies.SetActive(true); }
    public void DisableDragonFlies() { if (dragonFlies != null) dragonFlies.SetActive(false); }

    public void EnableBench() { if (bench != null) bench.SetActive(true); }
    public void DisableBench() { if (bench != null) bench.SetActive(false); }

    public void EnableForestTrees() { if (forestTrees != null) forestTrees.SetActive(true); }
    public void DisableForestTrees() { if (forestTrees != null) forestTrees.SetActive(false); }

    public void EnableCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
            StartCoroutine (FadeCanvasGroup (creditsCanvasGroup, 0f, 1f));
        }
    }

    public void DisableCredits()
    {
        if (creditsPanel != null)
            StartCoroutine (FadeOutAndDeactivate());
    }

    private IEnumerator FadeCanvasGroup (CanvasGroup canvGroup, float from, float to)
    {
        float elapsed = 0f;
        canvGroup.alpha = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp (from, to, elapsed / fadeDuration);
            yield return null;
        }

        canvGroup.alpha = to;
    }

    private IEnumerator FadeOutAndDeactivate()
    {
        yield return FadeCanvasGroup (creditsCanvasGroup, 1f, 0f);
        creditsPanel.SetActive(false);
    }
}
