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
                new AudioProximityCheck(this),
                }, this) ;


        //Om shoot != on cooldown && in range, shoot
        Filter shootSequence = new Filter(new List<BTNode>
        {
           new Shoot(this)
        }, this);
        shootSequence.AddCondition(new ShootOnCooldown(this));
        shootSequence.AddCondition(new Inverter(new TargetInRange(this), this));


        //Target in range behöver ha barn? för att sedan i den hierarkin skapa en shootsequence
        Filter targetVisible = new Filter(new List<BTNode>
             {
              shootSequence,
              new MoveToTarget(this)
             }, this) ;
        targetVisible.AddCondition(new VisualProximityCheck(this));




        //Selector Sub-Root Node
        Selector RootSelector = new Selector(new List<BTNode>
                {
                targetVisible,
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
