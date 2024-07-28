using System;
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
    public int seed = 10;
    public bool randomSeed;

    public GameObject meshPlane;
    public GameObject meshCrossRoad;
    public GameObject meshRoad;

    public GameObject buildingObject;


    private SerializedProperty propMeshPlane;
    private SerializedProperty propMeshCrossRoad;
    private SerializedProperty propMeshRoad;

    private SerializedProperty propBuildingObject;

    private SerializedProperty propMinHeight;
    private SerializedProperty propMaxHeight;
    private SerializedProperty propSizeFromStreet;
    private SerializedProperty propMaxStreetDeviation;



    private void OnEnable()
    {
        SetupSerializedObject();
        SceneView.duringSceneGui += DrawPreview;

        CenteralizedRandom.Init(seed);
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

        propMeshPlane = so.FindProperty(nameof(meshPlane));
        propMeshCrossRoad = so.FindProperty(nameof(meshCrossRoad));
        propMeshRoad = so.FindProperty(nameof(meshRoad));
        propBuildingObject = so.FindProperty(nameof(buildingObject));
    }

    private void DrawPreview(SceneView sceneView)
    {
        if (parentObject == null)
            return;
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

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(propMeshPlane);
        EditorGUILayout.PropertyField(propMeshCrossRoad);
        EditorGUILayout.PropertyField(propMeshRoad);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(propBuildingObject);

        if (GUILayout.Button("test"))
        {
            MakeBase();
        }


        if (so.ApplyModifiedProperties())
        {
            SceneView.RepaintAll();
        }
    }


    private void MakeBase()
    {

        RemoveOldObjects();

        GameObject plots = new("Plots");
        plots.transform.SetParent(parentObject, false);

        GameObject crossRoads = new("Crossroad");
        crossRoads.transform.SetParent(parentObject, false);

        GameObject roads = new("Roads");
        roads.transform.SetParent(parentObject, false);

        GameObject buildings = new("Buildings");
        buildings.transform.SetParent(parentObject, false);

        for (int i = 0; i < horizontalLines; i++)
        {
            for (int j = 0; j < verticalLines; j++)
            {
                MakePlot(plots, buildings, i, j);
                MakeCrossRoad(crossRoads, i, j);
                MakeRoads(roads, i, j);
            }
        }

        SetStaticFlags();
    }

    private void SetStaticFlags()
    {
        StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(parentObject.gameObject);
        SetStaticFlagsChildrenRecursive(parentObject, flags);
    }

    private void SetStaticFlagsChildrenRecursive(Transform tf, StaticEditorFlags flags)
    {
        GameObjectUtility.SetStaticEditorFlags(tf.gameObject, flags);

        if (tf.childCount == 0)
            return;
        
        for (int i = tf.childCount - 1; i >= 0; i--)
        {
            Debug.Log(i);
            Debug.Log(parentObject.childCount);
            SetStaticFlagsChildrenRecursive(tf.GetChild(i), flags);
        }
    }

    private void RemoveOldObjects()
    {
        for(int i = parentObject.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parentObject.GetChild(i).gameObject);
        }
    }

    private void MakeCrossRoad(GameObject parent, int indexWidth, int indexHeight)
    {
        float totalWidth = verticalLines * sizeRoad + (verticalLines - 1) * widthPlot;
        float halfWidth = totalWidth / 2f;
        float totalHeight = horizontalLines * sizeRoad + (horizontalLines - 1) * heightPlot;
        float halfHeight = totalHeight / 2f;

        float incrementWidth = sizeRoad + widthPlot;
        float incrementHeight = sizeRoad + heightPlot;

        Vector3 center = parentObject.position;
        Vector3 size = new Vector3(sizeRoad / 2f, 1, sizeRoad / 2f);

        float xCoord = center.x + (sizeRoad/2f) - halfWidth + incrementWidth * indexHeight;
        float zCoord = center.z + (sizeRoad / 2f) - halfHeight + incrementHeight * indexWidth;

        Vector3 centerLine = new Vector3(xCoord, center.y + center.y, zCoord);

        GameObject go = Instantiate(meshCrossRoad, parent.transform);
        go.transform.localScale = size;
        go.transform.position = centerLine;
    }

    private void MakePlot(GameObject plots, GameObject buildings, int indexWidth, int indexHeight)
    {
        if (indexWidth >= horizontalLines - 1)
            return;
        if (indexHeight >= verticalLines - 1)
            return;

        float totalWidth = verticalLines * sizeRoad + (verticalLines - 1) * widthPlot;
        float halfWidth = totalWidth / 2f;
        float totalHeight = horizontalLines * sizeRoad + (horizontalLines - 1) * heightPlot;
        float halfHeight = totalHeight / 2f;

        float incrementWidth = sizeRoad + widthPlot;
        float incrementHeight = sizeRoad + heightPlot;

        Vector3 center = parentObject.position;
        Vector3 size = new Vector3(widthPlot / 2f, 1, heightPlot / 2f);

        float xCoord = center.x + sizeRoad + (widthPlot / 2f) - halfWidth + incrementWidth * indexHeight;
        float zCoord = center.z + sizeRoad + (heightPlot / 2f) - halfHeight + incrementHeight * indexWidth;

        Vector3 centerLine = new Vector3(xCoord, center.y + center.y, zCoord);

        GameObject go = Instantiate(meshPlane, plots.transform);
        go.transform.localScale = size;
        go.transform.position = centerLine;

        MakeBuilding(buildings, centerLine);

    }

    private void MakeBuilding(GameObject parent, Vector3 center)
    {
        GameObject go = Instantiate(buildingObject, parent.transform);
        go.transform.position = center;

        if (!go.TryGetComponent<BuildingGenerator>(out BuildingGenerator buildingGenerator))
            return;


        float width = CenteralizedRandom.Range(widthPlot - (sizeFromStreet * 2f) - (maxBuildingDeviation ), widthPlot - (sizeFromStreet * 2f) + (maxBuildingDeviation ));
        float height = CenteralizedRandom.Range(minHeight, maxHeight);
        float lenght = CenteralizedRandom.Range(heightPlot - (sizeFromStreet * 2f) - (maxBuildingDeviation ), heightPlot - (sizeFromStreet * 2f) + (maxBuildingDeviation ));


        buildingGenerator.SetupBuilding(new Vector3(width, height, lenght));
        buildingGenerator.GenerateBuilding();

    }


    private void MakeRoads(GameObject parent, int indexWidth, int indexHeight)
    {
        if (indexWidth < horizontalLines - 1)
            MakeVerticalRoads(parent, indexWidth, indexHeight);
        if (indexHeight < verticalLines - 1)
         MakeHorizontalRoads(parent, indexWidth, indexHeight);

    }

    private void MakeVerticalRoads(GameObject parent, int indexWidth, int indexHeight)
    {
        float totalWidth = verticalLines * sizeRoad + (verticalLines - 1) * widthPlot;
        float halfWidth = totalWidth / 2f;
        float totalHeight = horizontalLines * sizeRoad + (horizontalLines - 1) * heightPlot;
        float halfHeight = totalHeight / 2f;

        float incrementWidth = sizeRoad + widthPlot;
        float incrementHeight = sizeRoad + heightPlot;

        Vector3 center = parentObject.position;


        Vector3 size = new Vector3((sizeRoad / 2f), 1, (heightPlot / 2f));

        float xCoord = center.x + (sizeRoad / 2f) - halfWidth + incrementWidth * indexHeight;
        float zCoord = center.z + sizeRoad + (heightPlot / 2f) - halfHeight + incrementHeight * indexWidth;

        Vector3 centerLine = new Vector3(xCoord, center.y, zCoord);

        Quaternion rot = Quaternion.identity;

        GameObject go = Instantiate(meshRoad, centerLine, rot, parent.transform);
        go.transform.localScale = size;
    }

    private void MakeHorizontalRoads(GameObject parent, int indexWidth, int indexHeight)
    {
        float totalWidth = verticalLines * sizeRoad + (verticalLines - 1) * widthPlot;
        float halfWidth = totalWidth / 2f;
        float totalHeight = horizontalLines * sizeRoad + (horizontalLines - 1) * heightPlot;
        float halfHeight = totalHeight / 2f;

        float incrementWidth = sizeRoad + widthPlot;
        float incrementHeight = sizeRoad + heightPlot;

        Vector3 center = parentObject.position;

       
        Vector3 size = new Vector3((sizeRoad / 2f), 1, widthPlot / 2f);

        float xCoord = center.x + sizeRoad + (widthPlot / 2f) - halfWidth + incrementWidth * indexHeight;
        float zCoord = center.z + (sizeRoad / 2f) - halfHeight + incrementHeight * indexWidth;

        Vector3 centerLine = new Vector3(xCoord, center.y, zCoord);

        Quaternion rot = Quaternion.Euler(0, 90, 0);

        GameObject go = Instantiate(meshRoad, centerLine, rot, parent.transform);
        go.transform.localScale = size;
    }
}
