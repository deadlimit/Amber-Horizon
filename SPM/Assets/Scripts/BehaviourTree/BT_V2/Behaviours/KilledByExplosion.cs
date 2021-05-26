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
        Debug.Log("KilledByExplosion----");

        //Vi vill egentligen returnera success när animationen är över el. efter en viss tid, 
        //just nu förstörs objektet direkt av Destroy-noden.
        if (timer <= 0)
            return Status.BH_SUCCESS;

        Debug.Log(timer);
       
        return Status.BH_RUNNING;
    }
}
