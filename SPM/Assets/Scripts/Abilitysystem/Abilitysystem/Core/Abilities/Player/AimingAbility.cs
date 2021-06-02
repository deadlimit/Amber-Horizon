using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "AimingAbility", menuName = "Abilities/AimingAbility")]
public class AimingAbility : GameplayAbility
{
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask collisionMask;

    [Header("Inspector Assigns")]
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject launchPoint;
    [SerializeField] private BlackHole bh;
   
    private LineRenderer lr;
    private float flightTime = 1f;
    private int resolution = 10;
    private Vector3 vo;
    private Transform cursorTransform;
    private Transform launchPointTransform;

    public override void Activate(GameplayAbilitySystem Owner) {

        //Scriptable objects behåller sina referenser från föregående scener
        //måste hämta på nytt efter scenetransit. 
        //Cached transforms
        if (cursor == null)
        {
            cursor = AimCursor.CursorObject.gameObject;
            cursorTransform = cursor.transform;
        }
        if (launchPoint == null)
        {
            launchPoint = LaunchPoint.Point.gameObject;
            launchPointTransform = launchPoint.transform;
        }

        lr = Owner.gameObject.GetComponent<LineRenderer>();
        lr.enabled = true;
        Owner.ApplyEffectToSelf(AppliedEffect);
        
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(camRay, out RaycastHit hit, Mathf.Infinity, collisionMask))
        {
            PlaceCursor(hit, camRay);
        }
        else
        {
            cursor.SetActive(false);
            cursorTransform.position = launchPointTransform.position + camRay.direction * maxDistance;
        }

        vo = CalculateVelocity(cursorTransform.position, launchPointTransform.position, flightTime);
        DrawArc(vo, cursorTransform.position);

    }
    private void PlaceCursor(RaycastHit hit, Ray camRay)
    {
        if ((hit.point - launchPointTransform.position).magnitude <= maxDistance)
        {
            cursor.SetActive(true);
            cursorTransform.position = hit.point;
            Quaternion q = Quaternion.FromToRotation(Vector3.up, hit.normal);
            cursorTransform.rotation = Quaternion.Slerp(cursorTransform.rotation, q, 20 * Time.deltaTime);
        }
        else if ((hit.point - launchPointTransform.position).magnitude > maxDistance)
        {
            cursor.SetActive(false);
            cursorTransform.position = launchPointTransform.position + camRay.direction * maxDistance;
        }
    }
    public void FireBlackHole() {
        GameObject blackHole = ObjectPooler.Instance.Spawn("BlackHole", launchPointTransform.position, Quaternion.identity);
        //BlackHole obj = Instantiate(bh, launchPoint.gameObject.transform.position, Quaternion.identity);
        blackHole.GetComponent<BlackHole>().velocity = vo;
    }

    void DrawArc(Vector3 vo, Vector3 finalPos)
    {
        for (int i = 0; i < resolution; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, (i / (float)resolution));
            lr.SetPosition(i, pos);
        }
        lr.SetPosition(resolution, finalPos);
    }
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance.normalized;
        distanceXZ.y = 0f;

        float displacementY = distance.y;
        float displacementXZ = distance.magnitude;

        float velXZ = displacementXZ / time;
        float velY = displacementY / time + (0.5f * bh.GetGravity()) * time;

        Vector3 trajectory = distanceXZ * velXZ;
        trajectory.y = velY;

        return trajectory;
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 result = launchPointTransform.position + vo * time;
        float speedY = (-0.5f * bh.GetGravity() * (time * time)) + (vo.y * time) + launchPointTransform.position.y;

        result.y = speedY;
        return result;
    }

    public override void Deactivate(GameplayAbilitySystem Owner)
    {
        if(lr)
            lr.enabled = false;
        cursor.SetActive(false);
        Owner.RemoveTag(this.AbilityTag);
    }


}

