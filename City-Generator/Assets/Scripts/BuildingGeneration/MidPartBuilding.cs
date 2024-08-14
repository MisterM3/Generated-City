using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MidPartBuilding : MonoBehaviour, IGenerateBuildingPart
{
    private float width = 5f;
    private float height = 4f;
    private float lenght = 5f;

    [SerializeField] private GameObject sidePrefab;

    public void SetDimensions(Vector3 dimensions)
    {
        width = dimensions.x;
        height = dimensions.y;
        lenght = dimensions.z;
    }


    public GameObject GenerateBuildingPart()
    {
        GameObject buildingPart = new("MiddlePart");

        MakeBuilding(new Vector3(width, height, lenght), buildingPart.transform);

        for (int i = buildingPart.transform.childCount - 1; i >= 0; i--)
        {
            if (buildingPart.transform.GetChild(i).TryGetComponent<AdsOnBuilding>(out AdsOnBuilding ads))
            {
                ads.Place();
            }
        }

        //buildingPart.transform.localScale = new Vector3(width, height, lenght);
        return buildingPart;
    }

    public void RandomizeValues()
    {
        throw new System.NotImplementedException();
    }

    private void MakeBuilding(Vector3 size, Transform parent)
    {
        width = size.x;
        height = size.y;
        lenght = size.z;

        GameObject left = Instantiate(sidePrefab, parent);
        left.transform.position = new Vector3(-width / 2, 0, 0);
        left.transform.localScale = new Vector3(height / 2, 1, lenght / 2);
        GameObject right = Instantiate(sidePrefab, parent);
        right.transform.position = new Vector3(width / 2, 0, 0);
        right.transform.rotation = Quaternion.Euler(0, 180, -90);
        right.transform.localScale = new Vector3(height / 2, 1, lenght / 2);
        GameObject forward = Instantiate(sidePrefab, parent);
        forward.transform.position = new Vector3(0, 0, -lenght / 2);
        forward.transform.rotation = Quaternion.Euler(0, -90, -90);
        forward.transform.localScale = new Vector3(height / 2, 1, width / 2);
        GameObject back = Instantiate(sidePrefab, parent);
        back.transform.position = new Vector3(0, 0, lenght / 2);
        back.transform.rotation = Quaternion.Euler(0, 90, -90);
        back.transform.localScale = new Vector3(height / 2, 1, width / 2);
    }
}
