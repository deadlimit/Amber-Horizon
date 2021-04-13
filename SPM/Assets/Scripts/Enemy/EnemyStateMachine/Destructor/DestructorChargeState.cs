using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Destructor Charge State", menuName = "New Destructor Charge State")]
public class DestructorChargeState : State {

    private Destructor destructor;

    protected override void Initialize() {
        destructor = (Destructor) owner;
    }

    public override void Enter() {
        destructor.Animator.SetTrigger("Melee");
    }

    public override void RunUpdate() {
        destructor.transform.LookAt(destructor.Target);
        
    }
}
