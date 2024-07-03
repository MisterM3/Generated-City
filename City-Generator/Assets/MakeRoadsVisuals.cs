using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeRoadsVisuals : MonoBehaviour
{
    [SerializeField] DrawRoadData _drawData;

    [SerializeField] GameObject _prefabRoad;
    [SerializeField] Transform _parent;


    [ContextMenu("DrawRoads")]
    public void DrawRoads()
    {

        for(int i = _parent.childCount - 1; i >= 0; i--)
        {
            Transform child = _parent.GetChild(i);
            DestroyImmediate(child.gameObject);
        }

       List<RoadPosition> roadPositions = _drawData.RoadPositions;
       
        foreach(RoadPosition roadPosition in roadPositions)
        {
            DrawRoad(roadPosition);
        }

    }


    private void DrawRoad(RoadPosition roadPosition)
    {

        Vector3 direction = roadPosition.endPos - roadPosition.startPos;
        Vector3 middleRoad = ((direction) / 2) + roadPosition.startPos;

        Quaternion euler = Quaternion.identity;

        if (direction.normalized.Abs() == Vector3.right)
        {
           euler = Quaternion.Euler(Vector3.up * 90);
        }

        GameObject roadObject = Instantiate(_prefabRoad, middleRoad, euler);
        Transform roadTransform = roadObject.transform;
        roadTransform.SetParent(_parent);

        

        Vector3 scaleOld = direction.Abs() - (Vector3.one * 5);
        

        Vector3 scale = Vector3.Max(scaleOld, Vector3.one * 5);

        if (direction.normalized.Abs() == Vector3.right)
        {
            scale = new Vector3(scale.z, scale.y, scale.x);
        }

        roadTransform.localScale = scale;

        MeshRenderer rend = roadTransform.GetComponent<MeshRenderer>();

        Vector2 rot = new Vector2(Mathf.Round(Mathf.Max(scaleOld.z / 5f, 1f)), Mathf.Round(Mathf.Max(scaleOld.x / 5f, 1f)));

        if (direction.normalized.Abs() == Vector3.right)
        {
            rot = new Vector2(rot.y, rot.x);
        }

        var tempMaterial = new Material(rend.sharedMaterial);
        tempMaterial.mainTextureScale = rot;
        rend.sharedMaterial = tempMaterial;



    }
}
