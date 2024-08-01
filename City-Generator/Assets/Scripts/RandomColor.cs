using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RandomColor : MonoBehaviour
{

    [SerializeField] Color selectedColor;
    [SerializeField] List<Color> colors;
    [SerializeField] MaterialPropertyBlockHolder holder;
    static readonly int shPropColor = Shader.PropertyToID("_BaseColor");

    private void OnEnable()
    {
        if (UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this.gameObject))
            return;


        if (selectedColor == Color.black && colors != null && colors.Count != 0)
        {
            selectedColor = colors.RandomItem();
            holder.SerilalizedMbp.SetColor(shPropColor, selectedColor);
            holder.UpdateRenderer();
        }
    }

}
