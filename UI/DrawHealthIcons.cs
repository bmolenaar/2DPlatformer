using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawHealthIcons : MonoBehaviour
{

    GameObject player;
    PlayerHealth playerHealth;
    float maxHealth;
    float curHealth;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private float xPos;
    private float yPos;

    List<GameObject> hearts = new List<GameObject>();
    

    private void Awake()
    {
        xPos = 100;
        yPos = -50;
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        maxHealth = playerHealth.playerMaxHealth;
        
    }

    void Start()
    {

        for (int i = 0; i < maxHealth; i++)
        {
            string num = i.ToString();
            GameObject heart = new GameObject(string.Format("Heart{0}", num));
            RectTransform rectTransform = heart.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            heart.tag = "Heart";

            rectTransform.transform.SetParent(gameObject.transform);
            rectTransform.localScale = new Vector3(0.35f, 0.35f, 1);
            rectTransform.anchoredPosition = new Vector2(xPos, yPos);

            Image img = heart.AddComponent<Image>();
            img.sprite = fullHeart;
            xPos += 40;
        }
       
        hearts.AddRange(GameObject.FindGameObjectsWithTag("Heart"));

    }


    void Update()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        curHealth = playerHealth.playerCurHealth;

        if (curHealth < maxHealth)
        {
            foreach (var h in hearts)
            {
                h.GetComponent<Image>().sprite = emptyHeart;
            }

            //If I have an even number of hearts left
            if (curHealth % 2 == 1 || curHealth % 2 == 0)
            {
                for (int i = 0; i < curHealth; i++)
                {
                    hearts[i].GetComponent<Image>().sprite = fullHeart;
                }
            }
            //If I have a decimal number of hearts left
            else if (curHealth % 2 == 0.5 || curHealth % 2 == 1.5)
            {
                for (int i = 0; i < Mathf.CeilToInt(curHealth); i++)
                {
                    if (i == Mathf.CeilToInt(curHealth) - 1)
                    {
                        hearts[i].GetComponent<Image>().sprite = halfHeart;
                    }
                    else
                    {
                        hearts[i].GetComponent<Image>().sprite = fullHeart;
                    }                    
                }
            }
        }
        else if (curHealth == maxHealth && hearts[hearts.Count - 1].GetComponent<Image>().sprite != fullHeart)
        {
            foreach (var h in hearts)
            {
                h.GetComponent<Image>().sprite = fullHeart;
            }
        }
        else
        {
            return;
        }

    }
}
