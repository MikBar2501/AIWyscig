using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Edge
{
    public Node node;
    public LineRenderer line;
}

public class Node : MonoBehaviour {

    public Transform nextNodesHolder; //zapisz gdzies public mat zmien go na static
    List<Edge> edges;

    public string moreString;
    public string lessString;

    private void Awake()
    {
        edges = new List<Edge>();
    }

    // Use this for initialization
    void Start () {
        if(nextNodesHolder)
		foreach(Node node in nextNodesHolder.GetComponentsInChildren<Node>())
        {
            ConnectTo(node);
        }
	}

    void ConnectTo(Node node)
    {
        LineRenderer line = CreateConnectionLine();
        edges.Add(new Edge { node = node, line = line });
        line.SetPositions(new Vector3[] { transform.position, node.transform.position });
    }

    LineRenderer CreateConnectionLine()
    {
        GameObject line = new GameObject();
        line.transform.parent = transform;
        LineRenderer lineRend = line.AddComponent<LineRenderer>();

        lineRend.material = NNGraphControl.staticMat;
        line.layer = 9;
        lineRend.SetColors(Color.green, Color.green);
        lineRend.SetWidth(0.1f, 0.1f);

        return lineRend;
    }

    public void SetValue(float value)
    {
        Color color = value * Color.green + (1 - value) * Color.red;
        GetComponent<Image>().color = color;

        foreach(Edge edge in edges)
        {
            edge.line.SetColors(color, color);
        }

        Text text = transform.GetComponentInChildren<Text>();
        if(text)
            text.text = value > 0.5f ? moreString : lessString;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
