
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FixTiling : MonoBehaviour
{


    [SerializeField] private DirectionUV direction;

    private MaterialPropertyBlock mpb;

    static readonly int shPropColor = Shader.PropertyToID("_BaseColor");
    static readonly int shMainTex = Shader.PropertyToID("_BaseMap_ST");
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
        MeshRenderer rend = GetComponent<MeshRenderer>();
        SetMpbTilingSize();
        rend.SetPropertyBlock(Mpb);
    }

    private void SetMpbTilingSize()
    {

        Vector4 offsetTiling = new Vector4(0, 0, 1, 1);

        if (direction == DirectionUV.X || direction == DirectionUV.XY)
        {
            offsetTiling.x = this.transform.lossyScale.x;
        }

        if (direction == DirectionUV.Y || direction == DirectionUV.XY)
        {
            offsetTiling.y = this.transform.lossyScale.z;
        }

        Debug.Log(offsetTiling);

        Mpb.SetColor(shPropColor, Color.red);
        Mpb.SetVector("_BaseColorMap_ST", offsetTiling);

    }

    [System.Serializable]
    private enum DirectionUV
    {
        None,
        X,
        Y,
        XY
    }
}
#endif
