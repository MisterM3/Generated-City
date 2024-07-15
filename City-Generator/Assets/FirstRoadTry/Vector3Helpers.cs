using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Helpers
{
    
    public static Vector3 Abs(this Vector3 vector)
    {
        vector.x = Mathf.Abs(vector.x);
        vector.y = Mathf.Abs(vector.y);
        vector.z = Mathf.Abs(vector.z);
        return vector;
    }
}
