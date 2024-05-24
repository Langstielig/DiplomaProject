using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseWindow;
    [SerializeField] GameObject saveWindow;
    [SerializeField] GameObject loadWindow;

    public bool isOpen = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
        saveWindow.SetActive(false);
        loadWindow.SetActive(false);
    }

    private void Update()
    {
        CheckEscButton();
    }

    private void CheckEscButton()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ShowOrHidePauseMenu();
        }
    }

    public void ShowOrHidePauseMenu()
    {
        bool isActive = pauseMenu.activeSelf;
        if(isActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        pauseMenu.SetActive(!isActive);
        isOpen = !isOpen;
    }

    public void ShowOrHideSaveWindow()
    {
        bool isActive = pauseWindow.activeSelf;
        pauseWindow.SetActive(!isActive);
        isActive = saveWindow.activeSelf;
        saveWindow.SetActive(!isActive);
    }

    public void ShowOrHideLoadWindow()
    {
        bool isActive = pauseWindow.activeSelf;
        pauseWindow.SetActive(!isActive);
        isActive = loadWindow.activeSelf;
        loadWindow.SetActive(!isActive);
    }
}
