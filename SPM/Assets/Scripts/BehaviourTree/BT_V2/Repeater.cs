using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : Decorator
{

    public Repeater(BTNode child, BehaviourTree bt) : base(child, bt) 
    {
    }
    public override Status Evaluate()
    {
        //Debug.Log("Child returned: " + m_child.Evaluate() + "from " + m_child);
        m_child.Evaluate();
        return Status.BH_RUNNING;
    }
}

/* 
 * G�r det att �teranv�nda denna om man vill utf�ra ett visst beteende x antal g�nger?  
 */
