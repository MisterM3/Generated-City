using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGizmosTest : MonoBehaviour
{

    [SerializeField] private List<RoadPosition> roadPositions;

    [SerializeField] bool randomSeed = false;
    [SerializeField] int seed = 10;


    [SerializeField] int amountRoads = 10;
    int amountLeft;

    private void Awake()
    {
        if (randomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(seed);


    }

    [ContextMenu("TestRoads")]
    public void DrawRoads()
    {
        if (randomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(seed);
        roadPositions = new();
        roadPositions.Clear();

        amountLeft = amountRoads;

        Vector3 startPos = Vector3.zero;



        AddNewLine(startPos, Vector3.forward);
        AddNewLine(startPos, Vector3.back);
        AddNewLine(startPos, Vector3.left);
        AddNewLine(startPos, Vector3.right);

    }


    public IEnumerator AddNewLine(Vector3 startPos, Vector3 direction)
    {

        amountLeft--;
        if (amountLeft <= 0)
            return;

        RoadPosition pos = DrawNewRoad(startPos, direction);

        bool intersects = false;

        foreach (RoadPosition roadPosition in roadPositions)
        {
            if (RoadHelpers.LineLineIntersection(out Vector3 intersection, pos, roadPosition))
            {
                if (intersection == pos.startPos)
                    continue;
                pos.endPos = intersection;
                intersects = true;
            }
        }

        roadPositions.Add(pos);

        if (intersects) return;


        if (RandomHelper.CoinToss())
        {
            AddNewLine(pos.endPos, Quaternion.AngleAxis(90, Vector3.up) * direction);
        }
        if (RandomHelper.CoinToss())
        {
            AddNewLine(pos.endPos, Quaternion.AngleAxis(-90, Vector3.up) * direction);
        }
        if (RandomHelper.CoinToss())
        {
            AddNewLine(pos.endPos, direction);
        }

    }

    public IEnumerator DrawNewRoad(out RoadPosition roadPosition, Vector3 startPoint, Vector3 directionVector)
    {

        RoadPosition roadPosition = new();
        roadPosition.startPos = startPoint;
        Vector3 endPos = startPoint;


        bool isStopped = false;

      //  Random.state = randomstate;

        while (!isStopped)
        {

            Debug.Log(directionVector);
            Debug.Log(directionVector * Random.Range(.25f, 1f));
            endPos += directionVector * Random.Range(.25f, 1f);
            Debug.Log(endPos);

            float checkEnd = Random.Range(0f, 1f);
            if (checkEnd > .75f)
            {
                isStopped = true;
            }

        }
        roadPosition.endPos = endPos;
  
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (roadPositions == null)
        {
           return;
        }
        foreach(RoadPosition road in roadPositions)
        {
            
            


            Gizmos.DrawLine(road.startPos, road.endPos);
        }
    }
}

[System.Serializable]
public struct RoadPosition
{
    public Vector3 startPos;
    public Vector3 endPos;
}
