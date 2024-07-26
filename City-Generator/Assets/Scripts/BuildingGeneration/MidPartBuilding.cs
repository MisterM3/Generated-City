using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidPartBuilding : MonoBehaviour, IGenerateBuildingPart
{
    private float width = 5f;
    private float height = 4f;
    private float lenght = 5f;

    public GameObject test;



    public void SetDimensions(Vector3 dimensions)
    {
        width = dimensions.x;
        height = dimensions.y;
        lenght = dimensions.z;
    }

    public void RandomizeIndentation()
    {
        // isIndented;
    }

    public GameObject GenerateBuildingPart()
    {
        GameObject buildingPart = Instantiate(test);

        buildingPart.transform.localScale = new Vector3(width, height, lenght);
        return buildingPart;
    }

    public void RandomizeValues()
    {
        throw new System.NotImplementedException();
    }
}
