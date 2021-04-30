using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inveter_V1 : Node
{
    //lista m barn
    private Node m_node;


    public Node node
    {
        get { return m_node; }
    }

    public Inveter_V1(Node node)
    {
        m_node = node;
    }
    //utvärdera alla barn, om alla lyckas returnerar vi success, avbryter annars vid första failure
    public override NodeStates Evaluate()
    {
            switch (m_node.Evaluate())
            {
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;

            }
        
        m_nodeState = NodeStates.SUCCESS;
        return m_nodeState;
    }
}
