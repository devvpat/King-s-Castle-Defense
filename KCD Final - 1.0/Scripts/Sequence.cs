//Tien-Yi Lee (thanks to Unity's tutorial https://learn.unity.com/project/behaviour-trees?uv=2019.4&courseId=5dd851beedbc2a1bf7b72bed)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childstatus = children[currentChild].Process();
        if (childstatus == Status.RUNNING) return Status.RUNNING;
        if (childstatus == Status.FAILURE)
            return childstatus;

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}
