using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTrailController : MonoBehaviour
{
    public float delayBetweenPairs = 0.5f;

    // Butterfly
    public GameObject objectToActivateWithFlowers;

    private List <Animator[]> flowerPairs = new List <Animator[]>();

    private void Start()
    {
        foreach (Transform pair in transform)
        {
            Animator[] animators = pair.GetComponentsInChildren <Animator>();
            if (animators.Length == 2)
            {
                flowerPairs.Add(animators);

                // Hide Flowers And Butterfly At Start
                foreach (Animator anim in animators)
                {
                    anim.gameObject.SetActive (false);
                }
            }
        }

        if (objectToActivateWithFlowers != null)
        {
            objectToActivateWithFlowers.SetActive (false);
        }
    }

    // Reveal Flowers And Butterfly
    public void ShowFlowerTrail()
    {
        foreach (var pair in flowerPairs)
        {
            foreach (Animator anim in pair)
            {
                anim.gameObject.SetActive (true);
            }
        }

        if (objectToActivateWithFlowers != null)
        {
            objectToActivateWithFlowers.SetActive(true);
        }
    }

    // Trigger Flower Animation
    public void TriggerFlowerTrail()
    {
        StartCoroutine (OpenFlowerSequence());
    }

    private IEnumerator OpenFlowerSequence()
    {
        foreach (var pair in flowerPairs)
        {
            foreach (var animator in pair)
            {
                animator.SetTrigger ("isOpen");
            }

            yield return new WaitForSeconds (delayBetweenPairs);
        }
    }

    // Hide Flowers & Butterfly 
    public void HideFlowerTrail()
    {
        foreach (var pair in flowerPairs)
        {
            foreach (Animator anim in pair)
            {
                anim.gameObject.SetActive (false);
            }
        }

        if (objectToActivateWithFlowers != null)
        {
            objectToActivateWithFlowers.SetActive (false);
        }
    }
}
