using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : Control {

    public Slider slider;
    public Text sliderValue;

    public bool floatDisplay = false;

    public override float GetValue()
    {
        return slider.value;
    }

    public void SetMinMax(float min, float max)
    {
        slider.minValue = min;
        slider.maxValue = max;
    }

    private void Update()
    {
        if(!floatDisplay)
            sliderValue.text = (int)slider.value + "";
        else
            sliderValue.text = ((float)((int)(slider.value * 10)))/10 + "";
    }

    public override void SetBaseValue(float value)
    {
        slider.value = value;
    }

}
