using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class DeathScreen : MonoBehaviour
{
    Image deathPanel;
    public float fadeRate = 5f;

    void Start()
    {
        deathPanel = GetComponent<Image>();
    }

    public IEnumerator Draw()
    {
        float r = deathPanel.color.r;
        float g = deathPanel.color.g;
        float b = deathPanel.color.b;

        while(deathPanel.color.a < 0.75f)
        {
            deathPanel.color = new Color(r, g, b, fadeRate * Time.deltaTime);
            

        }

        yield return null;
    }
}
