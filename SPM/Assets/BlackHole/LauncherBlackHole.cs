using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherBlackHole : MonoBehaviour
{
    public GameObject cursor;
    public GameObject launchPoint;
    public BlackHole bh;
    public LayerMask collisionMask;
    public LineRenderer lr;
    public float flightTime = 1f;

    private int resolution = 10;
    private Camera cam;
    private bool isAiming;
    
    void Start()
    {
        cam = Camera.main;
        lr.positionCount = resolution + 1; 
    }

    // Update is called once per frame
    void Update()
    {
        if(isAiming)
            LaunchProjectile(); 
    }

    public void Activate() {
        lr.enabled = true;
        isAiming = true; }
    public void Deactivate() 
    {
        lr.enabled = false;
        isAiming = false;
        cursor.SetActive(false); }

    public float maxDistance = 400f;
    private void LaunchProjectile() 
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(camRay, out RaycastHit hit, 100f, collisionMask)) 
        {
            Vector3 lastLegalPoint = transform.position;

            if ((hit.point - launchPoint.transform.position).magnitude < maxDistance)
            {
                cursor.transform.position = hit.point;
                lastLegalPoint = hit.point;
            }

            if ((hit.point - launchPoint.transform.position).magnitude > maxDistance )
            {
                Debug.Log("if");
                Vector3 direction = (hit.point - launchPoint.transform.position);

                direction.Normalize();
                cursor.transform.position = lastLegalPoint;
                cursor.transform.position = launchPoint.transform.position - cursor.transform.localScale / 2 + direction * maxDistance;
            }

            Vector3 vo = CalculateVelocity(cursor.transform.position, launchPoint.transform.position, flightTime);
            
            DrawArc(vo, cursor.transform.position);

            transform.rotation = Quaternion.LookRotation(vo);
            if (Input.GetMouseButtonDown(0)) 
            {
                BlackHole obj = Instantiate(bh, launchPoint.transform.position, Quaternion.identity);
                obj.velocity = vo; 
            }
        }
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
   /* LaunchData CalculateLaunchData(Vector3 origin, Vector3 target) 
    {
        float displacementY = target.y - origin.y;
        Vector3 displacementXZ = target - origin;
        displacementXZ.y = 0f;

        float time = Mathf.Sqrt(-2*)
    }
    /*        Debug.Log("Drawpath: bh �r " + bh);
        float timeToTarget = Mathf.Sqrt(-2 * launchSpeedY / playerPhys.gravity) + Mathf.Sqrt(2 * (bh.velocity.y - launchSpeedY) / playerPhys.gravity);        
        Vector3 previousDrawPoint = bh.transform.position;

        int resolution = 30;
        for (int i = 0; i < resolution; i++) 
        {
            Debug.Log(i);
            float simulationTime = i / (float)resolution * timeToTarget;
            Vector3 displacement = bh.velocity * simulationTime +
            playerPhys.gravity * Vector3.down /*vector3.down??*/ //* simulationTime * simulationTime / 2f;
    /*Vector3 drawPoint = bh.transform.position + displacement;
    Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
    previousDrawPoint = drawPoint; */


    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time) 
    {
        //svarta hålet är lite stort, det blir imprecist när man skjuter på en vertikal yta

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
        float speedY = (-0.5f * bh.gravity* (time * time)) + (vo.y * time) + launchPoint.transform.position.y;

        result.y = speedY;
        return result;
    }
}
