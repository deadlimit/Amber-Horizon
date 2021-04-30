using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence_V1 : Node
{
    //lista m barn
    protected List<Node> m_nodes = new List<Node>();

    //m�ste skapas med lista �ver barn
    public Sequence_V1(List<Node> nodes)
    {
        m_nodes = nodes;
    }

    //utv�rdera alla barn, om alla lyckas returnerar vi success, avbryter annars vid f�rsta failure
    public override NodeStates Evaluate()
    {
        bool anyChildRunning = false;

        foreach(Node n in m_nodes)
        {
            switch (n.Evaluate())
            {
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.SUCCESS:
                    continue;
                case NodeStates.RUNNING:
                    anyChildRunning = true;
                    continue;
                default:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
            }     
        }
        m_nodeState = anyChildRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
        return m_nodeState;
    }

}
/*public override NodeStates Evaluate()
{
    foreach (Node n in m_nodes)
    {
       �f(n.Evaluate() != NodeStates.FAILURE)
            return n.m_nodeState
    }

                    ...eller? 
}*/