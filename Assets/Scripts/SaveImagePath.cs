using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveImagePath : MonoBehaviour {

    [HideInInspector]
    public string imgPath;

	public void Save()
    {

        string fullPath ="";

        if(Application.isEditor)
        {
            fullPath = imgPath;
        }
        else
        {
            fullPath = ImageLoader.folderPath + "/" + imgPath;
        }
        PlayerPrefs.SetString("img_path", fullPath);
        if(PlayerPrefs.GetInt("training") == 1)
        {
            Menu.instanceMenu.OpenNetSettings();
        }
        else
        {
            Application.LoadLevel(1);
        }
    }
      


}
