using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveImagePath : MonoBehaviour {

    [HideInInspector]
    public string imgPath;

	public void Save()
    {
        PlayerPrefs.SetString("img_path", imgPath);
    }
}
