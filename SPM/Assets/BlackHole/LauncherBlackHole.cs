using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherBlackHole : MonoBehaviour
{
    public GameObject cursor;

    public Transform playerPos;
    public LayerMask collisionMask;
    public LineRenderer lr;
    public float flightTime = 1f;

    public BlackHole bh;
    private int resolution = 10;
    private Camera cam;
    private Controller3D player;
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
    void LaunchProjectile() 
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100f, collisionMask)) 
        {
            cursor.transform.position = hit.point + Vector3.up * 0.1f;
            Vector3 vo = CalculateVelocity(hit.point, playerPos.position, flightTime);

            DrawArc(vo, cursor.transform.position);

            transform.rotation = Quaternion.LookRotation(vo);
            if (Input.GetMouseButtonDown(0)) 
            {
                BlackHole obj = Instantiate(bh, playerPos.position, Quaternion.identity);
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

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time) 
    {
        //svarta hålet är lite stort, det blir imprecist när man skjuter på en vertikal yta

        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance.normalized;
        distanceXZ.y = 0f;

        float speedY = distance.y;
        float speedXZ = distance.magnitude;

        float velXZ = speedXZ / time;
        float velY = speedY / time + (0.5f * bh.gravity) * time;

        Vector3 trajectory = distanceXZ * velXZ;
        trajectory.y = velY;

        return trajectory;
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time) 
    {
        Vector3 result = playerPos.position + vo * time;
        float speedY = (-0.5f * bh.gravity * (time * time)) + (vo.y * time) + playerPos.position.y;

        result.y = speedY;
        return result;
    }
}
