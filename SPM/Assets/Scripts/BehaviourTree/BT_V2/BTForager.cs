using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTForager : BehaviourTree
{
    private Teleport teleportNode;
    public Forager forager { get; private set; }

    private void Start()
    {
        base.Start();
        forager = (Forager)owner;

        //Forager specific values
        blackboard.Add("FleeDistance", new DataContainer<float>(forager.FleeDistance));
        blackboard.Add("FireCooldown", new DataContainer<float>(forager.FireCooldown));

        m_root = BehaviourTreeBuilder();
    }

    void Update()
    {
        m_root.Tick();
    }

    private BTNode BehaviourTreeBuilder()
    {
        Debug.Log("Bygger BT");

        //Patrol-------------------------------------------------------------------------
        Sequence patrolSequence = new Sequence(new List<BTNode>
            {
            new Patrol(this),
            new Wait(this, maxWaitTime)
            }, this, "patrolSequence", new IsTargetNull(this));


        //Investigate------------------------------------------------------------------
        Sequence investigateTarget = new Sequence(new List<BTNode>
            {
            new Investigate(this)
            }, this, "investigateTarget", new TargetNotNull(this));

        Sequence investigateLastSeen = new Sequence(new List<BTNode>
            {
            new InvestigateLastSeen(this)
            }, this, "investigateLastSeen", new LastSeenPosition(this));

        Selector investigateSelector = new Selector(new List<BTNode>
            {
            investigateLastSeen,
            investigateTarget,
            new AudioProximityCheck(this),
            }, this, "investigateSelector");

        //Flee-------------------------------------------------------------------------
        //Named teleport for calling a method on it from Animation event
        teleportNode = new Teleport(this);
        Sequence fleeSequence = new Sequence(new List<BTNode>
            {
             teleportNode
            }, this, "fleeFilter", new TargetTooClose(this));

        //Target visible/in range------------------------------------------------------
        Selector targetInRange = new Selector(new List<BTNode>
            {
            new Shoot(this),
            new Reposition(this)
            }, this, "shootOrReposition", new TargetInRange(this));

        Selector targetVisible = new Selector(new List<BTNode>
             {
              new AlertAllies(this),
              fleeSequence,
              targetInRange,
              new MoveToTarget(this)
             }, this, "targetVisible", new VisualProximityCheck(this));

        //AI Death---------------------------------------------------------------------------
        Selector causeOfDeath = new Selector(new List<BTNode>
            {
            new KilledByBlackHole(this),
            new KilledByExplosion(this),
            }, this, "causeOfDeath");

        Sequence deathSequence = new Sequence(new List<BTNode>
            {
            causeOfDeath,
             new DestroyOwner(this)
            }, this, "deathSequence", new AIDied(this));

        //Root Selector--------------------------------------------------------------------
        Selector RootSelector = new Selector(new List<BTNode>
                {
                deathSequence,
                targetVisible,
                investigateSelector,
                patrolSequence
                }, this, "RootSelector");

        //Parallel node for independent timer execution-------------------------------
        timerNode = new TimerNode(this);
        Parallel rootParallel = new Parallel(new List<BTNode>
            {
            timerNode,
            RootSelector
            }, this, "rootParallel");
       
        //Repeater Root Node----------------------------------------------------------
        return new Repeater(rootParallel, this);

        //TEST SEQUENCE-------------------------------------------------
        /*Filter testFilter = new Filter(new List<BTNode>
            {
            new STest1(this)
            }, this);
        testFilter.AddCondition(new TargetNotNull(this));

        Selector testSelector = new Selector(new List<BTNode>
            {
            testFilter,
            new STest2(this),
            new STest3(this)
            }, this);
        return new Repeater(testSelector, this);*/
        //-------------------------------------------------------------------
    }

    //Called by animation event
    public void TeleportFinished()
    {
        teleportNode.ExecuteTeleport();
    }
}
