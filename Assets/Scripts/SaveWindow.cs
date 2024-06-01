using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveWindow : MonoBehaviour
{
    [SerializeField] private GameObject loadFile;
    [SerializeField] private GameObject saveFiles;
    [SerializeField] private Text inputText;

    private void Start()
    {
        for (int i = 0; i < FilesNames.files.Count; i++)
        {
            GameObject newLoadFile = Instantiate(loadFile);

            GameObject nameTextGameObject = newLoadFile.transform.GetChild(0).gameObject;
            GameObject dateTextGameObject = newLoadFile.transform.GetChild(1).gameObject;

            Text nameText = nameTextGameObject.GetComponent<Text>();
            Text dateText = dateTextGameObject.GetComponent<Text>();

            nameText.text = FilesNames.files[i].name;
            dateText.text = FilesNames.files[i].date;

            newLoadFile.transform.SetParent(saveFiles.transform);
        }
    }

    public void AddFile()
    {
        if(inputText.text.Length > 0)
        {
            string date = DateTime.Now.ToString();
            GameObject newFile = Instantiate(loadFile);

            GameObject nameTextGameObject = newFile.transform.GetChild(0).gameObject;
            GameObject dateTextGameObject = newFile.transform.GetChild(1).gameObject;

            Text nameText = nameTextGameObject.GetComponent<Text>();
            Text dateText = dateTextGameObject.GetComponent<Text>();

            nameText.text = inputText.text;
            dateText.text = date;

            FilesNames.File file = new FilesNames.File(inputText.text, date);
            FilesNames.files.Add(file);

            newFile.transform.SetParent(saveFiles.transform);
        }
    }
}
