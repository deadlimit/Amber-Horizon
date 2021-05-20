using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourTree : MonoBehaviour
{
    public class DataContainer { }
    public class DataContainer<T> : DataContainer
    {
        private T value;
        public DataContainer(T value)
        {
            this.value = value; 
        }

        public T GetValue()
        {
            return value;
        }
        public void SetValue(T newVal)
        {
            value = newVal;
        }
    }
    
    public Vector3 startPos { get; private set; }
    public Forager owner { get; private set; }
    public NavMeshAgent ownerAgent { get; private set; }
    public Transform ownerTransform { get; private set; }
    public TimerNode timerNode { get; private set; }
   
    private BTNode m_root;
    [SerializeField] private LayerMask playerMask; 


    //public Dictionary<string, object> blackboard = new Dictionary<string, object>();
    public Dictionary<string, DataContainer> blackboard = new Dictionary<string, DataContainer>();
    //Det h�r kr�ver h�rdkodning i kastning n�r man h�mtar v�rden
    public DataContainer<T> GetBlackBoardValue<T>(string blackboardKey)
    {
        return (DataContainer<T>)blackboard[blackboardKey];
    }

    private void Start()
    {
        this.owner = GetComponent<Forager>();
        ownerTransform = owner.transform;
        ownerAgent = owner.Pathfinder.agent;
        startPos = ownerTransform.position;
        m_root = BehaviourTreeBuilder();
        blackboard.Add("Target", new DataContainer<Vector3>(new Vector3(29.96f, 0.1342f, 9.88f)));
        blackboard.Add("TargetTransform", new DataContainer<Transform>(null));
        blackboard.Add("LastSeenPosition", new DataContainer<Vector3>(Vector3.zero));
    }
    private void Update()
    {
        m_root.Tick();
    }
    private BTNode BehaviourTreeBuilder()
    {
        Debug.Log("Bygger BT");
        //NoTargetFilter, Patrol & Wait
        Filter patrolSequence = new Filter(new List<BTNode>
            {
            new Patrol(owner, this),
            new Wait(this, 4f)
            }, this, "patrolSequence");
        patrolSequence.AddCondition(new IsTargetNull(this));

        //Condition hindrar Investigate fr�n att printa sin OnInit, och if:s 
        //faller igenom och returnerar running till top-branch, det �r det som sp�kar
        Filter investigateTarget = new Filter(new List<BTNode>
            {
            new Investigate(this)
            }, this, "investigateTarget");
        investigateTarget.AddCondition(new TargetNotNull(this));


        //InvestigateLastSeen with filter
        Filter investigateLastSeen = new Filter(new List<BTNode>
            {
            new InvestigateLastSeen(this)
            }, this, "investigateLastSeen");
        investigateLastSeen.AddCondition(new LastSeenPosition(this));

        //Investigate & AudioCheck
        Selector investigateSelector = new Selector(new List<BTNode>
            {
            investigateLastSeen,
            investigateTarget,
            new AudioProximityCheck(this),
            }, this, "investigateSelector") ;


        Selector targetInRange = new Selector(new List<BTNode>
        {
            new Shoot(this),
            new Reposition(this)
        }, this, "shootOrReposition", new TargetInRange(this));


        //Hade kanske egentligen velat ha en selector med filter h�r, alternativet till det kanske 
        //�r att s�tta en succeeder p� shootSequence, och sedan utv�dera avst�ndet inuti MoveToTarget, men d� g�r vi det tv� g�nger ist�llet, det blir dumt.
        //Annars f�r det blir en f�r�lder till targetvisible som �r ett filter, och sedan �r targetVisible sj�lv en selector
        Selector targetVisible = new Selector(new List<BTNode>
             {
              targetInRange,
              new MoveToTarget(this)
             }, this, "targetVisible", new VisualProximityCheck(this));      


        //Selector Sub-Root Node
        Selector RootSelector = new Selector(new List<BTNode>
                {
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

    public LayerMask GetPlayerMask()
    {
        return playerMask;
    }
}
