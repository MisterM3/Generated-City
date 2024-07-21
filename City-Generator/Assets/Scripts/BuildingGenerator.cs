using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    [SerializeField] private float _widthBuilding = 5f;
    [SerializeField] private float _heightBuilding = 5f;
    [SerializeField] private float _lenghtBuilding = 5f;
    [SerializeField] GameObject go;



    public void SetupBuilding(Vector3 scale)
    {
        _widthBuilding = scale.x;
        _heightBuilding = scale.y;
        _lenghtBuilding = scale.z;
    }

    public void GenerateBuilding()
    {
        this.transform.RemoveAllChildren();

        GameObject building = Instantiate(go, this.transform);
        building.transform.localScale = new Vector3(_widthBuilding, _heightBuilding, _lenghtBuilding);

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
