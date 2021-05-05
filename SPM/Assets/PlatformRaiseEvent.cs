using UnityEngine;

public class PlatformRaiseEvent : MonoBehaviour, IEventPanelInteract {

    public Vector3 Destination;
    public float RaiseSpeed;
    public float LowerSpeed;
    
    private Vector3 startPosition;
    private float totalDistance;
    private void Awake() {
        startPosition = transform.localPosition;
        totalDistance = Vector3.Distance(startPosition, Destination);
    }

    private void Update() {
        transform.localPosition = MoveToTargetPosition(startPosition, LowerSpeed);
    }
    
    public void ActivateEvent() {
        transform.localPosition = MoveToTargetPosition(Destination, RaiseSpeed);
    }

    public void EventDone() {
        enabled = false;
    }

    public float CalculatePercentageDone() {
        
        float currentDistance = Vector3.Distance(transform.localPosition, Destination);

        return 1 - (currentDistance / totalDistance);
    }
    
    private Vector3 MoveToTargetPosition(Vector3 destination, float speed) {
        return Vector3.MoveTowards(transform.localPosition, destination, speed * Time.fixedDeltaTime);
    }
}
