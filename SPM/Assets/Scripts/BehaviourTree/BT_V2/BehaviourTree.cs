using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourTree : MonoBehaviour
{
    public class DataContainer {}
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

    //References
    public Enemy owner { get; private set; }
    public NavMeshAgent ownerAgent { get; private set; }
    public Transform ownerTransform { get; private set; }
    public TimerNode timerNode { get; protected set; }
    
    //variable values
    public Vector3 startPos { get; private set; }
    public float maxWaitTime { get; private set; }
    public float minWaitTime { get; private set; }

    
    protected BTNode m_root;
    public Dictionary<string, DataContainer> blackboard = new Dictionary<string, DataContainer>();

    public DataContainer<T> GetBlackBoardValue<T>(string blackboardKey)
    {
        return (DataContainer<T>)blackboard[blackboardKey];
    }

    protected void Start()
    {
        owner = GetComponent<Enemy>();
        ownerTransform = owner.transform;
        ownerAgent = owner.Pathfinder.agent;
        startPos = ownerTransform.position;
        ownerAgent.speed = owner.MovementSpeedDefault;

        maxWaitTime = owner.MaxWaitTimeOnPatrol;
        minWaitTime = owner.MinWaitTimeOnPatrol;

        blackboard.Add("Target", null);
        blackboard.Add("TargetTransform", new DataContainer<Transform>(null));
        blackboard.Add("LastSeenPosition", new DataContainer<Vector3>(new Vector3(0, 0, 0)));
        blackboard.Add("HasCalledForHelp", new DataContainer<bool>(new bool()));
        blackboard.Add("AlerterTransform", new DataContainer<Transform>(null));
    }


}
