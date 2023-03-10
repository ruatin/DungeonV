using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerCurrentHp;
    public int playerMaxHp = 50;
    
    private bool isDestroy;


    private void Awake()
    {
        playerCurrentHp = playerMaxHp;
    }

    public void Damaged(int amount)
    {
        if (isDestroy)
        {
            return;
        }
        
        playerCurrentHp -= amount;

        UiManager.Instance.SetHpText(playerCurrentHp); 

        if (playerCurrentHp <= 0)
        {
            isDestroy = true;
            playerCurrentHp = 0;
        }
    }
}
