using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTForager : BehaviourTree
{
    public Forager forager { get; private set; }
    private Teleport teleportNode;
    private Shoot shootNode;

    private new void Start()
    {
        base.Start();
        forager = (Forager)owner;

        //Forager specific values
        blackboard.Add("FleeDistance", new DataContainer<float>(forager.FleeDistance));
        blackboard.Add("FireCooldown", new DataContainer<float>(forager.FireCooldown));

        m_root = BehaviourTreeBuilder();
    }
    private void Update()
    {
        m_root.Tick();
    }

    private BTNode BehaviourTreeBuilder()
    {
        //Patrol-------------------------------------------------------------------------
        Sequence patrolSequence = new Sequence(new List<BTNode>
            {
            new Patrol(this),
            new Wait(this, maxWaitTime)
            }, this, "patrolSequence", new TargetIsNull(this));


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
        shootNode = new Shoot(this);
        Selector targetInRange = new Selector(new List<BTNode>
            {
            shootNode,
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
        Selector treeRoot = new Selector(new List<BTNode>
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
            treeRoot
            }, this, "rootParallel");

        //Repeater Root Node----------------------------------------------------------
        return new Repeater(rootParallel, this);

    }

    //Called by animation event
    public void TeleportFinished()
    {
        teleportNode.ExecuteTeleport();
    }

    public void ShootAnimationFinished()
    {
        shootNode.ShootAnimationFinished();
    }
}
