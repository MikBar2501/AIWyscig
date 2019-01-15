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
    public GameObject m3;

    public static Menu instanceMenu;

    public Button versus;
    public Button reset;

    public Image colorPanel;
    public Image colorPanel2;
    public List<Color> panelColors;
    int colorIterator;

    [Header("Slider")]
    public SliderControl sliderCount;
    public SliderControl sliderKeepPrec;
    public SliderControl sliderHard;
    public SliderControl sliderSoft;
    public SliderControl sliderRange;

    // Use this for initialization
    void Start () {
        instanceMenu = this;
        //File.WriteAllText("tescik.test", "ojejejje");
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

    public void OpenNetSettings()
    {
        m1.SetActive(false);
        m2.SetActive(false);
        m3.SetActive(true);

        colorIterator = 0;

        colorPanel.color = panelColors[0];
        colorPanel2.color = panelColors[0];

        GenerationsManager.generationsSettings = new List<GenerationSettings>();
        GenerationsManager.generationsSettings.Add(new GenerationSettings());
        GenerationsManager.generationsSettings.Add(new GenerationSettings());
        GenerationsManager.generationsSettings.Add(new GenerationSettings());
    }

    public void Back()
    {
        if(colorIterator == 0)
        {
            m1.SetActive(false);
            m2.SetActive(true);
            m3.SetActive(false);
            return;
        }

        colorIterator--;
        colorPanel.color = panelColors[colorIterator];
        colorPanel2.color = panelColors[colorIterator];
    }

    public void Apply()
    {
        

        int startID = 0;
        if(colorIterator > 0)
        {
            startID = GenerationsManager.generationsSettings[colorIterator - 1].endIndex + 1;
        }

        int count = (int)sliderCount.GetValue();

        GenerationSettings settings = new GenerationSettings
        {
            ID = colorIterator,
            keepPerc = sliderKeepPrec.GetValue() / 100,
            count = count,
            color = panelColors[colorIterator],
            startIndex = startID,
            endIndex = startID + count - 1,
            hardMutationChanse = (int)sliderHard.GetValue(),
            softMutationChanse = (int)sliderSoft.GetValue(),
            rangeOfMutation = sliderRange.GetValue()
        };
        GenerationsManager.generationsSettings[colorIterator] = settings;
        //PlayerPrefs.SetInt(colorIterator + "count", 5);


        if (colorIterator >= panelColors.Count - 1)
        {
            Application.LoadLevel(1);
            return;
        }

        colorIterator++;
        colorPanel.color = panelColors[colorIterator];
        colorPanel2.color = panelColors[colorIterator];
    }
}
