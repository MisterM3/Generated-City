using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadHelpers 
{
    public static bool LineLineFarIntersection(out Vector3 intersection, Vector3 linePoint1,
            Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }


    public static bool LineLineIntersection(out Vector3 intersectionPoint, Vector3 lineStart1,
            Vector3 lineEnd1, Vector3 lineStart2, Vector3 lineEnd2)
    {
        Vector3 intersection;
        Vector3 aDiff = lineEnd1 - lineStart1;
        Vector3 bDiff = lineEnd2 - lineStart2;
        if (LineLineFarIntersection(out intersection, lineStart1, aDiff, lineStart2, bDiff))
        {
            float aSqrMagnitude = aDiff.sqrMagnitude;
            float bSqrMagnitude = bDiff.sqrMagnitude;

            if ((intersection - lineStart1).sqrMagnitude <= aSqrMagnitude
                 && (intersection - lineEnd1).sqrMagnitude <= aSqrMagnitude
                 && (intersection - lineStart2).sqrMagnitude <= bSqrMagnitude
                 && (intersection - lineEnd2).sqrMagnitude <= bSqrMagnitude)
            {
                intersectionPoint = intersection;
                return true;
            }
        }

        intersectionPoint = Vector3.zero;
        return false;
    }

    public static bool LineLineIntersection(out Vector3 intersectionPoint, RoadPosition roadOne, RoadPosition roadTwo)
    {

        Vector3 intersection;
        Vector3 aDiff = roadOne.endPos - roadOne.startPos;
        Vector3 bDiff = roadTwo.endPos - roadTwo.startPos;
        if (LineLineFarIntersection(out intersection, roadOne.startPos, aDiff, roadTwo.startPos, bDiff))
        {
            float aSqrMagnitude = aDiff.sqrMagnitude;
            float bSqrMagnitude = bDiff.sqrMagnitude;

            if ((intersection - roadOne.startPos).sqrMagnitude <= aSqrMagnitude
                 && (intersection - roadOne.endPos).sqrMagnitude <= aSqrMagnitude
                 && (intersection - roadTwo.startPos).sqrMagnitude <= bSqrMagnitude
                 && (intersection - roadTwo.endPos).sqrMagnitude <= bSqrMagnitude)
            {
                intersectionPoint = intersection;
                return true;
            }
        }

        intersectionPoint = Vector3.zero;
        return false;
    }
}
