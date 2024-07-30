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
        MeshRenderer rend = null;

        if (TryGetComponent<MeshRenderer>(out rend))
            rend.SetPropertyBlock(Mpb);
    }

}
