using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour {

    [Range(0.0f, 180)]
    public float maxAngle;
    public int eyesCount;
    public GameObject eyePref;
    // Use this for initialization
    private void Awake()
    {
        float startAngle = -maxAngle;
        float endAngleOffset = 2 * maxAngle;

        for(int i = 0; i < eyesCount; i++)
        {
            GameObject eye = Instantiate(eyePref, transform);
            eye.transform.eulerAngles = new Vector3(0,0, startAngle + (i / (float)(eyesCount - 1) * endAngleOffset));
            eye.transform.position = transform.position;
        }
    }

    public float[] GetSight()
    {
        float[] sight = new float[transform.childCount];

        int iterator = 0; 
        foreach(Transform eye in transform)
        {
            sight[iterator] = eye.GetComponent<Eye>().GetDistanceNormalized();
            iterator++;
        }

        return sight;
    }
}
