using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPartBuilding : MonoBehaviour, IGenerateBuildingPart
{

    [SerializeField] GameObject topPrefab;

    private Vector3 size;

    public GameObject GenerateBuildingPart()
    {
        GameObject go = Instantiate(topPrefab);
        go.transform.localScale = new Vector3(size.x/2, size.y, size.z/2);
        return go;   
    }

    public void RandomizeValues()
    {
    }

    public void SetDimensions(Vector3 dimensions)
    {
        size = dimensions;
    }
}
