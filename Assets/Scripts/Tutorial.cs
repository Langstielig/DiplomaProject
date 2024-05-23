using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorialWindow;
    [SerializeField] GameObject moveText;
    [SerializeField] GameObject attackAndDefenseText;
    [SerializeField] GameObject inventoryText;
    [SerializeField] GameObject cameraText;
    [SerializeField] GameObject interactionText;
    [SerializeField] GameObject pauseMenuText;

    private void Start()
    {
        HideAllWindows();
        StartCoroutine(ShowMoveText());
    }

    private void HideAllWindows()
    {
        tutorialWindow.SetActive(false);
        moveText.SetActive(false);
        attackAndDefenseText.SetActive(false);
        inventoryText.SetActive(false);
        cameraText.SetActive(false);
        interactionText.SetActive(false);
        pauseMenuText.SetActive(false);
    }

    IEnumerator ShowMoveText()
    {
        tutorialWindow.SetActive(true);
        moveText.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.W));

        StartCoroutine(ShowAttackAndDefenseText());
    }

    IEnumerator ShowAttackAndDefenseText()
    {
        moveText.SetActive(false);
        attackAndDefenseText.SetActive(true);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(1));

        StartCoroutine(ShowInventoryText());
    }

    IEnumerator ShowInventoryText()
    {
        attackAndDefenseText.SetActive(false);
        inventoryText.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Tab));

        StartCoroutine(ShowCameraText());
    }

    IEnumerator ShowCameraText()
    {
        inventoryText.SetActive(false);
        cameraText.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

        StartCoroutine(ShowInteractionText());
    }

    IEnumerator ShowInteractionText()
    {
        cameraText.SetActive(false);
        interactionText.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        StartCoroutine(ShowPauseMenuText());
    }

    IEnumerator ShowPauseMenuText()
    {
        interactionText.SetActive(false);
        pauseMenuText.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Escape));

        pauseMenuText.SetActive(false);
        tutorialWindow.SetActive(false);
    }
}
