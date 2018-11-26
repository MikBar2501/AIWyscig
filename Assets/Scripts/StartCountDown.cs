using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCountDown : MonoBehaviour {

    public Text text;

    public static StartCountDown main;

    public int counter = 3;

    private void Awake()
    {
        main = this;
    }

    public void StartCount()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        text.gameObject.SetActive(true);
        text.text = counter + "";

        while(counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
            text.text = counter + "";
        }
        text.text = "GO!";
        GenerationsManager.main.ActivateCars();
        yield return new WaitForSeconds(0.3f);
        text.text = "";
    }
}
