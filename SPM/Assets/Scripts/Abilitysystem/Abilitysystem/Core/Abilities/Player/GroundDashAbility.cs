using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using EventCallbacks;
using UnityEngine;

[CreateAssetMenu(fileName = "Air Dash", menuName = "Abilities/Air Dash")]
public class GroundDashAbility : GameplayAbility {

    public float timeWithOutGravity;
    public float dashLength;
    
    public override void Activate(GameplayAbilitySystem Owner) {
        Owner.StartCoroutine(Dash(Owner));
    }
    
    private IEnumerator Dash(GameplayAbilitySystem Owner) {
        
        
        
    }

    private List<Vector3> CalculateDeltaPositions(Vector3 start, Vector3 end, int points) {
        
        for (int i = 0; i < points; i++) {
                
        }
    } 
}