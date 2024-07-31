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

        RecursiveTiling(transform);
    }

    private void RecursiveTiling(Transform tf)
    {
        MaterialPropertyBlockHolder mpbHolder;



        if (!tf.TryGetComponent<MaterialPropertyBlockHolder>(out mpbHolder))
            mpbHolder = tf.gameObject.AddComponent<MaterialPropertyBlockHolder>();
        SetMpbTilingSize(mpbHolder, tf);

        if (tf.childCount == 0)
            return;

        for (int i = tf.childCount - 1; i >= 0; i--)
        {
            Transform child = tf.GetChild(i);
            RecursiveTiling(child);
        }
    }

    private void SetMpbTilingSize(MaterialPropertyBlockHolder mpbHolder, Transform tf)
    {

        Vector4 offsetTiling = new Vector4(1, 1, 0, 0);

        if (direction == DirectionUV.X || direction == DirectionUV.XY)
        {
            offsetTiling.x = Mathf.RoundToInt(tf.lossyScale.x * multiplier);
            offsetTiling.x = Mathf.Max(offsetTiling.x, .25f);
        }

        if (direction == DirectionUV.Y || direction == DirectionUV.XY)
        {
            offsetTiling.y = Mathf.RoundToInt(tf.lossyScale.z * multiplier);
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
