using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsFunctions
{
    public static Vector2 NormalForce(Vector2 velocity, Vector2 normal)
    {
        float dot = Vector2.Dot(velocity, normal);

        Vector2 projection = dot > 0 ? 0 * normal : dot * normal;
        return -projection;
    }
    public static Vector3 NormalForce3D(Vector3 velocity, Vector3 normal)
    {
        
        float dot = Vector3.Dot(velocity, normal);
        //vi kan få bort slope-problemen om vi multiplicerar dot * Vector3.up istället när spelaren träffar mark. Tveksamt vad som händer utöver det dock. 
        // bör man alltså ha separat kollision för marken? Hmm. 
        Vector3 projection = dot > 0 ? 0 * normal : dot * normal;
        return -projection;
    }

    //Returnerar endast friktionen som ska appliceras
    public static Vector3 CalcFriction(Vector3 normalForce, Vector3 vel, float staticFrictionCoefficient, float kineticFrictionCoefficient)
    {
        Vector3 temp = vel;
        if (vel.magnitude < normalForce.magnitude * staticFrictionCoefficient)
            vel = Vector3.zero;
        else
        {
            vel -= vel.normalized * normalForce.magnitude * kineticFrictionCoefficient;
        }
        return temp - vel;
    }
}
