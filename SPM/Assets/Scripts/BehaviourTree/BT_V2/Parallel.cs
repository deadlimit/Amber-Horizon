using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Composite
{
    public Parallel(List<BTNode> children, BehaviourTree bt) : base(children, bt) {    }
    public Parallel(List<BTNode> children, BehaviourTree bt, string name) : base(children, bt, name) { }

    //gameaipro-artikeln har en mycket mer ing�ende anv�nding av paralleler, men f�r tillf�llet beh�ver jag bara
    //m�jlighet att k�ra tv� subtr�d samtidigt
    public override Status Evaluate()
    {
        //jag vet att denna kommer vara ett direkt barn till repeater, allts� spelar returv�rdet just nu ingen roll
        foreach(BTNode child in children)
        {
            child.Tick();
        }

        return Status.BH_SUCCESS;
    }
}
