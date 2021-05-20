using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerNode : BTNode
{
    private float fireCooldown;
    public TimerNode(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        if(fireCooldown > 0)
            fireCooldown -= Time.deltaTime;

        return Status.BH_SUCCESS;
    }

    public void SetFireCooldown(float val) {  fireCooldown = val; }
    public float GetFireCooldown() {return fireCooldown ; }
}
