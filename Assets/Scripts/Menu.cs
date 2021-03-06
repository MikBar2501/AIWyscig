﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour {

    public Transform imagesHolder;
    public GameObject imageFilePref;

    public Button reset;
    public Button versus;

	// Use this for initialization
	void Start () {
        //File.WriteAllText("tescik.test", "ojejejje");
        LoadImages();
        bool save = GenerationsManager.IsThereBrainSaved();
        reset.interactable = save;
        versus.interactable = save;
    }

    public void LoadImages()
    {
        List<ImageFile> imageFiles = GetComponent<ImageLoader>().LoadImages();

        foreach(ImageFile file in imageFiles)
        {
            GameObject imgObj = Instantiate(imageFilePref, imagesHolder);
            imgObj.GetComponent<RawImage>().texture = file.image;
            imgObj.transform.GetChild(0).GetComponent<Text>().text = file.name;
            imgObj.GetComponent<SaveImagePath>().imgPath = file.fullPath;
        }
    }

    public void SetTraining(bool training)
    {
        PlayerPrefs.SetInt("training", training ? 1 : 0);
    }
	
}
