using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNGraphControl : MonoBehaviour {

    public static NNGraphControl main;

    public Material mat;
    public static Material staticMat;

    Node[][] nodes;

    public void SetNodeValue(int x, int y, float value)
    {
        value += 1;
        value /= 2;
        nodes[x][y].SetValue(value);
    }

    private void Awake()
    {
        main = this;
        staticMat = mat;
    }

    private void Start()
    {
        List<Node[]> nodesList = new List<Node[]>();

        for (int i = 0; i < GenerationsManager.main.layers.Length; i++) 
        {
            Transform nodesHolder = transform.GetChild(i).GetChild(0);

            Node[] nodes = nodesHolder.GetComponentsInChildren<Node>();

            nodesList.Add(nodes);
        }

        nodes = nodesList.ToArray();
    }

}
