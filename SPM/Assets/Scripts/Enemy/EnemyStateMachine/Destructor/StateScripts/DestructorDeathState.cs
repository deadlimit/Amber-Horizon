using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestructorDeathState", menuName = "Enemy States/Destructor/New DestructorDeathState")]

public class DestructorDeathState : State
{
    private Destructor destructor;
    protected override void Initialize()
    {
        destructor = owner as Destructor;
    }
    public override void RunUpdate()
    {
        Destroy(destructor.gameObject, 0.5f);
    }
}
