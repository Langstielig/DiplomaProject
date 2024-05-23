using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesScript : MonoBehaviour
{
    public void Scenes(int numberOfScene)
    {
        SceneManager.LoadScene(numberOfScene);
    }
}
