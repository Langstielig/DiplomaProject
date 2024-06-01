using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadWindow : MonoBehaviour
{
    [SerializeField] private GameObject loadFile;
    [SerializeField] private GameObject loadFiles;

    private int countOfFiles = 0;

    private void Start()
    {
        for(int i = 0; i < FilesNames.files.Count; i++)
        {
            AddFile(i);
        }   
    }

    private void Update()
    {
        if(countOfFiles != FilesNames.files.Count)
        {
            AddFile(countOfFiles);
        }
    }

    private void AddFile(int index)
    {
        GameObject newLoadFile = Instantiate(loadFile);

        GameObject nameTextGameObject = newLoadFile.transform.GetChild(0).gameObject;
        GameObject dateTextGameObject = newLoadFile.transform.GetChild(1).gameObject;

        Text nameText = nameTextGameObject.GetComponent<Text>();
        Text dateText = dateTextGameObject.GetComponent<Text>();

        nameText.text = FilesNames.files[index].name;
        dateText.text = FilesNames.files[index].date;

        newLoadFile.transform.SetParent(loadFiles.transform);

        countOfFiles++;
    }
}
