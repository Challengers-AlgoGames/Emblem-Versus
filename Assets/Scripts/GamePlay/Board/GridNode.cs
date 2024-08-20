using UnityEngine;
using System.Collections.Generic;

public class NodeBase : MonoBehaviour
{
    public Vector3Int GridPosition;
    public bool Walkable = true;
    public NodeBase Connection;
    public float G;
    public float H;
    public float F => G + H;
    public List<NodeBase> Neighbors = new List<NodeBase>();

    public void SetG(float value) => G = value;
    public void SetH(float value) => H = value;
    public void SetConnection(NodeBase node) => Connection = node;

    public float GetDistance(NodeBase node)
    {
        return Vector3Int.Distance(GridPosition, node.GridPosition);
    }

    public void SetColor(Color color)
    {
        // Assuming a method to set node color for debugging purposes
        GetComponent<Renderer>().material.color = color;
    }
}
