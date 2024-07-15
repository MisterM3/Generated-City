using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MakeCrossRoadsVisuals : MonoBehaviour
{


    [SerializeField] GameObject _fourway;
    [SerializeField] GameObject _threeWay;
    [SerializeField] GameObject _corner;
    [SerializeField] GameObject _end;

    [SerializeField] Transform _parent;


    public void MakeCrossRoad(CrossRoadData data)
    {
        int directionsCount = data.Directions.Count;


        switch(directionsCount)
        {
            case 1:
                MakeEnd(data);
                break;
            case 2:
                MakeCorner(data);
                break;
            case 3:
                MakeThreeWay(data);
                break;
            case 4:
                MakeFourWay(data);
                break;
            default:
                Logger.Log("Wrong amount of directions!");
                break;
        }
    }


    private void MakeThreeWay(CrossRoadData data)
    {
        var directions = data.Directions;

        Quaternion directionObject = Quaternion.AngleAxis(90, Vector3.up);

        if (directions.Contains(Directions.Up) && !directions.Contains(Directions.Down))
        {
            directionObject = Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (directions.Contains(Directions.Right) && !directions.Contains(Directions.Left))
        {
            directionObject = Quaternion.AngleAxis(180, Vector3.up);

        }
        else if (directions.Contains(Directions.Down) && !directions.Contains(Directions.Up))
        {
            directionObject = Quaternion.AngleAxis(270, Vector3.up);
        }
        else if (directions.Contains(Directions.Left) && !directions.Contains(Directions.Right))
        {
            directionObject = Quaternion.AngleAxis(0, Vector3.up);
        }

        InstantiateCrossRoad(_threeWay, data.Position, directionObject);

    }


    private void InstantiateCrossRoad(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject roadObject = Instantiate(prefab, position, rotation);
        Transform roadTransform = roadObject.transform;
        roadTransform.SetParent(_parent);
        var flags = GameObjectUtility.GetStaticEditorFlags(_parent.gameObject);
        GameObjectUtility.SetStaticEditorFlags(roadObject, flags);
       // roadObject.hideFlags = HideFlags.HideInHierarchy;
    }

    private void MakeFourWay(CrossRoadData data)
    {
        InstantiateCrossRoad(_fourway, data.Position, Quaternion.identity);
    }


    private void MakeCorner(CrossRoadData data)
    {
        var directions = data.Directions;

        Quaternion directionObject = Quaternion.AngleAxis(90, Vector3.up);

        if (directions.Contains(Directions.Up))
        {
            if (directions.Contains(Directions.Left))
            {
                directionObject = Quaternion.AngleAxis(-90, Vector3.up);
            }
            else if (directions.Contains(Directions.Right))
            {
                directionObject = Quaternion.AngleAxis(0, Vector3.up);
            }
        }
        else if (directions.Contains(Directions.Down))
        {
            if (directions.Contains(Directions.Left))
            {
                directionObject = Quaternion.AngleAxis(180, Vector3.up);
            }
            else if (directions.Contains(Directions.Right))
            {
                directionObject = Quaternion.AngleAxis(90, Vector3.up);
            }
        }

        InstantiateCrossRoad(_corner, data.Position, directionObject);
    }

    private void MakeEnd(CrossRoadData data)
    {
        var directions = data.Directions;

        Quaternion directionObject = Quaternion.AngleAxis(0, Vector3.up);

        if (directions.Contains(Directions.Up))
        {
            directionObject = Quaternion.AngleAxis(0, Vector3.up);
        }
        else if (directions.Contains(Directions.Right))
        {
            directionObject = Quaternion.AngleAxis(90, Vector3.up);

        }
        else if (directions.Contains(Directions.Down))
        {
            directionObject = Quaternion.AngleAxis(180, Vector3.up);
        }
        else if (directions.Contains(Directions.Left))
        {
            directionObject = Quaternion.AngleAxis(-90, Vector3.up);
        }

        InstantiateCrossRoad(_end, data.Position, directionObject);
    }
}
