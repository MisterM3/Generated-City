using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BottomPartBuilding : MonoBehaviour, IGenerateBuildingPart
{

    private float width = 5f;
    private float height = 4f;
    private float lenght = 5f;


    List<GameObject> bottomPartPrefabs;

    public GameObject test;


    public IndentShape indentShape = IndentShape.None;

    [SerializeField] private FlatIndent indentValues;

    private bool isIndented = false;

    private SlopeSide slopeSide = SlopeSide.Left;

    private float maxIndentation = 2.5f;
    private float maxHeightOffsetIndentation = 2.5f;

    private float indentation = 0;
    private float heightOffset = 0;

    [SerializeField] private List<Color> difColors;
    [SerializeField] private List<Material> difMaterials;


    public GameObject slipPrefab;
    public GameObject cornerPrefab;
    private bool hasSlip;

    [SerializeField, Range(30, 60)] private float angle = 45f;


    public void SetDimensions(Vector3 dimensions)
    {
        width = dimensions.x;
        height = dimensions.y;
        lenght = dimensions.z;
    }

    public void RandomizeValues()
    {
        indentShape = CenteralizedRandom.RandomizeEnum<IndentShape>();
        slopeSide = CenteralizedRandom.RandomizeEnum<SlopeSide>();
        if (indentValues == null)
            indentValues = new();
        indentValues.RandomizeInit(maxIndentation);

        indentation = CenteralizedRandom.Range(0, maxIndentation);
        heightOffset = CenteralizedRandom.Range(0, maxHeightOffsetIndentation);
    }

    public GameObject GenerateBuildingPart()
    {
        GameObject buildingPart = new() { name = "Building" };

        indentShape = CenteralizedRandom.RandomizeEnum<IndentShape>();

        switch (indentShape)
        {
            case IndentShape.None:
                MakeNormal(buildingPart.transform);
                break;
            case IndentShape.Flat:
                buildingPart = MakeFlat(buildingPart);
                break;
            case IndentShape.Slope:
                buildingPart = MakeSlope(buildingPart);
                break;
        }



        return buildingPart;
    }

    public void MakeNormal(Transform parent)
    {
        MakeBuilding(new Vector3(width, height, lenght), parent);
    }


    private void MakeBuilding(Vector3 size, Transform parent)
    {
        width = size.x;
        height = size.y;
        lenght = size.z;

        GameObject left = Instantiate(test, parent);
        left.transform.position = new Vector3(-width / 2, 0, 0);
        left.transform.localScale = new Vector3(height / 2, 1, lenght / 2);
        GameObject right = Instantiate(test, parent);
        right.transform.position = new Vector3(width / 2, 0, 0);
        right.transform.rotation = Quaternion.Euler(0, 180, -90);
        right.transform.localScale = new Vector3(height / 2, 1, lenght / 2);
        GameObject forward = Instantiate(test, parent);
        forward.transform.position = new Vector3(0, 0, -lenght / 2);
        forward.transform.rotation = Quaternion.Euler(0, -90, -90);
        forward.transform.localScale = new Vector3(height / 2, 1, width / 2);
        GameObject back = Instantiate(test, parent);
        back.transform.position = new Vector3(0, 0, lenght / 2);
        back.transform.rotation = Quaternion.Euler(0, 90, -90);
        back.transform.localScale = new Vector3(height / 2, 1, width / 2);
    }


    
    [System.Serializable]
    private class FlatIndent
    {
        [Min(0)] public float leftIndent = 0;
        [Min(0)] public float rightIndent = 0;
        [Min(0)] public float forwardIndent = 0;
        [Min(0)] public float backwardsIndent = 0;

        public void RandomizeInit(float maxIndentPerSide)
        {
            leftIndent = CenteralizedRandom.Range(0, maxIndentPerSide);
            rightIndent = CenteralizedRandom.Range(0, maxIndentPerSide);
            forwardIndent = CenteralizedRandom.Range(0, maxIndentPerSide);
            backwardsIndent = CenteralizedRandom.Range(0, maxIndentPerSide);
        }

    }

    public GameObject MakeFlat(GameObject go)
    {

        float objectWidth = width - indentValues.leftIndent - indentValues.rightIndent;
        float objectLenght = lenght - indentValues.forwardIndent - indentValues.backwardsIndent;

        float xPos = -indentValues.leftIndent + indentValues.rightIndent;
        float zPos = -indentValues.forwardIndent + indentValues.backwardsIndent;

        MakeBuilding(new Vector3(objectWidth, height, objectLenght), go.transform);
        go.transform.localPosition = new Vector3(xPos / 2, go.transform.localPosition.y, zPos / 2);

        return go;
    }


    private enum SlopeSide
    {
        Left,
        Right,
        Foward,
        Back,
        Horizontal,
        Vertical
    }

    public GameObject MakeSlope(GameObject go)
    {


        Vector3 size = Vector3.one;
        Vector3 position = Vector3.zero;


        switch (slopeSide)
        {
            case SlopeSide.Left:
                size = new Vector3(width - indentation, height - heightOffset, lenght);
                position = new Vector3(-indentation / 2, go.transform.localPosition.y, go.transform.localPosition.z);
                break;
            case SlopeSide.Right:
                size = new Vector3(width - indentation, height - heightOffset, lenght);
                position = new Vector3(indentation / 2, go.transform.localPosition.y, go.transform.localPosition.z);
                break;
            case SlopeSide.Foward:
                size = new Vector3(width, height - heightOffset, lenght - indentation);
                position = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, -indentation / 2);
                break;
            case SlopeSide.Back:
                size = new Vector3(width, height - heightOffset, lenght - indentation);
                position = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, indentation / 2);
                break;
            case SlopeSide.Horizontal:
                size = new Vector3(width - (indentation * 2), height - heightOffset, lenght);
                break;
            case SlopeSide.Vertical:
                size = new Vector3(width, height - heightOffset, lenght - (indentation * 2));
                break;
        }

        MakeBuilding(size, go.transform);
        go.transform.position = position;
        return go;
    }
}




public enum IndentShape
{
    None,
    Flat,
    Slope,
}
