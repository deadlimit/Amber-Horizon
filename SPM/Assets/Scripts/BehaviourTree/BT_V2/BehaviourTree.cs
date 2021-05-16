using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    public Forager owner { get; private set; }
    public Transform ownerTransform { get; private set; }
    private BTNode m_root;
    [SerializeField] private LayerMask playerMask; 
    private bool startedBehaviour;
    private Coroutine behaviour;
    

    public Dictionary<string, object> blackboard = new Dictionary<string, object>();

    private void Start()
    {
        Debug.Log("Behaviour tree skapat");
        this.owner = GetComponent<Forager>();
        ownerTransform = owner.transform;
        Debug.Log("owner : " + owner);
        m_root = BehaviourTreeBuilder();
        blackboard.Add("Target", new Vector3(29.96f, 0.1342f, 9.88f));
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



        Filter investigateSequence = new Filter(
            new List<BTNode>
                {
                   new Investigate(this)
                }, 
            this);
        investigateSequence.AddCondition(new Inverter(this, (new IsTargetNull(this))));



        //Selector Sub-Root Node
        Selector RootSelector = new Selector(new List<BTNode>
                {
                investigateSequence,
                new VisualProximityCheck(this),
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
