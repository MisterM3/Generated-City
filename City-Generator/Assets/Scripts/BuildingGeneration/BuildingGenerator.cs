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
    [SerializeField] TopPartBuilding generateTop;

    [SerializeField] FixTilingChildren fixTiling;


    [SerializeField] ColorEnum color;
    private MaterialsBuilding currentMaterials;

    [SerializeField] List<MaterialsBuilding> buildingMaterials = new();


    [System.Serializable]
    private enum ColorEnum
    {
        Beige,
        DarkBlue,
        DarkGray,
    }

    [System.Serializable]
    private struct MaterialsBuilding
    {
        public ColorEnum color;

        public List<Material> bottomMaterial;

        public List<Material> middleMaterial;

        public Material topMaterial;
    }


    public void SetupBuilding(Vector3 scale)
    {
        _widthBuilding = scale.x;
        _heightBuilding = scale.y;
        _lenghtBuilding = scale.z;

        color = CenteralizedRandom.RandomizeEnum<ColorEnum>();

        foreach(MaterialsBuilding matBuilding in buildingMaterials)
        {
            if (matBuilding.color == color)
            {
                currentMaterials = matBuilding;
                break;
            }
        }

    }



    private void RecursiveSetMaterialBottom(Transform tf, Material mat)
    {
        
        if (tf.TryGetComponent<Renderer>(out Renderer rend))
        {
            if (tf.tag != "NoTiling")
                rend.sharedMaterial = mat;
        }

        for(int i = 0; i != tf.childCount; i++)
        {
            RecursiveSetMaterialBottom(tf.GetChild(i), mat);
        }
    }

    private void RecursiveSetMaterialDifferntMid(Transform tf)
    {

        Material mat = currentMaterials.middleMaterial.RandomItem();

        if (tf.TryGetComponent<Renderer>(out Renderer rend))
        {
            if (tf.tag != "NoTiling")
            rend.sharedMaterial = mat;
        }

        for (int i = 0; i != tf.childCount; i++)
        {
            RecursiveSetMaterialDifferntMid(tf.GetChild(i));
        }
    }

    public void GenerateBuilding()
    {
        this.transform.RemoveAllChildren();


        if (generateBottom == null)
            return;

        foreach (MaterialsBuilding matBuilding in buildingMaterials)
        {
            if (matBuilding.color == color)
            {
                currentMaterials = matBuilding;
                break;
            }
        }


        generateBottom.SetDimensions(new Vector3(_widthBuilding, _heightBuilding / 3f, _lenghtBuilding));
        generateBottom.RandomizeValues();
        GameObject buildingBottom = generateBottom.GenerateBuildingPart();

        RecursiveSetMaterialBottom(buildingBottom.transform, currentMaterials.bottomMaterial.RandomItem());

        buildingBottom.transform.SetParent(this.transform, false);

        if (generateMiddle == null)
            return;

        generateMiddle.SetDimensions(new Vector3(_widthBuilding, (_heightBuilding / 3f) * 2, _lenghtBuilding));
        generateMiddle.RandomizeIndentation();
        GameObject buildingMiddle = generateMiddle.GenerateBuildingPart();

        RecursiveSetMaterialDifferntMid(buildingMiddle.transform);

        buildingMiddle.transform.SetParent(this.transform, false);
        buildingMiddle.transform.position = new Vector3(buildingMiddle.transform.position.x, buildingBottom.transform.position.y + _heightBuilding / 3f, buildingMiddle.transform.position.z);

        generateTop.SetDimensions(new Vector3(_widthBuilding, 1, _lenghtBuilding));
        GameObject buildingTop = generateTop.GenerateBuildingPart();
        buildingTop.transform.SetParent(this.transform, false);
        buildingTop.transform.position = new Vector3(buildingTop.transform.position.x,_heightBuilding, buildingTop.transform.position.z);

        if (buildingTop.TryGetComponent<Renderer>(out Renderer rend))
        {
            rend.sharedMaterial = currentMaterials.topMaterial;
        }

        fixTiling.FixTilingChilds();


    }

    public void GenerateBuildingNotRandom()
    {
        this.transform.RemoveAllChildren();


        if (generateBottom == null)
            return;

        foreach (MaterialsBuilding matBuilding in buildingMaterials)
        {
            if (matBuilding.color == color)
            {
                currentMaterials = matBuilding;
                break;
            }
        }


        generateBottom.SetDimensions(new Vector3(_widthBuilding, _heightBuilding / 3f, _lenghtBuilding));
        GameObject buildingBottom = generateBottom.GenerateBuildingPart();

        RecursiveSetMaterialBottom(buildingBottom.transform, currentMaterials.bottomMaterial.RandomItem());

        buildingBottom.transform.SetParent(this.transform, false);

        if (generateMiddle == null)
            return;

        generateMiddle.SetDimensions(new Vector3(_widthBuilding, (_heightBuilding / 3f) * 2, _lenghtBuilding));
        generateMiddle.RandomizeIndentation();
        GameObject buildingMiddle = generateMiddle.GenerateBuildingPart();

        RecursiveSetMaterialDifferntMid(buildingMiddle.transform);

        buildingMiddle.transform.SetParent(this.transform, false);
        buildingMiddle.transform.position = new Vector3(buildingMiddle.transform.position.x, buildingBottom.transform.position.y + _heightBuilding / 3f, buildingMiddle.transform.position.z);

        generateTop.SetDimensions(new Vector3(_widthBuilding, 1, _lenghtBuilding));
        GameObject buildingTop = generateTop.GenerateBuildingPart();
        buildingTop.transform.SetParent(this.transform, false);
        buildingTop.transform.position = new Vector3(buildingTop.transform.position.x, _heightBuilding, buildingTop.transform.position.z);

        if (buildingTop.TryGetComponent<Renderer>(out Renderer rend))
        {
            rend.sharedMaterial = currentMaterials.topMaterial;
        }

        fixTiling.FixTilingChilds();

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
