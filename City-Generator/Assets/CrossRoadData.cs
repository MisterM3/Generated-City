using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrossRoadData 
{
    [SerializeField] Vector3 _position;
    public Vector3 Position => _position;
    [SerializeField] List<Directions> _directions;

    public CrossRoadData(Vector3 position, List<Directions> directions = null)
    {
        _position = position;
        _directions = directions;
    }

    public CrossRoadData(Vector3 position, Directions direction)
    {
        _position = position;
        AddDirection(direction);
    }

    public void AddDirection(Directions direction)
    {
        if (_directions == null)
        {
            _directions = new List<Directions>();
        }

        if (_directions.Contains(direction))
        {
            Logger.Log("Direction could not be added on crossroad, it was already in list");
            return;
        }

        _directions.Add(direction);
    }

    public void RemoveDirection(Directions direction)
    {
        if (_directions == null)
        {
            _directions = new List<Directions>();
            Logger.Log("Direction could not be removed on crossroad, it was not in list");
            return;
        }

        if (!_directions.Contains(direction))
        {
            Logger.Log("Direction could not be removed on crossroad, it was not in list");
            return;
        }

        _directions.Remove(direction);
    }

}

[System.Serializable]
public enum Directions
{
    None,
    Up,
    Right,
    Down,
    Left,
}
