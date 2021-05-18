using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : BTNode
{
    public Shoot(BehaviourTree bt) : base(bt)
    {

    }
    public override Status Evaluate()
    {
        //Reset path så att agenten stannar när den ska skjuta
        Debug.Log("Shoot!!");
        bt.owner.Pathfinder.agent.ResetPath();
        bt.owner.Fire(); 
        return Status.BH_FAILURE;
    }
}
