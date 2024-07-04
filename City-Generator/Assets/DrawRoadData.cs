using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DrawRoadData : MonoBehaviour
{

    [SerializeField] private List<RoadPosition> roadPositions;
    public List<RoadPosition> RoadPositions => roadPositions;


    [SerializeField] private List<CrossRoadData> crossRoadData;

    private List<RoadPosition> removedRoadPositions;

    private List<RoadPosition> needConnectingPositions;

    private List<RoadSave> endPointDirectionDictionary;

    [SerializeField] bool randomSeed = false;
    [SerializeField] int seed = 10;

    [SerializeField] int minDistance = 5;


    [SerializeField] AnimationCurve curve;

    [SerializeField] int amountRoads = 10;
    int amountLeft;


    [SerializeField] GameObject test;
    [SerializeField] GameObject parent;

    private void Awake()
    {
        if (randomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(seed);

        roadPositions = new();
        roadPositions.Clear();
        needConnectingPositions = new();
        needConnectingPositions.Clear();
        endPointDirectionDictionary = new();

        removedRoadPositions = new();

        amountLeft = amountRoads;

        Vector3 startPos = Vector3.zero;

        AddToList(startPos, Vector3.forward);
        AddToList(startPos, Vector3.back);
        AddToList(startPos, Vector3.left);
        AddToList(startPos, Vector3.right);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OneRoadIteration();
        }
    }

    public async void OneRoadIteration()
    {

        if ((!(amountLeft <= 0 || endPointDirectionDictionary.Count == 0)))
        {
            for (int i = endPointDirectionDictionary.Count - 1; i >= 0; i--)
            {
                RoadSave rs = endPointDirectionDictionary[i];

                await AddNewLine(rs.startPos, rs.direction);
                endPointDirectionDictionary.RemoveAt(i);
            }
        }
    }

    [ContextMenu("TestRoads")]
    public async void DrawRoads()
    {
        if (randomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(seed);
        roadPositions = new();
        roadPositions.Clear();
        crossRoadData.Clear();
        crossRoadData.Add(new(Vector3.zero));
        needConnectingPositions = new();
        needConnectingPositions.Clear();
        endPointDirectionDictionary = new();

        removedRoadPositions = new();

        amountLeft = amountRoads;

        Vector3 startPos = Vector3.zero;

        AddToList(startPos, Vector3.forward);
        AddToList(startPos, Vector3.back);
        AddToList(startPos, Vector3.left);
        AddToList(startPos, Vector3.right);


        while (!(amountLeft <= 0 || endPointDirectionDictionary.Count == 0))
        {

            for (int i = endPointDirectionDictionary.Count - 1; i >= 0; i--)
            {
                RoadSave rs = endPointDirectionDictionary[i];

                await AddNewLine(rs.startPos, rs.direction);
                endPointDirectionDictionary.RemoveAt(i);
            }
        }


        for (int i = crossRoadData.Count - 1; i >= 0; i--)
        {
            Vector3 positionCrossRoad = crossRoadData[i].Position;

            Vector3 dirFirst = Vector3.zero;
            Vector3 dirSecond = Vector3.zero;

            int indexOne = -1;
            int indexTwo = -1;

            bool moreThanTwoConnections = false;


            for (int j = roadPositions.Count - 1; j >= 0; j--)
            {
                RoadPosition roadPosition = roadPositions[j];

                if (roadPosition.startPos != positionCrossRoad && roadPosition.endPos != positionCrossRoad)
                    continue;

                if (dirFirst == Vector3.zero)
                {
                    dirFirst = roadPosition.endPos - roadPosition.startPos;
                    dirFirst.Normalize();
                    indexOne = j;
                    continue;
                }
                else if (dirSecond == Vector3.zero)
                {
                    dirSecond = roadPosition.endPos - roadPosition.startPos;
                    dirSecond.Normalize();
                    indexTwo = j;
                    continue;
                }

                moreThanTwoConnections = true;
                break;
            }

            if (moreThanTwoConnections)
                continue;

            if(dirFirst == dirSecond)
            {
                crossRoadData.RemoveAt(i);
                RoadPosition rd = roadPositions[indexOne];
                rd.startPos = roadPositions[indexTwo].startPos;
                roadPositions[indexOne] = rd;
                roadPositions.RemoveAt(indexTwo);

            }
        }

        return;

        foreach(CrossRoadData cross in crossRoadData)
        {
            var obj = Instantiate(test, cross.Position, Quaternion.identity);
            obj.transform.SetParent(parent.transform, true);
        }

    }

    public void AddToList(Vector3 start, Vector3 direction)
    {
        RoadSave rs = new();
        rs.startPos = start;
        rs.direction = direction;

        endPointDirectionDictionary.Add(rs);
    }

    public async Task AddNewLine(Vector3 startPos, Vector3 direction)
    {

        amountLeft--;
        if (amountLeft <= 0)
            return;

        RoadPosition pos = await DrawNewRoad(startPos, direction);

        if (pos.startPos.y == -1)
        {
            Debug.Log("too small");
            removedRoadPositions.Add(pos);
            amountLeft++;
            return;
        }

        if (pos.startPos.y == -2)
        {
            Debug.Log("why! " + pos.startPos.y);
            needConnectingPositions.Add(pos);
            amountLeft++;
            return;
        }

        bool intersects = false;


        for(int i = roadPositions.Count - 1; i >= 0; i--)
        {
            RoadPosition roadPosition = roadPositions[i];

            if (RoadHelpers.LineLineIntersection(out Vector3 intersection, pos, roadPosition))
            {
                if (intersection == pos.startPos)
                    continue;
                pos.endPos = intersection;
                intersects = true;

                RoadPosition splitRoadOne = new RoadPosition() { startPos = roadPosition.startPos, endPos = intersection };
                RoadPosition splitRoadTwo = new RoadPosition() { startPos = intersection, endPos = roadPosition.endPos };

                roadPositions.RemoveAt(i);

                removedRoadPositions.Add(roadPosition);

                if (splitRoadOne.endPos - splitRoadOne.startPos != Vector3.zero)
                    roadPositions.Add(splitRoadOne);
                if (splitRoadTwo.endPos - splitRoadTwo.startPos != Vector3.zero)
                    roadPositions.Add(splitRoadTwo);

            }
        }

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

        crossRoadData.Add(new(pos.endPos, pos.endPos - pos.startPos));

        if (intersects) return;


        if (RandomHelper.CoinToss())
        {
            AddToList(pos.endPos, Quaternion.AngleAxis(90, Vector3.up) * direction);
        }
        if (RandomHelper.CoinToss())
        {
            AddToList(pos.endPos, Quaternion.AngleAxis(-90, Vector3.up) * direction);
        }
        if (RandomHelper.CoinToss())
        {
            AddToList(pos.endPos, direction);
        }

    }

    public async Task<RoadPosition> DrawNewRoad(Vector3 startPoint, Vector3 directionVector)
    {

        RoadPosition roadPosition = new();
        roadPosition.startPos = startPoint;
        Vector3 endPos = startPoint;


        bool isStopped = false;

        while (!isStopped)
        {
            endPos += directionVector * Random.Range(5f, 20f);

            Vector3 difference = endPos - startPoint;

            float percentageChange = curve.Evaluate(difference.magnitude/20);

            float checkEnd = Random.Range(0f, 1f);
            if (checkEnd < percentageChange)
            {
                isStopped = true;
            }

        }
        roadPosition.endPos = endPos;

        RoadPosition intersectRoadPosition = new();

        foreach(RoadPosition rp in roadPositions)
        {

            float distance = RoadHelpers.ShortestBridge(rp, roadPosition);

            if ((distance < minDistance) && !roadPosition.SameStartEndPoint(rp))
            {
                if (RoadHelpers.IsPerpendicular(rp, roadPosition))
                {
                    roadPosition.startPos.y = -1;
                    Debug.Log($"Too Close: {rp.startPos} : {rp.endPos} OtherRoad {roadPosition.startPos} : {roadPosition.endPos} Distance {distance}");
                    break;
                }
                else
                {
                   // roadPosition.startPos.y = -2;
                    intersectRoadPosition = rp;
                }
            }
           

        }

        if (intersectRoadPosition.startPos != Vector3.zero)
        {
         //   roadPosition.endPos = intersectRoadPosition.startPos;
            Debug.Log("got one");

            var dif = roadPosition.endPos - roadPosition.startPos;

            Debug.Log(dif);

            if ((dif.normalized == Vector3.forward) || (dif.normalized == Vector3.back))
                Debug.Log("ome");
            if ((dif.normalized == Vector3.left) || (dif.normalized == Vector3.right))
                Debug.Log("fa");


            if (RoadHelpers.LineLineFarIntersection(out Vector3 intersection, intersectRoadPosition, roadPosition))
            {
                Debug.Log("true");
                roadPosition.endPos = intersection;
            }
        }

        return roadPosition;
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


        Gizmos.color = Color.blue;

        if (removedRoadPositions.Count == null)
            return;

        if (removedRoadPositions.Count != 0)
        {
            for (int i = removedRoadPositions.Count - 1; i >= 0; i--)
            {
                Vector3 rp = removedRoadPositions[i].startPos;
                rp.y = 0;

                Gizmos.DrawLine(rp, removedRoadPositions[i].endPos);
            }
        }

        if (needConnectingPositions.Count == 0)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        for (int i = needConnectingPositions.Count - 1; i >= 0; i--)
        {
            Vector3 rp = needConnectingPositions[i].startPos;
            rp.y = 0;

            Gizmos.DrawLine(rp, needConnectingPositions[i].endPos);
        }
    }
    

}

[System.Serializable]
public struct RoadPosition
{
    public Vector3 startPos;
    public Vector3 endPos;
}

public struct RoadSave
{
    public Vector3 startPos;
    public Vector3 direction;

}
