
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, RequireComponent(typeof(MaterialPropertyBlockHolder))]
public class FixTiling : MonoBehaviour
{


    [SerializeField] private DirectionUV direction;
    [SerializeField] private float multiplier = 1f;

    static readonly int shPropColor = Shader.PropertyToID("_BaseColorMap_ST");
    private MaterialPropertyBlockHolder mpbHolder;



    public void OnEnable()
    {
        if (mpbHolder == null)
            mpbHolder = GetComponent<MaterialPropertyBlockHolder>();
        SetMpbTilingSize();
    }

    private void SetMpbTilingSize()
    {

        Vector4 offsetTiling = new Vector4(1, 1, 0, 0);

        if (direction == DirectionUV.X || direction == DirectionUV.XY)
        {
            offsetTiling.x = Mathf.RoundToInt(this.transform.lossyScale.x * multiplier);
            offsetTiling.x = Mathf.Max(offsetTiling.x, .25f);
        }

        if (direction == DirectionUV.Y || direction == DirectionUV.XY)
        {
            offsetTiling.y = Mathf.RoundToInt(this.transform.lossyScale.z * multiplier);
            offsetTiling.y = Mathf.Max(offsetTiling.y, .25f);
        }

        mpbHolder.SerilalizedMbp.SetVector(shPropColor, offsetTiling);
        mpbHolder.UpdateRenderer();
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
