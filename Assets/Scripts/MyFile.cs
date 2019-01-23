using System.IO;
using UnityEngine;

public class MyFile
{
    public void PublicWrite(string text, string fileName)
    {
        File.WriteAllText(Application.persistentDataPath + fileName, text);
        Debug.Log("Sucessful writing file");
    }

    public string Read(string fileName) => File.ReadAllText($@"/storage/emulated/0/temp/myFolder/{fileName}");
}