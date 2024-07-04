using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{


    [SerializeField] DrawRoadData data;
    [SerializeField] MakeRoadsVisuals visuals;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ContextMenu("Make Roads")]
    public void MakeRoads()
    {
        data.DrawRoads();
        visuals.DrawRoads();
    }
}
