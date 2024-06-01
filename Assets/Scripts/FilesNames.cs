using System.Collections.Generic;
using UnityEngine;

public class FilesNames : MonoBehaviour
{
    public struct File
    {
        public string name;
        public string date;

        public File(string fileName, string fileDate)
        {
            name = fileName;
            date = fileDate;
        }
    }

    public static List<File> files = new List<File>
        {
            new File("New file", "16.05.2024 21:03:15"),
            new File("Test save", "20.05.2024 14:22:46")
        };
}
