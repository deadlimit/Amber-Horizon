using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    BH_INVALID, BH_RUNNING, BH_FAILURE, BH_SUCCESS
}
public class BTNode
{ 
    public string name { get; private set; }
    protected BehaviourTree bt;
    private Status m_status { get; set; }

    //Constructors
    public BTNode(BehaviourTree bt) 
    {
        name = "";
        this.bt = bt;
        m_status = Status.BH_INVALID;
    }
    public BTNode(BehaviourTree bt, string name)
    {
        this.name = name;
        this.bt = bt;
        m_status = Status.BH_INVALID;
    }
    
    #region BTNode methods
    public virtual void OnInitialize() { }
    public Status Tick()
    {
        if (m_status != Status.BH_RUNNING)
            OnInitialize();
        m_status = Evaluate();
        if (m_status != Status.BH_RUNNING) OnTerminate(m_status);
        return m_status;
    }
    public virtual Status Evaluate()
    {
        return Status.BH_FAILURE;
    }
    public virtual void OnTerminate(Status status) { }
    #endregion

   
}
