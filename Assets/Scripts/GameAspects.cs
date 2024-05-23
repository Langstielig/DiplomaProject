using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAspects : MonoBehaviour
{
    [SerializeField] private List<GameObject> hearts;
    [SerializeField] private List<GameObject> emptyHearts;
    [SerializeField] private Text experiencePointsText;
    [SerializeField] private Text levelText;

    private string experiencePointsFormat = "{0}/10";
    private string levelFormal = "L{0}";

    public void AddHeart()
    {
        if(DataHolder.mainCharacterHealth < 5)
        {
            hearts[DataHolder.mainCharacterHealth - 1].SetActive(true);
            emptyHearts[DataHolder.mainCharacterHealth - 1].SetActive(false);
        }
    }

    public void RemoveHeart()
    {
        if(DataHolder.mainCharacterHealth > 0)
        {
            hearts[DataHolder.mainCharacterHealth - 1].SetActive(false);
            emptyHearts[DataHolder.mainCharacterHealth - 1].SetActive(true);
            DataHolder.mainCharacterHealth--;
        }
    }

    public void AddExperiencePoint()
    {
        if(DataHolder.mainCharacterExperiencePoints < 10)
        {
            DataHolder.mainCharacterExperiencePoints++;
            experiencePointsText.text = String.Format(experiencePointsFormat, DataHolder.mainCharacterExperiencePoints);
        }
        else
        {
            DataHolder.mainCharacterLevel++;
            levelText.text = String.Format(levelFormal, DataHolder.mainCharacterLevel);
            DataHolder.mainCharacterExperiencePoints = 0;
            experiencePointsText.text = String.Format(experiencePointsFormat, DataHolder.mainCharacterExperiencePoints);
        }
    }
}
