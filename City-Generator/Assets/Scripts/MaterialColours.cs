using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialColours : MonoBehaviour
{


    public Material mat;


    [System.Serializable]
    public enum MaterialColour
    {
        MetalBlue,
        Beige,
        MetalBlack,
        Gray,
    }

    [System.Serializable]
    public struct BottomMaterial
    {
        [SerializeField] Color color;
        [SerializeField] List<Material> mat;

        public Material GetMaterial()
        {
            return mat.RandomItem();
        }
    }




}
