using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagerBehaviourTree : MonoBehaviour
{
    Forager forager;
    GameObject target;
    Vector3 patrolPoint;
    Node behaviourTree;

    //------------ vad gör dessa egentligen?
    public delegate void TreeExecuted();
    public event TreeExecuted onTreeExecuted;

    public delegate void NodePassed(string trigger);
    //----------------------------------------------

    private void Awake()
    {
        Debug.Log("FBT Awake");
        forager = gameObject.GetComponent<Forager>();
        Debug.Assert(forager);
    }

    public Selector_V1 rootNode;
    private ActionNode patrolNode;
    //BT
    private void Start()
    {
        patrolNode = new ActionNode(TargetCheck);

        rootNode = new Selector_V1(new List<Node>  {

        patrolNode
        });
       
    }
    private void FixedUpdate()
    {
        Evaluate();
    }

    public void Evaluate()
    {
        rootNode.Evaluate();
        StartCoroutine(Execute());
    }

    private IEnumerator Execute()
    {
        Debug.Log("inuti execute");
        yield return new WaitForSeconds(2.5f);

        if(patrolNode.nodeState == NodeStates.SUCCESS)
        {
            Debug.Log("Start Patrol");
            MoveToPatrolPoint();
        }
        if(onTreeExecuted !=null)
        {
            onTreeExecuted();
        }
    }
    //om den ser spelaren, sätt som target och anfall
    private NodeStates TargetCheck()
    {
        if (target != null)
            return NodeStates.SUCCESS;
        return NodeStates.FAILURE;
    }



    //Annars idla

    private void SetPatrolPoint()
    {
        forager.Pathfinder.GetSamplePositionOnNavMesh(forager.transform.position, 10, 100);
    }
    public void MoveToPatrolPoint()
    {      
        forager.Pathfinder.agent.SetDestination(patrolPoint);
    }

    private void ReachedPatrolPoint()
    {
        Evaluate();
    }

}
