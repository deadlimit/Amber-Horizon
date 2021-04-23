using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherBlackHole : MonoBehaviour
{
    public GameObject cursor;
    public GameObject launchPoint;
    public LayerMask collisionMask;
    public LineRenderer lr;
    public float flightTime = 1f;
    public BlackHole bh;
    public Camera playerCam;

    private int resolution = 10;
    private Camera cam;
    private bool isAiming;

    public Vector3 cursorPos;
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

       cursorPos = cursor.transform.position;
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
            }

            else if ((hit.point - launchPoint.transform.position).magnitude > maxDistance)
            {
                cursor.transform.position = launchPoint.transform.position + camRay.direction * maxDistance;
            }          
        }

        else 
        {
            cursor.transform.position = launchPoint.transform.position + camRay.direction  * maxDistance;
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
