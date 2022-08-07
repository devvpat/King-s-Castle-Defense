//Tien-Yi Lee (thanks to Unity's tutorial https://learn.unity.com/project/behaviour-trees?uv=2019.4&courseId=5dd851beedbc2a1bf7b72bed)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status { SUCCESS, RUNNING, FAILURE };
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;

    public Node() { }

    public Node(string n)
    {
        name = n;
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }
}