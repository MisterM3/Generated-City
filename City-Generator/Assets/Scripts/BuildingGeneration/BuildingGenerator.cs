using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField] private float _widthBuilding = 5f;
    [SerializeField] private float _heightBuilding = 5f;
    [SerializeField] private float _lenghtBuilding = 5f;
    [SerializeField] GameObject go;

    [SerializeField] BottomPartBuilding generateBottom;
    [SerializeField] MidPartBuilding generateMiddle;



    public void SetupBuilding(Vector3 scale)
    {
        _widthBuilding = scale.x;
        _heightBuilding = scale.y;
        _lenghtBuilding = scale.z;
    }

    public void GenerateBuilding()
    {
        this.transform.RemoveAllChildren();


        if (generateBottom == null)
            return;

        generateBottom.SetDimensions(new Vector3(_widthBuilding, _heightBuilding / 3f, _lenghtBuilding));
        generateBottom.RandomizeValues();
        GameObject buildingBottom = generateBottom.GenerateBuildingPart();

        buildingBottom.transform.SetParent(this.transform, false);

        if (generateMiddle == null)
            return;

        generateMiddle.SetDimensions(new Vector3(_widthBuilding, (_heightBuilding / 3f) * 2, _lenghtBuilding));
        generateMiddle.RandomizeIndentation();
        GameObject buildingMiddle = generateMiddle.GenerateBuildingPart();

        buildingMiddle.transform.SetParent(this.transform, false);
        buildingMiddle.transform.position = new Vector3(buildingMiddle.transform.position.x, buildingBottom.transform.position.y + _heightBuilding / 3f, buildingMiddle.transform.position.z); 

        //  GameObject building = Instantiate(go, this.transform);
        //  building.transform.localScale = new Vector3(_widthBuilding, _heightBuilding, _lenghtBuilding);

    }

    public void OnEnable()
    {
        GenerateBuilding();
    }

}


public static class Extensions
{
    public static void RemoveAllChildren(this Transform tf)
    {
        if (tf.childCount == 0)
            return;

        for (int i = tf.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.DestroyImmediate(tf.GetChild(i).gameObject);
        }
    }
}
