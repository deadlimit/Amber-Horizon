using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "AimingAbility", menuName = "Abilities/AimingAbility")]
public class AimingAbility : GameplayAbility
{
    public GameObject cursor;
    public GameObject launchPoint;
    public LayerMask collisionMask;
    public LineRenderer lr;
    public float flightTime = 1f;
    public BlackHole bh;
    public Camera playerCam;
    public float maxDistance = 10f;

    private int resolution = 10;
    private Camera cam;
    private Vector3 vo;
    private void Awake()
    {
        
    }
    public override void Activate(GameplayAbilitySystem Owner)
    {
        lr = Owner.gameObject.GetComponent<LineRenderer>();
        Debug.Assert(lr);
        lr.enabled = true;
        Owner.ApplyEffectToSelf(AppliedEffect);
        cam = Camera.main;
        launchPoint = GameObject.FindGameObjectWithTag("LaunchPoint");

            Ray camRay = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(camRay, out RaycastHit hit, 100f, collisionMask))
            {
                if ((hit.point - launchPoint.transform.position).magnitude < maxDistance)
                {
                    cursor.transform.position = hit.point;
                }

                else if ((hit.point - launchPoint.transform.position).magnitude > maxDistance)
                {
                    cursor.transform.position = launchPoint.transform.position + camRay.direction * maxDistance;
                }
            }

            else
            {
                cursor.transform.position = launchPoint.transform.position + camRay.direction * maxDistance;
            }

        vo = CalculateVelocity(cursor.transform.position, launchPoint.transform.position, flightTime);
        DrawArc(vo, cursor.transform.position);

    }
    public void FireBlackHole() 
    {
        BlackHole obj = Instantiate(bh, launchPoint.gameObject.transform.position, Quaternion.identity);
        obj.velocity = vo;
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
        float velY = displacementY / time + (0.5f * bh.gravity) * time;

        Vector3 trajectory = distanceXZ * velXZ;
        trajectory.y = velY;

        return trajectory;
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 result = launchPoint.transform.position + vo * time;
        float speedY = (-0.5f * bh.gravity * (time * time)) + (vo.y * time) + launchPoint.transform.position.y;

        result.y = speedY;
        return result;
    }
}

