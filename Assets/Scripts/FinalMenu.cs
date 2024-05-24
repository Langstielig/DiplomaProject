using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMenu : MonoBehaviour
{
    [SerializeField] private GameObject winWindow;
    [SerializeField] private GameObject loseWindow;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        bool flag = true;
        for(int i = 0; i < DataHolder.scrolls.Count; i++)
        {
            flag = flag && DataHolder.scrolls[i];
            Debug.Log("Flag is " + flag);
        }
        Debug.Log("Health is" + DataHolder.mainCharacterHealth);
        if(flag && DataHolder.mainCharacterHealth > 0)
        {
            ShowWinWindow();
        }
        else
        {
            ShowLoseWindow();
        }    
    }

    public void ShowWinWindow()
    {
        winWindow.SetActive(true);
        loseWindow.SetActive(false);
    }

    public void ShowLoseWindow()
    {
        winWindow.SetActive(false);
        loseWindow.SetActive(true);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
