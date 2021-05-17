using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    public class DataContainer<T>
    {
        private System.Type type;
        private T value;
        public DataContainer(System.Type type, T value)
        {
            this.type = type;
            this.value = value; 
        }
        public System.Type GetContainerType()
        {
            return type;
        }
        public T GetContainerValue()
        {
            return value;
        }
    }
    
    public Forager owner { get; private set; }
    public Transform ownerTransform { get; private set; }
    private BTNode m_root;
    [SerializeField] private LayerMask playerMask; 
    private bool startedBehaviour;
    private Coroutine behaviour;


    public Dictionary<string, object> blackboard = new Dictionary<string, object>();
   // public Dictionary<string, DataContainer<object>> blackboard = new Dictionary<string, DataContainer<object>>();
    private void Start()
    {
        Debug.Log("Behaviour tree skapat");
        this.owner = GetComponent<Forager>();
        ownerTransform = owner.transform;
        Debug.Log("owner : " + owner);
        m_root = BehaviourTreeBuilder();
        blackboard.Add("Target", new Vector3(29.96f, 0.1342f, 9.88f));
        blackboard.Add("TargetTransform", null);
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

        //
        Selector investigateSelector = new Selector(new List<BTNode>
                {
                investigateTarget,
                new AudioProximityCheck(this),
                }, this) ;

        //Selector Sub-Root Node
        Selector RootSelector = new Selector(new List<BTNode>
                {
                new VisualProximityCheck(this),
                investigateSelector,
                patrolSequence
                }, this);

       //Repeater Root Node
       return new Repeater(RootSelector, this);

        //Om vi saknar target, gör AudioProximityCheck
        //Finns just nu ingen riktig audio, så det är bara en mindre radie
       /* Filter ifTargetNullCheckAudible = new Filter(new List<BTNode>
                {
                 new Inverter(this, 
                    new AudioProximityCheck(this))
                }, this) ;
        ifTargetNullCheckAudible.AddCondition(new IsTargetNull(this));


        Selector alertSequence = new Selector(
            new List<BTNode>
                {
                   ifTargetNullCheckAudible,
                   new Investigate(this)
                }, this);*/


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
