﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveImagePath : MonoBehaviour {

    [HideInInspector]
    public string imgPath;

	public void Save()
    {
        PlayerPrefs.SetString("img_path", imgPath);
        if(Menu.instanceMenu.learn == true) {
            SceneManager.LoadScene("Training");
        } else {
            SceneManager.LoadScene("Versus");
        }
        
    }


}
