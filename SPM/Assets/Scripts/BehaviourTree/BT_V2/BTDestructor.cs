using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDestructor : BehaviourTree
{
    public Destructor destructor { get; private set; }
    private DestructorAttack destructorAttackNode; 
    private new void Start()
    {
        base.Start();
        destructor = (Destructor)owner;
        destructorAttackNode = new DestructorAttack(this);
        m_root = BehaviourTreeBuilder();
    }
    private void Update()
    {
        m_root.Tick();
    }
    private BTNode BehaviourTreeBuilder()
    {
        Debug.Log("Bygger BT");

        //Patrol----------------------------------------------------------------------
        Sequence patrolSequence = new Sequence(new List<BTNode>
            {
            new Patrol(this),
            new Wait(this, 4f)
            }, this, "patrolSequence", new IsTargetNull(this));


        //Investigate------------------------------------------------------------------
        Sequence investigateLastSeen = new Sequence(new List<BTNode>
            {
            new InvestigateLastSeen(this)
            }, this, "investigateLastSeen", new LastSeenPosition(this));

        Selector investigateSelector = new Selector(new List<BTNode>
            {
            investigateLastSeen,
            }, this, "investigateSelector");


        //Target Visible----------------------------------------------------------------------
        Sequence pointAndCharge = new Sequence(new List<BTNode>
            {
             new DestructorPoint(this),
             new ChargeTarget(this)
        }, this, "pointAndCharge", new DefaultCondition(this));

        Selector attack = new Selector(new List<BTNode>
            {
            destructorAttackNode,
            }, this, "Attack Selector", new TargetInRange(this));

        Selector targetVisible = new Selector(new List<BTNode>
             {
              //new AlertAllies(this),
              attack,
              pointAndCharge
             }, this, "targetVisible", new VisualProximityCheck(this));


        //Destructor Death--------------------------------------------------------------
        Selector causeOfDeath = new Selector(new List<BTNode>
            {
            new KilledByExplosion(this),
            }, this, "causeOfDeath");

        Sequence deathSequence = new Sequence(new List<BTNode>
            {
            causeOfDeath,
             new DestroyOwner(this)
            }, this, "deathSequence", new AIDied(this));

        //Selector Sub-Root Node--------------------------------------------------------
        Selector RootSelector = new Selector(new List<BTNode>
                {
                deathSequence,
                targetVisible,
                investigateSelector,
                patrolSequence
                }, this, "RootSelector");

        //Parallel for independent execution of timernode-----------------------------------
        timerNode = new TimerNode(this);
        Parallel rootParallel = new Parallel(new List<BTNode>
            {
            timerNode,
            RootSelector
            }, this, "rootParallel");
       
        //Repeater Root Node
        return new Repeater(RootSelector, this);
    }

    //Called by animation event in Destructor melee anim
    public void AttackAnimationFinished()
    {
        destructorAttackNode.AttackAnimationFinished();
    }
}
