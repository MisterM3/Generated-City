using UnityEngine;

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


    public void UpdateRenderer()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.SetPropertyBlock(Mpb);
    }

}
