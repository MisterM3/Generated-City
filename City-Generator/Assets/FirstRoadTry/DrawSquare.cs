using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSquare : MonoBehaviour
{

    [SerializeField]
    DrawRoadData drawRoadData;

    [SerializeField] GameObject plane;
    [SerializeField] Transform parent;

    public void DrawBigSquare()
    {
        List<RoadPosition> roadPositions = drawRoadData.RoadPositions;


        float maxX = 0;
        float maxZ = 0;
        float minX = 0;
        float minZ = 0;

        foreach(RoadPosition roadPosition in roadPositions)
        {
            Vector3 endPos = roadPosition.endPos;

            minX = Mathf.Min(minX, endPos.x);
            minZ = Mathf.Min(minZ, endPos.z);
            maxX = Mathf.Max(maxX, endPos.x);
            maxZ = Mathf.Max(maxZ, endPos.z);
        }


        Vector3 position = new Vector3((maxX + minX) / 2f, -1, (maxZ + minZ) / 2f);

        GameObject planeObject = Instantiate(plane, position, Quaternion.identity, parent);

        float xSize = Mathf.Abs(minX) + maxX;
        float zSize = Mathf.Abs(minZ) + maxZ;


        planeObject.transform.localScale = new Vector3(xSize/10f, 0, zSize/10);


    }
}
