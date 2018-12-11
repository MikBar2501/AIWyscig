using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour {

    public Transform imagesHolder;
    public GameObject imageFilePref;
    public bool learn;

    public GameObject m1;
    public GameObject m2;

    public static Menu instanceMenu;

    public Button versus;
    public Button reset;

	// Use this for initialization
	void Start () {
        instanceMenu = this;
        File.WriteAllText("tescik.test", "ojejejje");
        LoadImages();

        if (!File.Exists(GenerationsManager.saveFileName))
        {
            reset.interactable = false;
            versus.interactable = false;
        }

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

    public void Mode(bool learn) {
        m1.SetActive(false);
        m2.SetActive(true);
        PlayerPrefs.SetInt("training", learn ? 1 : 0);
    }	

    public void ResetPrefs()
    {
        reset.interactable = false;
        versus.interactable = false;
        PlayerPrefs.DeleteAll();

        if (File.Exists(GenerationsManager.saveFileName))
        {
            File.Delete(GenerationsManager.saveFileName);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
