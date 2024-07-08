using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class DrawRoadData : MonoBehaviour
{

    [SerializeField] private List<RoadPosition> roadPositions;
    public List<RoadPosition> RoadPositions => roadPositions;


    [SerializeField] private List<CrossRoadData> crossRoadData;
    public List<CrossRoadData> CrossRoadData => crossRoadData;

    private List<RoadPosition> removedRoadPositions;

    private List<RoadPosition> needConnectingPositions;

    private List<RoadSave> endPointDirectionDictionary;

    private Dictionary<Vector3, CrossRoadData> crossRoadPairs;


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
            Debug.Log("click");
            OneRoadIteration();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("clccick");
            CheckRoads();
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

    public void CheckRoads()
    {

        for (int i = roadPositions.Count - 1; i >= 0; i--)
        {
            RoadPosition roadOne = roadPositions[i];
            Vector3 endPosRoad = roadOne.endPos;

            for (int j = roadPositions.Count - 1; j >= 0; j--)
            {

                RoadPosition roadTwo = roadPositions[j];
                if (roadOne == roadTwo)
                    continue;

                if (roadOne.IsParralel(roadTwo))
                    continue;

                float distance = roadTwo.GetClosestPointToLine(endPosRoad);

                if (distance >= minDistance || distance == 0)
                    continue;



                roadOne.endPos += roadOne.GetNormalizedDirection() * distance;


                //At wrong side of road (so goes away from road, not towards
                if (roadTwo.GetClosestPointToLine(roadOne.endPos) >= 0.05f)
                    continue;

                roadPositions[i] = roadOne;


           //     RoadPosition subSextionTwoStart = new RoadPosition() { startPos = roadTwo.startPos, endPos = roadOne.endPos };
         //       RoadPosition subSextionTwoEnd = new RoadPosition() { startPos = roadOne.endPos, endPos = roadTwo.endPos };


                //   roadPositions.RemoveAt(j);
                //   roadPositions.Add(subSextionTwoEnd);
                break;
            }
        }


        //Splitting roads
        for (int i = roadPositions.Count - 1; i >= 0; i--)
        {
            RoadPosition roadOne = roadPositions[i];
            Vector3 endPosRoad = roadOne.endPos;

            for (int j = roadPositions.Count - 1; j >= 0; j--)
            {
                RoadPosition roadTwo = roadPositions[j];
                if (roadOne == roadTwo)
                    continue;

                if (roadTwo.GetClosestPointToLine(endPosRoad) >= 0.05f)
                    continue;

                //Roads don't need splitting
                if (roadOne.endPos == roadTwo.endPos || roadOne.endPos == roadTwo.startPos)
                    continue;

                RoadPosition subSextionTwoStart = new RoadPosition() { startPos = roadTwo.startPos, endPos = roadOne.endPos };
               RoadPosition subSextionTwoEnd = new RoadPosition() { startPos = roadOne.endPos, endPos = roadTwo.endPos };

                Debug.LogError("spltitting");

                roadPositions.RemoveAt(j);
                roadPositions.Add(subSextionTwoStart);
                roadPositions.Add(subSextionTwoEnd);

                break;
            }
        }





        //Remove short deadend roads
        for (int i = roadPositions.Count - 1; i >= 0; i--)
        {

            RoadPosition roadPosition = roadPositions[i];

            if (i == 15)
            {
                Debug.LogWarning("_______________");
                Debug.LogWarning(roadPosition.startPos);
                Debug.LogWarning(roadPosition.endPos);
                Debug.LogWarning("_______________");

            }
            if (roadPosition.GetDistance() > minDistance)
                continue;
            Debug.Log("_______________");
            Debug.Log(roadPosition.startPos);
            Debug.Log(roadPosition.endPos);
            Debug.Log("_______________");
                Debug.LogWarning(roadPosition.GetDistance());

            bool test = false;
            foreach (RoadPosition secondRoadPosition in roadPositions)
            {
                if (secondRoadPosition == roadPosition)
                    continue;

                if ((roadPosition.endPos == secondRoadPosition.startPos || roadPosition.endPos == secondRoadPosition.endPos) && (roadPosition.startPos == secondRoadPosition.endPos || roadPosition.startPos == secondRoadPosition.startPos))
                {
                    Debug.Log(roadPosition.endPos);
                    Debug.Log(secondRoadPosition.startPos);
                    Debug.Log(roadPosition.endPos == secondRoadPosition.startPos);
                    Debug.Log(roadPosition.endPos == secondRoadPosition.endPos);
                    test = true;
                    break;
                }

            }

            if (!test)
            {
                Debug.Log("test");
                roadPositions.RemoveAt(i);

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
                    dirFirst = dirFirst.normalized.Abs();
                    indexOne = j;
                    continue;
                }
                else if (dirSecond == Vector3.zero)
                {
                    dirSecond = roadPosition.endPos - roadPosition.startPos;
                    dirSecond = dirSecond.normalized.Abs();
                    indexTwo = j;
                    continue;
                }

                moreThanTwoConnections = true;
                break;
            }

            if (moreThanTwoConnections || dirFirst == Vector3.zero)
                continue;

            

            if(dirFirst == dirSecond)
            {


                RoadPosition rd = roadPositions[indexOne];
                rd.startPos = roadPositions[indexTwo].startPos;
                roadPositions[indexOne] = rd;
                Debug.Log(roadPositions[indexOne].startPos + " : " + roadPositions[indexOne].endPos);
                roadPositions.RemoveAt(indexTwo);

            }
        }

        CheckRoads();

        CreateCrossRoads();

        return;

        foreach(CrossRoadData cross in crossRoadData)
        {
            var obj = Instantiate(test, cross.Position, Quaternion.identity);
            obj.transform.SetParent(parent.transform, true);
        }

    }

    public void CreateCrossRoads()
    {
        crossRoadPairs = new Dictionary<Vector3, CrossRoadData>();

        foreach(RoadPosition roadPosition in roadPositions)
        {

            Vector3 positionCrossRoad = roadPosition.endPos;

            Directions direction = roadPosition.GetDirectionEnum();

            Directions crossRoadDirection;

            //If road goes up, road enters crossRoad down (switch all to opposite sides)
            switch (direction)
            {
                case Directions.Up:
                    crossRoadDirection = Directions.Down;
                    break;
                case Directions.Right:
                    crossRoadDirection = Directions.Left;
                    break;
                case Directions.Down:
                    crossRoadDirection = Directions.Up;
                    break;
                case Directions.Left:
                    crossRoadDirection = Directions.Right;
                    break;
                default:
                    crossRoadDirection = Directions.None;
                    break;
            }

            if (crossRoadPairs.TryGetValue(positionCrossRoad, out CrossRoadData data))
            {
                data.AddDirection(crossRoadDirection);
            }
            else
            {
                CrossRoadData crossRoadData = new(positionCrossRoad, crossRoadDirection);

                crossRoadPairs.Add(positionCrossRoad, crossRoadData);
            }




            Vector3 startPosRoad = roadPosition.startPos;

            if (crossRoadPairs.TryGetValue(startPosRoad, out CrossRoadData data2))
            {
                data2.AddDirection(direction);
            }
            else
            {
                CrossRoadData crossRoadData = new(startPosRoad, direction);

                crossRoadPairs.Add(startPosRoad, crossRoadData);
            }

            //Get end point
            //Get direction (get vector see which side)
            //Check if in dictionary
            //If is
            //Add direction to existing crossRoad
            //If not
            //Create new crossRoad with direction
        }



        crossRoadData.Clear();

        foreach(KeyValuePair<Vector3, CrossRoadData> data in crossRoadPairs)
        {
            crossRoadData.Add(data.Value);
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
            if (!pos.TryDistanceParralel(out float distance, roadPosition))
                continue;

            

            if (distance <= minDistance)
            {
                removedRoadPositions.Add(pos);
                amountLeft++;
                return;
            }

            /*
            if (RoadHelpers.LineLineIntersection(out Vector3 intersection, pos, roadPosition))
            {
                if (intersection == pos.startPos)
                    continue;
                pos.endPos = intersection;
                intersects = true;
            }
            */
        }

        roadPositions.Add(pos);

        crossRoadData.Add(new(pos.endPos, Directions.None));

        if (intersects) return;


        if (RandomHelper.Percentage(.66f))
        {
            AddToList(pos.endPos, Quaternion.AngleAxis(90, Vector3.up) * direction);
        }
        if (RandomHelper.Percentage(.66f))
        {
            AddToList(pos.endPos, Quaternion.AngleAxis(-90, Vector3.up) * direction);
        }
        if (RandomHelper.Percentage(.66f))
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


            var dif = roadPosition.endPos - roadPosition.startPos;




            if (RoadHelpers.LineLineFarIntersection(out Vector3 intersection, intersectRoadPosition, roadPosition))
            {
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



        if (crossRoadData == null || crossRoadData.Count == 0)
        {
            return;
        }
        foreach (var crossRoad in crossRoadData)
        {
            Gizmos.DrawCube(crossRoad.Position, Vector3.one * 5);
        }

        return;

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

        return;

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

    private Vector3 direction;

    private Ray roadRay;

    public Ray GetRay()
    {
        if (roadRay.direction != Vector3.zero)
            return roadRay;

        roadRay = new();
        roadRay.origin = startPos;
        roadRay.direction = endPos - startPos;
        return roadRay;
    }

    public override string ToString()
    {
        return $"StartPos: {startPos}, Endpos: {endPos}";
    }

    public float GetClosestPointToLine(Vector3 point)
    {
        return HandleUtility.DistancePointLine(point, startPos, endPos);
    }

    public bool IsParralel(RoadPosition otherRoad)
    {
        return (this.GetDirection().normalized.Abs() == otherRoad.GetDirection().normalized.Abs());
    }

    public bool TryDistanceParralel(out float distance, RoadPosition otherRoad)
    {
        if (!this.IsParralel(otherRoad))
        {
            distance = float.MaxValue;
            return false;
        }

       

        if (GetDirection().normalized.Abs() == Vector3.right)
        {
            //Check if line is over eachother
            if ((otherRoad.startPos.x >= this.startPos.x && otherRoad.startPos.x < this.endPos.x) ||
                (otherRoad.endPos.x >= this.startPos.x && otherRoad.endPos.x < this.endPos.x) ||
                (this.startPos.x >= otherRoad.startPos.x && this.startPos.x < otherRoad.endPos.x) ||
                (this.endPos.x >= otherRoad.startPos.x && this.endPos.x < otherRoad.endPos.x))
            {
                distance = Mathf.Abs(otherRoad.endPos.x - this.endPos.x);
                return true;
            }
        }
        else
        {
            //Check if line is over eachother
            if ((otherRoad.startPos.z >= this.startPos.z && otherRoad.startPos.z < this.endPos.z) ||
                (otherRoad.endPos.z >= this.startPos.z && otherRoad.endPos.z < this.endPos.z) ||
                (this.startPos.z >= otherRoad.startPos.z && this.startPos.z < otherRoad.endPos.z) ||
                (this.endPos.z >= otherRoad.startPos.z && this.endPos.z < otherRoad.endPos.z))
            {
                distance = Mathf.Abs(otherRoad.endPos.z - this.endPos.z);
                return true;
            }
        }

        distance = float.MaxValue;
        return false;
       

    }

        public Vector3 GetDirection()
        {
            if (direction != Vector3.zero)
                return direction;

            direction = endPos - startPos;
            return direction;
        }

    public Directions GetDirectionEnum()
    {
        Vector3 direction = GetDirection();

        if (direction == Vector3.forward)
        {
            return Directions.Up;
        }
        else if (direction == Vector3.right)
        {
            return Directions.Right;
        }
        else if (direction == Vector3.left)
        {
            return Directions.Left;
        }
        else if (direction == Vector3.back)
        {
            return Directions.Down;
        }

        return Directions.None;

    }

        public Vector3 GetNormalizedDirection()
        {
            return GetDirection().normalized;
        }

    public float GetDistance()
    {
        if (direction != Vector3.zero)
            return direction.magnitude;

        direction = endPos - startPos;
        return direction.magnitude;
    }

    public static bool operator ==(RoadPosition rp1, RoadPosition rp2)
    {
        return (rp1.startPos == rp2.startPos && rp1.endPos == rp2.endPos);
    }

    public static bool operator !=(RoadPosition rp1, RoadPosition rp2)
    {
        return !(rp1 == rp2);
    }

}

public struct RoadSave
{
    public Vector3 startPos;
    public Vector3 direction;
}
