using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways, RequireComponent(typeof(MaterialPropertyBlockHolder))]
public class SetColorMaterial : MonoBehaviour
{
    [SerializeField] private List<Color> colors;

    static readonly int shPropColor = Shader.PropertyToID("_BaseColor");
    private MaterialPropertyBlockHolder mpbHolder;

    public void OnEnable()
    {
        if (mpbHolder == null)
            mpbHolder = GetComponent<MaterialPropertyBlockHolder>();
        SetColor();
    }

    private void SetColor()
    {
        mpbHolder.SerilalizedMbp.SetVector(shPropColor, colors.RandomItem());
        mpbHolder.UpdateRenderer();
    }

}
