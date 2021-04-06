using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class General
{   public static Vector2 NormalForce(Vector2 velocity, Vector2 normal) 
    {
        float dot = Vector2.Dot(velocity, normal);

        Vector2 projection =  dot > 0 ? 0 * normal : dot * normal;
        return -projection;
    }
    public static Vector3 NormalForce3D(Vector3 velocity, Vector3 normal)
    {
        float dot = Vector3.Dot(velocity, normal);

        Vector3 projection = dot > 0 ? 0 * normal : dot * normal;
        return -projection;
    }
}
