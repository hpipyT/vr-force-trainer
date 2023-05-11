using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float ultimate;

    public bool ultFull = false;
    public bool ultActivated = false;

    Image healthBar;
    Image ultBar;

    void Awake()
    {
        maxHealth = 10;
        currentHealth = maxHealth;
        healthBar = GameObject.Find("Player/Camera Offset/HUD/HP/PlayerHealth").GetComponent<Image>();
        
        ultimate = 100;        
        ultBar = GameObject.Find("Player/Camera Offset/HUD/UP/PlayerUlt").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        ultBar.fillAmount = ultimate / 100;

        

        if (ultimate >= 100)
        {
            ultimate = 100;
            ultFull = true;
        }
        if (ultimate < 0)
        {
            ultimate = 0;
        }

        if (currentHealth >= 100)
        {
            currentHealth = 100;
        }
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

    }


    public void TakeDamage()
    {

        currentHealth--;
    }

    public void FillUltimate(float amount)
    {
        // 100 max 
        ultimate += amount;
    }




}

