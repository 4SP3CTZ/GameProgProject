using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceSystem : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToLevelUp = 100;
    public int expIncreaseFactor = 2;
    public int moneyCurrent = 0;

    public Slider expBarSlider;
    public Text LevelText2D;
    public Text LevelText3D;
    public Text MoneyText2D;
   
    void Update()
    {
        UpdateUI();
    }

    private void GainExperienceFromEnemy(int amount)
    {
        GainExperience(amount);
    }

    private void GainExperience(int amount)
    {
        currentExp += amount;
        moneyCurrent += amount * 5;

        while (currentExp >= expToLevelUp)
        {
            LevelUp();
        }

    }

    private void LevelUp()
    {
        currentLevel++;
        currentExp -= expToLevelUp;
        expToLevelUp *= expIncreaseFactor;
    }

    private void UpdateUI()
    {
        if (expBarSlider != null)
        {
            expBarSlider.maxValue = expToLevelUp;
            expBarSlider.value = currentExp;
        }

        if (LevelText2D != null)
        {
            LevelText2D.text = currentLevel.ToString();
        }

        if (LevelText3D != null)
        {
            LevelText3D.text = currentLevel.ToString();
        }
        if (MoneyText2D != null)
        {
            MoneyText2D.text = moneyCurrent.ToString();
        }
    }
}
