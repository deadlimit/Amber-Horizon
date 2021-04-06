using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public abstract class State : ScriptableObject
{
    protected StateMachine stateMachine;
    protected object owner;

    public virtual void Initialize(StateMachine stateMachine, object owner) 
    {
        this.stateMachine = stateMachine;
        this.owner = owner;
        Initialize();
    }
    public virtual void Enter() { }
    public virtual void RunUpdate() { }
    public virtual void Exit() { }
    protected virtual void Initialize() { }
}
