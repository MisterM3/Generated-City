using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SquareBuilding", menuName = "ScriptableObjects/BuildingParts/SquareBuilding", order = 1)]
public class SquareBuildings : PartStrategy
{

    public override GameObject MakeBuildingPart(Vector3 size)
    {

        GameObject parent = new("BottomBuilding");
        Transform parentTf = parent.transform;

        float width = size.x;
        float height = size.y;
        float lenght = size.z;

        GameObject left = Instantiate(sidePrefab, parentTf);
        left.transform.position = new Vector3(-width / 2, 0, 0);
        left.transform.localScale = new Vector3(height / 2, 1, lenght / 2);
        GameObject right = Instantiate(sidePrefab, parentTf);
        right.transform.position = new Vector3(width / 2, 0, 0);
        right.transform.rotation = Quaternion.Euler(0, 180, -90);
        right.transform.localScale = new Vector3(height / 2, 1, lenght / 2);
        GameObject forward = Instantiate(sidePrefab, parentTf);
        forward.transform.position = new Vector3(0, 0, -lenght / 2);
        forward.transform.rotation = Quaternion.Euler(0, -90, -90);
        forward.transform.localScale = new Vector3(height / 2, 1, width / 2);
        GameObject back = Instantiate(sidePrefab, parentTf);
        back.transform.position = new Vector3(0, 0, lenght / 2);
        back.transform.rotation = Quaternion.Euler(0, 90, -90);
        back.transform.localScale = new Vector3(height / 2, 1, width / 2);

        return parent;
    }


    public override void RandomizeValues()
    {
    }
}
