using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CheckRoadSO : ScriptableObject
{
    public abstract bool CheckRoad(RoadPosition roadPosition, List<RoadPosition> allRoads);
}
