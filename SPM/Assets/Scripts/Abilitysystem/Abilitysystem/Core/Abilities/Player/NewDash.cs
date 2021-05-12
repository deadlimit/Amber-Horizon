using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDash", menuName = "Abilities/NewDash")]
public class NewDash: GameplayAbility {

    [SerializeField] private float dashLength;
    [SerializeField] private float timeWithoutGravity;
    [SerializeField] private int samplePoints;

    public override void Activate(GameplayAbilitySystem Owner) {

        Owner.StartCoroutine(Dash(Owner));


    }

    private IEnumerator Dash(GameplayAbilitySystem Owner) {


        Vector3 start = Owner.transform.position + Vector3.up * 3;
        Vector3 end = start + Owner.transform.forward * dashLength;


        List<Vector3> points = new List<Vector3>();

        float percent = samplePoints * .01f;
            
        for (int i = 0; i < samplePoints; i++) {
            points.Add(Vector3.Lerp(start, end, percent));
            percent += .01f;
        }

        
        for (int i = 0; i < points.Count; i++) {

            Physics.Raycast(points[i], Vector3.down, out var hit, 3, LayerMask.GetMask("Ground"));

            if (hit.collider)
                points[i] = hit.point;
            else {
                Vector3 feetLevel = points[i];
                feetLevel.y = Owner.transform.position.y;
                points[i] = feetLevel;
            }
        }
        
        for (int i = 1; i < points.Count; i++) 
            Debug.DrawLine(points[i - 1], points[i], Color.yellow , 5);
        
        yield return null;
        
    }
}
