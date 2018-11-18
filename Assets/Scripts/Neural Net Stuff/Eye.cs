using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour {

    LineRenderer line;

    Vector3[] posintions;

    public float distance = 3;
    float currentDistance = 0;

    Color color;

    // Use this for initialization
    void Start () {
        line = GetComponentInChildren<LineRenderer>();

        color = transform.parent.parent.GetComponent<SpriteRenderer>().color;

        posintions = new Vector3[2];
        line.SetVertexCount(2);

        line.endColor = color;
        line.startColor = color;
    }

    public float GetDistanceNormalized()
    {
        return currentDistance / distance;
    }
	
	// Update is called once per frame
	void Update () {

        Ray2D ray = new Ray2D(transform.position, transform.up);
        RaycastHit2D hit;
        hit = Physics2D.Raycast(ray.origin, ray.direction, distance, 1 << 0);

        posintions[0] = ray.origin;

        if (hit.collider != null)
        {
            posintions[1] =  hit.point;
        }
        else
        {
            posintions[1] = ray.origin + ray.direction * distance;
        }

        currentDistance = Vector2.Distance(posintions[0], posintions[1]);

        float normDist = GetDistanceNormalized();

        line.startColor = normDist * color + (1 - normDist) * Color.red;
        line.endColor = line.startColor;

        line.SetPositions(posintions);
    }
}
