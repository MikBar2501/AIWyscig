using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public struct ImageFile
{
    public string name;
    public Texture2D image;
    public string fullPath;
}

public class ImageLoader : MonoBehaviour {

    public string folderName;
    public Text testText;

    // Use this for initialization
    void Start () {
        //string txtName = "smile.png";  // your filename
        //Vector2 size = new Vector2(256, 256); // image size

        //Texture2D texture = LoadImage(size, Path.GetFullPath(txtName));
    }

    public List<ImageFile> LoadImages()
    {
        List<ImageFile> imageFiles = new List<ImageFile>();

        string dataPath = Application.dataPath;
        string folderPath = JumpBackOneFolder(dataPath) + "/" + folderName;

        testText.text = "";

        DirectoryInfo dir = new DirectoryInfo(folderPath);
        FileInfo[] info = dir.GetFiles("*.*");

        foreach (FileInfo f in info)
        {
            string name = f.ToString();
            //testText.text = folderPath + "/" + name;
            //Texture2D image = LoadImage(new Vector2(512,512), folderPath + "/" + name); //FOR BUILD
            Texture2D image = LoadImage(new Vector2(512, 512), name); //FOR EDITOR
            imageFiles.Add(new ImageFile { name = name, image = image, fullPath = name });
            //imageFiles.Add(new ImageFile { name = name, image = image, fullPath = folderPath + "/" + name });
        }

        return imageFiles;
    }

    public static string JumpBackOneFolder(string path)
    {
        string newPath = "";

        int indexOfSlash = 0;
        for(int i = path.Length - 1; i>=0; i--)
        {
            char curChar = path[i];
            //print(curChar);
            if (curChar == '/')
            {
                indexOfSlash = i;
                break;
            }
        }

        for (int i = 0; i < indexOfSlash; i++)
            newPath += path[i];

        return newPath;
    }

    private static Texture2D LoadImage(Vector2 size, string filePath)
    {

        byte[] bytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Trilinear;
        texture.LoadImage(bytes);
        texture.Apply();

        return texture;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
