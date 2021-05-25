using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerNode : BTNode
{
    private float attackCooldown;
    public TimerNode(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        if(attackCooldown > 0)
            attackCooldown -= Time.deltaTime;

        return Status.BH_SUCCESS;
    }

    public void SetAttackCooldown(float val) {  attackCooldown = val; }
    public float GetAttackCooldown() {return attackCooldown ; }
}
