using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject lines;
    [SerializeField] private List<GameObject> potions;

    private int numberOfCurrentPotion = 0;

    private void Start()
    {
        background.SetActive(false);
        lines.SetActive(false);
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
}
