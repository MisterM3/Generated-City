using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixTilingChildren : MonoBehaviour
{
    [SerializeField] private DirectionUV direction;
    [SerializeField] private float multiplier = 1f;

    static readonly int shPropColor = Shader.PropertyToID("_BaseColorMap_ST");

    public void FixTilingChilds()
    {

        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            MaterialPropertyBlockHolder mpbHolder;

            if (!child.TryGetComponent<MaterialPropertyBlockHolder>(out mpbHolder))
                mpbHolder = child.gameObject.AddComponent<MaterialPropertyBlockHolder>();
            
            SetMpbTilingSize(mpbHolder);
        }
    }

    private void SetMpbTilingSize(MaterialPropertyBlockHolder mpbHolder)
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

        mpbHolder.Mpb.SetVector(shPropColor, offsetTiling);
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
