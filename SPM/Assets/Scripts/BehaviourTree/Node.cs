using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    public delegate NodeStates NodeReturn();

    protected NodeStates m_nodeState;

    public NodeStates nodeState
    {
        get { return m_nodeState; }
    }
    public abstract NodeStates Evaluate();
}
