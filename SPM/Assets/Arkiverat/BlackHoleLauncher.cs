using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleLauncher : MonoBehaviour
{
    LineRenderer lr;
    public float velocity;
    public float angle;
    public int resolution = 10;

    float g;
    float radianAngle;
    Vector3 playerPos;
    Controller3D player;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        player = GetComponent<Controller3D>();
    }
    private void Start()
    {
        g = player.playerPhys.gravity;
    }

    private void Update()
    {
        playerPos = player.transform.position;
        RenderArc();
    }
    void RenderArc() 
    {
        lr.positionCount = resolution +1;
        lr.SetPositions(CalculateArcArray());
    }

    public void SetAngle(float pAngle) { angle = pAngle;  }
    public  void SetVelocity(float pVelocity) { velocity = pVelocity; }
    Vector3 [] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = ((velocity * velocity * Mathf.Sin(2 * radianAngle)) / g) ;

        for (int i = 0; i <= resolution; i++) 
        {
            if (i == 0)
                arcArray[i] = player.transform.position;

            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    //r�kna ut position f�r varje vertex
    Vector3 CalculateArcPoint(float t, float maxDistance) 
    {
        //n�gonting g�r att velocity p�verkar arcArray[0]:s position,
        //vilket inte borde vara m�jligt eftersom att ddet indexet �r uteslutet ur loopen
        float x = (playerPos.x + t * maxDistance) ;
        float y = playerPos.y +  t * maxDistance * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle))) ;
        float z = playerPos.z;

        return   new Vector3(x, y, z);



    }


}
