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
        GameObject buildingPart = Instantiate(test);

        indentShape = CenteralizedRandom.RandomizeEnum<IndentShape>();

        switch (indentShape)
        {
            case IndentShape.None:
                buildingPart = MakeNormal(buildingPart);
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

    public GameObject MakeNormal(GameObject go)
    {
        go.transform.localScale = new Vector3(width, height, lenght);
        return go;
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


        go.transform.localScale = new Vector3(objectWidth, height, objectLenght);
        go.transform.localPosition = new Vector3(xPos, go.transform.localPosition.y, zPos);

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



        switch (slopeSide)
        {
            case SlopeSide.Left:
                go.transform.localScale = new Vector3(width - indentation, height - heightOffset, lenght);
                go.transform.localPosition = new Vector3(-indentation, go.transform.localPosition.y, go.transform.localPosition.z);
                break;
            case SlopeSide.Right:
                go.transform.localScale = new Vector3(width - indentation, height - heightOffset, lenght);
                go.transform.localPosition = new Vector3(indentation, go.transform.localPosition.y, go.transform.localPosition.z);
                break;
            case SlopeSide.Foward:
                go.transform.localScale = new Vector3(width, height - heightOffset, lenght - indentation);
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, -indentation);
                break;
            case SlopeSide.Back:
                go.transform.localScale = new Vector3(width, height - heightOffset, lenght - indentation);
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, indentation);
                break;
            case SlopeSide.Horizontal:
                go.transform.localScale = new Vector3(width - (indentation * 2), height - heightOffset, lenght);
                break;
            case SlopeSide.Vertical:
                go.transform.localScale = new Vector3(width, height - heightOffset, lenght - (indentation * 2));
                break;
        }

        return go;
    }
}




public enum IndentShape
{
    None,
    Flat,
    Slope,
}
