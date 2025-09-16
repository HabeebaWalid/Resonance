using UnityEngine;
using TMPro;

public class CreditsScroller : MonoBehaviour
{
    [Header ("Scrolling Settings")]
    public float scrollSpeed = 50f;
    public RectTransform creditsText;  
    public float startY = -600f;
    public float endY = 1200f;

    private bool isRolling = false;

    void Start()
    {
        // Reset Text Position
        creditsText.anchoredPosition = new Vector2 (0, startY);
    }

    void Update()
    {
        if (isRolling)
        {
            creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (creditsText.anchoredPosition.y >= endY)
            {
                isRolling = false; 
            }
        }
    }

    public void StartCredits()
    {
        isRolling = true;
        // Reset Each Time
        creditsText.anchoredPosition = new Vector2 (0, startY); 
    }
}
