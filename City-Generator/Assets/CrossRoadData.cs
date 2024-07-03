using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrossRoadData 
{
    Vector3 _position;
    public Vector3 Position => _position;
    List<Vector3> _directions;

    public CrossRoadData(Vector3 position, List<Vector3> directions = null)
    {
        _position = position;
        _directions = directions;
    }

    public CrossRoadData(Vector3 position, Vector3 direction)
    {
        _position = position;
        AddDirection(direction);
    }

    public void AddDirection(Vector3 direction)
    {
        if (_directions == null)
        {
            _directions = new List<Vector3>();
        }

        _directions.Add(direction);
    }

    public void RemoveDirection(Vector3 direction)
    {
        if (_directions == null)
        {
            _directions = new List<Vector3>();
            Logger.Log("Direction could not be removed on crossroad, it was not in list");
            return;
        }

        _directions.Add(direction);
    }

}
