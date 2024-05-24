using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject scrollBackground;
    [SerializeField] private GameObject lines;
    [SerializeField] private List<GameObject> potions;
    [SerializeField] private List<GameObject> scrolls;
    [SerializeField] private List<GameObject> scrollsText;

    private int numberOfCurrentPotion = 0;
    private GameObject currentScrollText = null;

    public bool isOpen = false;

    private void Start()
    {
        SetActiveFalse();
        SetPotionsAndScrolls();
    }

    private void Update()
    {
        CheckTabButton();
    }

    private void CheckTabButton()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = background.activeSelf;
            background.SetActive(!isActive);
            isActive = lines.activeSelf;
            lines.SetActive(!isActive);
            if(!isActive)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = !isActive;
            isOpen = !isOpen;
        }
    }

    public void AddPotion()
    {
        potions[numberOfCurrentPotion].SetActive(true);
        numberOfCurrentPotion++;
    }

    public void RemovePotion()
    {
        potions[numberOfCurrentPotion].SetActive(false);
        numberOfCurrentPotion--;
    }

    public void AddScroll(int index)
    {
        scrolls[index].SetActive(true);
    }

    private void SetPotionsAndScrolls()
    {
        for(int i = 0; i < DataHolder.countOfPotions; i++)
        {
            potions[i].SetActive(true);
            numberOfCurrentPotion++;
        }

        for(int i = 0; i < 8; i++)
        {
            scrolls[i].SetActive(DataHolder.scrolls[i]);
        }
    }

    private void SetActiveFalse()
    {
        background.SetActive(false);
        lines.SetActive(false);
        scrollBackground.SetActive(false);

        for(int i = 0; i < scrollsText.Count; i++)
        {
            scrollsText[i].SetActive(false);
        }
    }

    public void OpenScrollText(int numberOfScroll)
    {
        currentScrollText = scrollsText[numberOfScroll];

        background.SetActive(false);
        lines.SetActive(false);

        scrollBackground.SetActive(true);
        scrollsText[numberOfScroll].SetActive(true);
    }

    public void CloseScrollText()
    {
        currentScrollText.SetActive(false);
        scrollBackground.SetActive(false);

        background.SetActive(true);
        lines.SetActive(true);

        currentScrollText = null;
    }
}
