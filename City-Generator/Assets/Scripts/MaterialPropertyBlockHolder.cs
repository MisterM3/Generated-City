using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MaterialPropertyBlockHolder : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    public MaterialPropertyBlock Mpb
    {
        get
        {
            if (mpb == null)
                mpb = new();
            return mpb;
        }
    }

    public void OnEnable()
    {
        UpdateRenderer();
    }

    public void UpdateRenderer()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.SetPropertyBlock(Mpb);
    }

}
