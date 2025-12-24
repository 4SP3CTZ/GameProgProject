using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrice : MonoBehaviour
{
    public int PriceOfItem = 0;

    public string ItemName;

    private ExperienceSystem currentMoney;
    
    
    public void Compare()
    {   
        currentMoney = FindObjectOfType<ExperienceSystem>();

        if (currentMoney == null)
        {
            Debug.LogError("ExperienceSystem not found!");
            return;
        }
        
        if (PriceOfItem <= currentMoney.moneyCurrent)
        {
            Debug.Log(ItemName + " Bought");
            currentMoney.moneyCurrent -= PriceOfItem;
        }
        else
        {
            Debug.Log("Cannot Buy Item");
        }
    }
}
