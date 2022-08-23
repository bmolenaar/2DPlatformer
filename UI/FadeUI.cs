using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    CanvasGroup cg;
    public Button respawnButton;
    public Button quitButton;

    float alphaMax = 0.8f;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    public IEnumerator Fade()
    {
        float counter = 0f;

        while (counter < 1 && cg.alpha < alphaMax)
        {
            counter += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0, alphaMax, counter);            
            yield return null;
        }
        respawnButton.interactable = true;
        quitButton.interactable = true;
    }
}
