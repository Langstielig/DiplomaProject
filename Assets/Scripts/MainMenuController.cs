using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject loadMenu;

    public void OpenOrCloseLoadMenu()
    {
        bool isOpened = loadMenu.activeSelf;
        loadMenu.SetActive(!isOpened);
        isOpened = mainMenu.activeSelf;
        mainMenu.SetActive(!isOpened);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
