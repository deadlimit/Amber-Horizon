using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }


    private void Update() {

        float zAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");
        animator.SetFloat("VelocityX", xAxis);
        animator.SetFloat("VelocityZ", zAxis);
    }
}
