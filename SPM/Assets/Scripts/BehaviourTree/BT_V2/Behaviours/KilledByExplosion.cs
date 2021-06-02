using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledByExplosion : BTNode
{
    private float timer = 3f;
    public KilledByExplosion(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        timer -= Time.deltaTime;
        //bt.ownerAgent.ResetPath();
        //forager.Pathfinder.agent.isStopped = true;
        bt.ownerAgent.enabled = false;
        //Object.Destroy(bt.owner.gameObject); 

        //Vi vill egentligen returnera success n�r animationen �r �ver el. efter en viss tid, 
        //just nu f�rst�rs objektet direkt av Destroy-noden.
        if (timer <= 0)
            return Status.BH_SUCCESS;

       
        return Status.BH_RUNNING;
    }
}
