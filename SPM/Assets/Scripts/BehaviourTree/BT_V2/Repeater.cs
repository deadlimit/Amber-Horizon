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

/* kan man inte göra en for-loop med repeats istället?? 
 * Status Repeat::update() {
while (true) {
 m_pChild->tick();
 if (m_pChild->getStatus() == BH_RUNNING) break;
 if (m_pChild->getStatus() == BH_FAILURE) return BH_FAILURE;
 if (++m_iCounter == m_iLimit) return BH_SUCCESS;
}

/*        for (int i = 0; i < repeats; i++)
        {
            m_child.Tick();
            if (m_child.getStatus() == Status.BH_RUNNING)
                break;
            if (m_child.getStatus() == Status.BH_FAILURE)
                return Status.BH_FAILURE;
        }
}*/
