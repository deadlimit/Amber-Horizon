using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform ownerTransform { get; private set; }
    private BTNode m_root;
    [SerializeField] private LayerMask playerMask; 
    private bool startedBehaviour;
    private Coroutine behaviour;


    //public Dictionary<string, object> blackboard = new Dictionary<string, object>();
    public Dictionary<string, DataContainer> blackboard = new Dictionary<string, DataContainer>();
    //Det här kräver hårdkodning i kastning när man hämtar värden
    public DataContainer<T> GetBlackBoardValue<T>(string blackboardKey)
    {
        return (DataContainer<T>)blackboard[blackboardKey];
    }

    private void Start()
    {
        this.owner = GetComponent<Forager>();
        ownerTransform = owner.transform;
        startPos = ownerTransform.position;
        m_root = BehaviourTreeBuilder();
        blackboard.Add("Target", new DataContainer<Vector3>(new Vector3(29.96f, 0.1342f, 9.88f)));
        blackboard.Add("TargetTransform", new DataContainer<Transform>(null));
        blackboard.Add("LastSeenPosition", new DataContainer<Vector3>(Vector3.zero));
    }
    private void Update()
    {
        if(!startedBehaviour)
        {
            behaviour = StartCoroutine(RunBehaviour());
            startedBehaviour = true;
        }
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
                }, this);
        patrolSequence.AddCondition(new IsTargetNull(this));

        //Om target är null ska vi inte undersöka, därför inverter i condition
        Filter investigateTarget = new Filter(new List<BTNode>
                {
                new Investigate(this)
                }, this);
        investigateTarget.AddCondition(new TargetNotNull(this));

        

        //Investigate & AudioCheck
        Selector investigateSelector = new Selector(new List<BTNode>
                {
                investigateTarget,
                new InvestigateLastSeen(this),
                new AudioProximityCheck(this),
                }, this) ;


        Sequence shootSequence = new Sequence(new List<BTNode>
            {
            new TargetInRange(this),
            new Shoot(this)
            }, this) ;

        //Hade kanske egentligen velat ha en selector med filter här, alternativet till det kanske 
        //är att sätta en succeeder på shootSequence, och sedan utvädera avståndet inuti MoveToTarget, men då gör vi det två gånger istället, det blir dumt.
        //Annars får det blir en förälder till targetvisible som är ett filter, och sedan är targetVisible själv en selector
        Selector targetVisible = new Selector(new List<BTNode>
             {
              shootSequence,
              new MoveToTarget(this)
             }, this);
       
        //Filter innan targetVisible så att vi kan göra targetVisible til en Selector
        Filter visualCheckFilter = new Filter(new List<BTNode>
            {
            targetVisible
            }, this);
        visualCheckFilter.AddCondition(new VisualProximityCheck(this));


        //Selector Sub-Root Node
        Selector RootSelector = new Selector(new List<BTNode>
                {
                visualCheckFilter,
                investigateSelector,
                patrolSequence
                }, this);

       //Repeater Root Node
       return new Repeater(RootSelector, this);
    }
    private IEnumerator RunBehaviour()
    {
        Status status = m_root.Evaluate();
        while(status == Status.BH_RUNNING)
        {
            yield return null;
            status = m_root.Evaluate();
        }

        Debug.Log("Behaviour has finished with: " + status);
    }
    public LayerMask GetPlayerMask()
    {
        return playerMask;
    }
}
