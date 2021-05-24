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
        blackboard.Add("Range", new DataContainer<float>(forager.range));
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

        //NoTargetFilter, Patrol & Wait
        Sequence patrolSequence = new Sequence(new List<BTNode>
            {
            new Patrol(this),
            new Wait(this, 4f)
            }, this, "patrolSequence", new IsTargetNull(this));

        //Condition hindrar Investigate från att printa sin OnInit, och if:s 
        //faller igenom och returnerar running till top-branch, det är det som spökar
        Sequence investigateTarget = new Sequence(new List<BTNode>
            {
            new Investigate(this)
            }, this, "investigateTarget", new TargetNotNull(this));


        //InvestigateLastSeen with filter
        Sequence investigateLastSeen = new Sequence(new List<BTNode>
            {
            new InvestigateLastSeen(this)
            }, this, "investigateLastSeen", new LastSeenPosition(this));

        //Investigate & AudioCheck
        Selector investigateSelector = new Selector(new List<BTNode>
            {
            investigateLastSeen,
            investigateTarget,
            new AudioProximityCheck(this),
            }, this, "investigateSelector");

        //Named teleport for calling a method on it from Animation event
        teleportNode = new Teleport(this);
        Sequence fleeSequence = new Sequence(new List<BTNode>
            {
             teleportNode
            }, this, "fleeFilter", new TargetTooClose(this));


        Selector targetInRange = new Selector(new List<BTNode>
            {
            new Shoot(this),
            new Reposition(this)
            }, this, "shootOrReposition", new TargetInRange(this));


        //Hade kanske egentligen velat ha en selector med filter här, alternativet till det kanske 
        //är att sätta en succeeder på shootSequence, och sedan utvädera avståndet inuti MoveToTarget, men då gör vi det två gånger istället, det blir dumt.
        //Annars får det blir en förälder till targetvisible som är ett filter, och sedan är targetVisible själv en selector
        Selector targetVisible = new Selector(new List<BTNode>
             {
              new AlertAllies(this),
              fleeSequence,
              targetInRange,
              new MoveToTarget(this)
             }, this, "targetVisible", new VisualProximityCheck(this));

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

        //Selector Sub-Root Node
        Selector RootSelector = new Selector(new List<BTNode>
                {
                deathSequence,
                targetVisible,
                investigateSelector,
                patrolSequence
                }, this, "RootSelector");

        timerNode = new TimerNode(this);

        Parallel rootParallel = new Parallel(new List<BTNode>
            {
            timerNode,
            RootSelector
            }, this, "rootParallel");
        //Repeater Root Node
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
