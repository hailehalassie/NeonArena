using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    TextMeshProUGUI healthText;
    Player player;

    [SerializeField] int numberOfLives;
    public Image[] lives;
    public Sprite life;
    int timesDead;

    // Use this for initialization
    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }




    // Update is called once per frame
    void Update()
    {
        healthText.text = player.GetHealth().ToString();
    }


    public void Lives()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < numberOfLives)
            {
                lives[i].enabled = true;

            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }
}

    
    
