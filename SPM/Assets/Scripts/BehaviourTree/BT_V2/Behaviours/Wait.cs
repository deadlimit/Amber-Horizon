using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : BTNode
{
    float maxWaitTime;
    float seconds;
    public Wait(BehaviourTree bt, float maxWaitTime) : base(bt)
    {
        this.maxWaitTime = maxWaitTime; 
    }
    public override void OnInitialize()
    {
        seconds = Random.Range(1.5f,  maxWaitTime); 
    }
    public override Status Evaluate()
    {
        if (seconds <= 0)
        {
            return Status.BH_SUCCESS;
        }
        else
        {
            seconds -= Time.deltaTime;
            return Status.BH_RUNNING;
        }
    }

}
