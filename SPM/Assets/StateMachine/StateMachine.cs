using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine
{
    private Dictionary<Type, State> instantiatedStates = new Dictionary<Type, State>();
    private State currentState;

    public StateMachine(object owner, State[] states) 
    {
        Debug.Assert(states.Length > 0);

        foreach (State state in states) 
        {
            State instantiated = UnityEngine.Object.Instantiate(state);
            instantiated.Initialize(this, owner);
            instantiatedStates.Add(state.GetType(), instantiated);
            if (!currentState)
                currentState = instantiated;
        }

        //currentState?.Enter();
    }
    public void RunUpdate() 
    {
        currentState?.RunUpdate();
    }
    public void ChangeState<T>() where T : State {

        if (instantiatedStates.ContainsKey(typeof(T)))
        {
            State instance = instantiatedStates[typeof(T)];
            currentState?.Exit();
            currentState = instance;
            currentState.Enter();
        }
        else
            Debug.Log(typeof(T) + "not found");
    }
}
 