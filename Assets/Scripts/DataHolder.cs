using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public static string mainCharacter;
    public static int mainCharacterHealth = 5;
    public static int mainCharacterExperiencePoints = 0;
    public static int mainCharacterLevel = 1;
    public static int countOfPotions = 0;
    public static List<bool> scrolls = new List<bool> { false, false, false, false, false, false, false, false };
    public static int sceneNumber;
}
