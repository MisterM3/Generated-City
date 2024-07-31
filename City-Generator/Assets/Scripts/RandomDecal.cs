using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RandomDecal : MonoBehaviour
{

    [SerializeField] Material selectedMaterial;
    [SerializeField] List<Material> mat;


    private void OnEnable()
    {
        if (selectedMaterial == null && mat != null && mat.Count != 0)
        {
            selectedMaterial = mat.RandomItem();
            GetComponent<Renderer>().material = selectedMaterial;
        }
    }

}
