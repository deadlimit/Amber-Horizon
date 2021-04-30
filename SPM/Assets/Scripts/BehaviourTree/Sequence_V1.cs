using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence_V1 : Node
{
    //lista m barn
    protected List<Node> m_nodes = new List<Node>();

    //måste skapas med lista över barn
    public Sequence_V1(List<Node> nodes)
    {
        m_nodes = nodes;
    }

    //utvärdera alla barn, om alla lyckas returnerar vi success, avbryter annars vid första failure
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
       íf(n.Evaluate() != NodeStates.FAILURE)
            return n.m_nodeState
    }

                    ...eller? 
}*/