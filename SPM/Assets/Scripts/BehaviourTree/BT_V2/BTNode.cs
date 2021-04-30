using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    BH_INVALID, BH_RUNNING, BH_FAILURE, BH_SUCCESS
}
public class BTNode
{ 
    public BehaviourTree m_tree;
    public virtual void OnInitialize() { }
    public virtual Status Evaluate() 
    { 
        return Status.BH_FAILURE; 
    }
    public virtual void OnTerminate(Status status) { }

    public BTNode(BehaviourTree bt) 
    {
        m_tree = bt;
        m_status = Status.BH_INVALID;
       // this.owner = (Forager)owner;
    }
    public Status Tick()
    {
        if (m_status != Status.BH_RUNNING)
            OnInitialize();
        m_status = Evaluate();
        if (m_status != Status.BH_RUNNING) OnTerminate(m_status);
        return m_status;

    }
    public Status getStatus()
    {
        return m_status;
    }
    private Status m_status;
}
