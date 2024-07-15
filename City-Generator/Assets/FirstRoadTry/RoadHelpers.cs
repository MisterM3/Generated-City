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

    public static bool LineLineFarIntersection(out Vector3 intersection, RoadPosition roadOne, RoadPosition roadTwo)
    {  
        Vector3 aDiff = roadOne.endPos - roadOne.startPos;
        Vector3 bDiff = roadTwo.endPos - roadTwo.startPos;
        return LineLineFarIntersection(out intersection, roadOne.startPos, aDiff.normalized, roadTwo.startPos, bDiff.normalized);
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
        return LineLineIntersection(out intersectionPoint, roadOne.startPos, roadOne.endPos, roadTwo.startPos, roadTwo.endPos);
    }


    public static bool IsPerpendicular(Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {
        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is parralel
        return !(Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f);
    }

    public static bool IsPerpendicular(RoadPosition roadOne, RoadPosition roadTwo)
    {
        Vector3 aDiff = roadOne.endPos - roadOne.startPos;
        Vector3 bDiff = roadTwo.endPos - roadTwo.startPos;

        float angle = Vector3.Angle(bDiff, aDiff);
        Debug.Log(angle);
        return angle <= 10.0f;
    }

    public static bool SameStartEndPoint(this RoadPosition roadPosition, RoadPosition otherRoad)
    {
        return (roadPosition.startPos == otherRoad.endPos || roadPosition.endPos == otherRoad.startPos || roadPosition.startPos == otherRoad.startPos || roadPosition.endPos == otherRoad.endPos);
    }

    //https://stackoverflow.com/questions/2824478/shortest-distance-between-two-line-segments
    public static float ShortestBridge(RoadPosition a, RoadPosition b, bool clamp = true)
    {
      //  if (a.Length == 0 || b.Length == 0) throw new ArgumentException("The supplied lines must not have a length of zero!");

        var eta = 1e-6;
        var r = b.startPos - a.startPos;
        var u = a.endPos - a.startPos;
        var v = b.endPos - b.startPos;
        var ru = Vector3.Dot(r, u);
        var rv = Vector3.Dot(r, v);
        var uu = Vector3.Dot(u, u);
        var uv = Vector3.Dot(u, v);
        var vv = Vector3.Dot(v, v);
        var det = uu * vv - uv * uv;

        float s, t;
        if (det < eta * uu * vv)
        {
            s = OptionalClamp01(ru / uu, clamp);
            t = 0;
        }
        else
        {
            s = OptionalClamp01((ru * vv - rv * uv) / det, clamp);
            t = OptionalClamp01((ru * uv - rv * uu) / det, clamp);
        }

        var S = OptionalClamp01((t * uv + ru) / uu, clamp);
        var T = OptionalClamp01((s * uv - rv) / vv, clamp);

        var A = a.startPos + S * u;
        var B = b.startPos + T * v;
        return Vector3.Distance(A, B);
    }

    private static float OptionalClamp01(float value, bool clamp)
    {
        if (!clamp) return value;
        if (value > 1) return 1;
        if (value < 0) return 0;
        return value;
    }
}
