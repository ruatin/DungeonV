using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    
    public Text pointText;
    public Text hpText;
    public Text enemySpawnText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }



    public void SetPointText(int value)
    {
        pointText.text = value.ToString();
    }
    
    public void SetHpText(int value)
    {
        hpText.text = $"Hp : {value}";
    }
    
    public void SetEnemySpawnText(int value)
    {
        enemySpawnText.text = $"Enemy Spawn\n{value}";
    }
}
