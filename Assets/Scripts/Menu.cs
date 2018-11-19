﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour {

    public Transform imagesHolder;
    public GameObject imageFilePref;

	// Use this for initialization
	void Start () {
        File.WriteAllText("tescik.test", "ojejejje");

        LoadImages();
	}

    public void LoadImages()
    {
        List<ImageFile> imageFiles = GetComponent<ImageLoader>().LoadImages();

        foreach(ImageFile file in imageFiles)
        {
            GameObject imgObj = Instantiate(imageFilePref, imagesHolder);
            imgObj.GetComponent<RawImage>().texture = file.image;
            imgObj.transform.GetChild(0).GetComponent<Text>().text = file.name;
        }
    }
	
}