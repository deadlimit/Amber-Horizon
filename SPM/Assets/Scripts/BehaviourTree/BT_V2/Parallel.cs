using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Composite
{
    public Parallel(List<BTNode> children, BehaviourTree bt) : base(children, bt) {    }
    public Parallel(List<BTNode> children, BehaviourTree bt, string name) : base(children, bt, name) { }

    //gameaipro-artikeln har en mycket mer ingående använding av paralleler, men för tillfället behöver jag bara
    //möjlighet att köra två subträd samtidigt
    public override Status Evaluate()
    {
        //jag vet att denna kommer vara ett direkt barn till repeater, alltså spelar returvärdet just nu ingen roll
        foreach(BTNode child in children)
        {
            child.Tick();
        }

        return Status.BH_SUCCESS;
    }
}
