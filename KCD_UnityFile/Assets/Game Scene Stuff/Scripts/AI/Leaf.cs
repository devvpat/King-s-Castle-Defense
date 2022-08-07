//Tien-Yi Lee (thanks to Unity's tutorial https://learn.unity.com/project/behaviour-trees?uv=2019.4&courseId=5dd851beedbc2a1bf7b72bed)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick();
    public Tick ProcessMethod;

    public Leaf() { }

    public Leaf(string n, Tick pm)
    {
        name = n;
        ProcessMethod = pm;
    }

    public override Status Process()
    {
        if (ProcessMethod != null)
            return ProcessMethod();
        return Status.FAILURE;
    }

}
