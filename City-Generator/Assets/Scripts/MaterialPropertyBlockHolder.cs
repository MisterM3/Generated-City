using UnityEngine;

[ExecuteAlways]
public class MaterialPropertyBlockHolder : MonoBehaviour
{
    [SerializeField] private SeriliazedPropertyBlocks mpb;
    public SeriliazedPropertyBlocks SerilalizedMbp
    {
        get
        {
            if (mpb == null)
                mpb = new();
            return mpb;
        }
    }


    private void OnEnable()
    {
        UpdateRenderer();
    }

    public void UpdateRenderer()
    {
        MeshRenderer rend = null;

        if (TryGetComponent<MeshRenderer>(out rend))
            rend.SetPropertyBlock(SerilalizedMbp.Mbp);
    }

}
