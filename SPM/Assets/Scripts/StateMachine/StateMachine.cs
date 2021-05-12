using System.Collections.Generic;
using UnityEngine;
using System;
using EventCallbacks;

public class StateMachine
{
    private Dictionary<Type, State> instantiatedStates = new Dictionary<Type, State>();
    public State CurrentState { get; private set; }

    public StateMachine(object owner, State[] states) 
    {
        Debug.Assert(states.Length > 0);

        foreach (State state in states) 
        {
            State instantiated = UnityEngine.Object.Instantiate(state);
            instantiated.Initialize(this, owner);
            instantiatedStates.Add(state.GetType(), instantiated);
            
            if (!CurrentState)
                CurrentState = instantiated;
        }
        CurrentState.Enter();
        
    }
    public void RunUpdate() 
    {
        CurrentState?.RunUpdate();
    }
    public void ChangeState<T>() where T : State {

        if (instantiatedStates.ContainsKey(typeof(T)))
        {
            State instance = instantiatedStates[typeof(T)];
            CurrentState?.Exit();
            CurrentState = instance;
            CurrentState.Enter();
        }
        else
            Debug.Log(typeof(T) + "not found");
    }
    
}
 