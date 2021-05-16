using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : BTNode
{
    float seconds;
    float count;
    public Wait(BehaviourTree bt, float seconds) : base(bt)
    {
        this.seconds = seconds; 
    }
    public override void OnInitialize()
    {
        count = seconds; 
    }
    public override Status Evaluate()
    {
        if (count <= 0)
        {
            Debug.Log("Returning success from wait");
            return Status.BH_SUCCESS;
        }
        else
        {
            count -= Time.deltaTime;
            return Status.BH_RUNNING;
        }
        //något har vi blivit fel
        return Status.BH_INVALID;
    }

}
