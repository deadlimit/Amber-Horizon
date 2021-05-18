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
    //Det h�r kr�ver h�rdkodning i kastning n�r man h�mtar v�rden
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

        //Om target �r null ska vi inte unders�ka, d�rf�r inverter i condition
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

        //Hade kanske egentligen velat ha en selector med filter h�r, alternativet till det kanske 
        //�r att s�tta en succeeder p� shootSequence, och sedan utv�dera avst�ndet inuti MoveToTarget, men d� g�r vi det tv� g�nger ist�llet, det blir dumt.
        //Annars f�r det blir en f�r�lder till targetvisible som �r ett filter, och sedan �r targetVisible sj�lv en selector
        Selector targetVisible = new Selector(new List<BTNode>
             {
              shootSequence,
              new MoveToTarget(this)
             }, this);
       
        //Filter innan targetVisible s� att vi kan g�ra targetVisible til en Selector
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
