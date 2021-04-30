using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NodeStates
{
    SUCCESS, FAILURE, RUNNING
}

public class Selector_V1 : Node
{
    //lista m barn
    protected List<Node> m_nodes = new List<Node>();

    public Selector_V1(List<Node> nodes)
    {
        m_nodes = nodes;
    }

    //utvärdera alla barn, om en lyckas så har Selector lyckats
    public override NodeStates Evaluate()
    {
        foreach(Node n in m_nodes)
        {
            switch (n.Evaluate())
            {
                case NodeStates.FAILURE:
                    continue;
                case NodeStates.SUCCESS:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
                case NodeStates.RUNNING:
                    m_nodeState = NodeStates.RUNNING;
                    return m_nodeState;
                default:
                    continue;
            }
        }
        m_nodeState = NodeStates.FAILURE;
        return m_nodeState;
    }

    //vill vi senare kunna lägga till fler barn, alltså efter konstruktion? 
    //AddChildNode(); 

}
