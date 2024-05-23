using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacterController : MonoBehaviour
{
    [SerializeField] GameObject femaleCharacter;
    [SerializeField] GameObject maleCharacter;

    public void ChangeCharacter()
    {
        bool isActive = femaleCharacter.activeSelf;
        femaleCharacter.SetActive(!isActive);
        isActive = maleCharacter.activeSelf;
        maleCharacter.SetActive(!isActive);
    }

    public void ChooseCharacter()
    {
        bool isActive = femaleCharacter.activeSelf;
        if(isActive)
        {
            DataHolder.mainCharacter = "female";
        }
        else
        {
            DataHolder.mainCharacter = "male";
        }
    }
}
