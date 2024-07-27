using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MainMaterialBuilding : MonoBehaviour
{
    [SerializeField] private List<Material> materials;

    public void OnEnable()
    {
        SetMaterial();
    }

    private void SetMaterial()
    {
        Renderer rend = GetComponent<MeshRenderer>();
        rend.sharedMaterial = materials.RandomItem();
    }
}
