using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CityGenerator : EditorWindow
{
    
    [MenuItem("Tools/CityGenerator")]
    public static void OpenWindow()
    {
       CreateWindow<CityGenerator>("City Generator");
    }



    [Min(2)]
    public int horizontalLines;
    [Min(2)]
    public int verticalLines;
    [Min(1)]
    public float sizeRoad;
    [Min(1)]
    public float widthPlot;
    [Min(1)]
    public float heightPlot;

    private SerializedProperty propHorizontalLines;
    private SerializedProperty propVerticalLines;
    private SerializedProperty propWidthRoad;
    private SerializedProperty propWidthPlot;
    private SerializedProperty propHeightPlot;


    public Transform parentObject;
    private SerializedObject so;
    private SerializedProperty propParentObject;


    
    public float minHeight;
    public float maxHeight;
    public float sizeFromStreet;

    public float maxBuildingDeviation;
    public int seed;
    public bool randomSeed;



    private SerializedProperty propMinHeight;
    private SerializedProperty propMaxHeight;
    private SerializedProperty propSizeFromStreet;
    private SerializedProperty propMaxStreetDeviation;



    private void OnEnable()
    {
        SetupSerializedObject();
        SceneView.duringSceneGui += DrawPreview;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DrawPreview;
    }
    private void SetupSerializedObject()
    {
        so = new SerializedObject(this);
        propParentObject = so.FindProperty(nameof(parentObject));
        propHorizontalLines = so.FindProperty(nameof(horizontalLines));
        propVerticalLines = so.FindProperty(nameof(verticalLines));
        propWidthRoad = so.FindProperty(nameof(sizeRoad));
        propWidthPlot = so.FindProperty(nameof(widthPlot));
        propHeightPlot = so.FindProperty(nameof(heightPlot));

        propMinHeight = so.FindProperty(nameof(minHeight));
        propMaxHeight = so.FindProperty(nameof(maxHeight));
        propSizeFromStreet = so.FindProperty(nameof(sizeFromStreet));
        propMaxStreetDeviation = so.FindProperty(nameof(maxBuildingDeviation));
    }

    private void DrawPreview(SceneView sceneView)
    {
        if (parentObject == null)
            return;

        DrawRoads();
        DrawHouses();

    }


    private void DrawHouses()
    {
        float totalWidth = verticalLines * sizeRoad + (verticalLines - 1) * widthPlot;
        float halfWidth = totalWidth / 2f;
        float totalHeight = horizontalLines * sizeRoad + (horizontalLines - 1) * heightPlot;
        float halfHeight = totalHeight / 2f;

        float incrementWidth = sizeRoad + (widthPlot);
        float incrementHeight = sizeRoad + heightPlot;

        Vector3 center = parentObject.position;


        float medHeight = maxHeight - ((maxHeight - minHeight) / 2f);

        Vector3 minSize = new Vector3(widthPlot - (sizeFromStreet * 2f) - maxBuildingDeviation, minHeight, heightPlot - (sizeFromStreet * 2f) - maxBuildingDeviation);
        Vector3 medSize = new Vector3(widthPlot - (sizeFromStreet * 2f), medHeight, heightPlot - (sizeFromStreet * 2f));
        Vector3 maxSize = new Vector3(widthPlot - (sizeFromStreet * 2f) + maxBuildingDeviation, maxHeight, heightPlot - (sizeFromStreet * 2f) + maxBuildingDeviation);


        //everything - 1 as there is 1 plot line less than there are roads
        for (int i = 0; i < horizontalLines - 1; i++)
        {
            for (int j = 0; j < verticalLines - 1; j++)
            {
                float xCoord = center.x + sizeRoad + (widthPlot / 2f) - halfWidth + incrementWidth * j;
                float zCoord = center.z + sizeRoad + (heightPlot / 2f) - halfHeight + incrementHeight * i;

                Vector3 minLine = new Vector3(xCoord, center.y + (minHeight / 2f), zCoord);
                Vector3 centerLine = new Vector3(xCoord, center.y + (medHeight / 2f), zCoord);
                Vector3 maxLine = new Vector3(xCoord, center.y + (maxHeight / 2f), zCoord);

                Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
                
                Handles.color = Color.red;
                Handles.DrawWireCube(minLine, minSize);
                Handles.color = Color.yellow;
                Handles.DrawWireCube(centerLine, medSize);
                Handles.color = Color.green;
                Handles.DrawWireCube(maxLine, maxSize);
                Handles.color = Color.white;

            }
        }

    }

    private void DrawRoads()
    {
        float totalWidth = verticalLines * sizeRoad + (verticalLines - 1) * widthPlot;
        float halfWidth = totalWidth / 2f;
        float totalHeight = horizontalLines * sizeRoad + (horizontalLines - 1) * heightPlot;
        float halfHeight = totalHeight / 2f;

        Vector3 center = parentObject.position;


        for (int i = 0; i < horizontalLines; i++)
        {

            float zCoord = center.z - halfHeight + (sizeRoad / 2f) + i * (sizeRoad + heightPlot);
            Vector3 centerLine = new Vector3(center.x, center.y, zCoord);

            Vector3 size = new Vector3(totalWidth, 0.1f, sizeRoad);
            Handles.DrawWireCube(centerLine, size);
        }

        for (int i = 0; i < verticalLines; i++)
        {

            float xCoord = center.x - halfWidth + (sizeRoad / 2f) + i * (sizeRoad + widthPlot);
            Vector3 centerLine = new Vector3(xCoord, center.y, center.z);

            Vector3 size = new Vector3(sizeRoad, 0.1f, totalHeight);
            Handles.DrawWireCube(centerLine, size);
        }
    }

    public void OnGUI()
    {
        so.Update();
        EditorGUILayout.PropertyField(propParentObject);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(propHorizontalLines);
        EditorGUILayout.PropertyField(propVerticalLines);
        EditorGUILayout.PropertyField(propWidthRoad);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(propWidthPlot);
        EditorGUILayout.PropertyField(propHeightPlot);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(propMinHeight);
        EditorGUILayout.PropertyField(propMaxHeight);
        EditorGUILayout.PropertyField(propSizeFromStreet);
        EditorGUILayout.PropertyField(propMaxStreetDeviation);


        if (so.ApplyModifiedProperties())
        {
            SceneView.RepaintAll();
        }
    }
}
