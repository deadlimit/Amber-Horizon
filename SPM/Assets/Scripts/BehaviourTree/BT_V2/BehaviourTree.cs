using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    public Forager owner { get; private set; }
    protected BTNode m_root;
    private bool startedBehaviour;
    private Coroutine behaviour;

    public Dictionary<string, object> blackboard = new Dictionary<string, object>();

    private void Start()
    {
        Debug.Log("Behaviour tree skapat");
        this.owner = GetComponent<Forager>();
        Debug.Log("owner : " + owner);
        m_root = BehaviourTreeBuilder();
        blackboard.Add("Target", null);
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
        Filter f = new Filter(new List<BTNode>
                {
                new Patrol(owner, this),
                new Wait(this, 4f)
                }, this);
        f.AddCondition(new IsTargetNull(this));
        /* return new Sequence(new List<BTNode>
         {
             new SetPatrolPoint(owner, this), new MoveToPatrolPoint(owner ,this)
         }, this);*/
       return new Repeater(f,
           
           this);

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
}
