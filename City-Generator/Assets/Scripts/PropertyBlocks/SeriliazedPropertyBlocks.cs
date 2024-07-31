using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SeriliazedPropertyBlocks: ISerializationCallbackReceiver
{


    private MaterialPropertyBlock mbp;

    public MaterialPropertyBlock Mbp
    {
        get 
        {
            if (mbp == null)
                mbp = Instantiate();
            return mbp;
        }
    }

    [SerializeField] private MbpValues values;

    public MbpValues Values
    {
        get
        {
            if (values == null)
                values = new();
            return values;
        }
    }

    private MaterialPropertyBlock Instantiate()
    {
        mbp = new();
        return Values.SetValuesMpb(Mbp);
    }




    public void SetColor(int nameID, Color color)
    {
        Values.Colors.Add(new MbpValue<Color>(nameID, color));
        Mbp.SetColor(nameID, color);
    }

    public void SetVector(int nameID, Vector4 vector)
    {
        Values.Vectors.Add(new MbpValue<Vector4>(nameID, vector));
        Mbp.SetVector(nameID, vector);
    }


    [SerializeField] private List<int> colorKeys = new();
    [SerializeField] private List<Color> colorValues = new();

    [SerializeField] private List<int> vectorKeys = new();
    [SerializeField] private List<Vector4> vectorValues = new();

    public void OnBeforeSerialize()
    {
        colorKeys.Clear();
        colorValues.Clear();
        vectorKeys.Clear();
        vectorValues.Clear();

        if (Values.HasColorSerialized())
        {
            foreach(MbpValue<Color> color in Values.Colors)
            {
                colorKeys.Add(color.nameID);
                colorValues.Add(color.value);
            }
        }

        if (Values.HasVectorsSerialized())
        {
            foreach (MbpValue<Vector4> vectors in Values.Vectors)
            {
                vectorKeys.Add(vectors.nameID);
                vectorValues.Add(vectors.value);
            }
        }
    }

    public void OnAfterDeserialize()
    {

        Values.Colors.Clear();
        Values.Vectors.Clear();

        for(int i = 0; i != colorKeys.Count; i++)
        {
            Values.Colors.Add(new MbpValue<Color>(colorKeys[i], colorValues[i]));
        }

        for (int i = 0; i != vectorKeys.Count; i++)
        {
            Values.Vectors.Add(new MbpValue<Vector4>(vectorKeys[i], vectorValues[i]));
        }
    }

    [System.Serializable]
    public class MbpValue<T>
    {
        public int nameID;
        public T value;

        public MbpValue(int nameID, T value)
        {
            this.nameID = nameID;
            this.value = value;
        }
    }

    [System.Serializable]
    public class MbpValues
    {
        [SerializeField] private List<MbpValue<Color>> color;
        public List<MbpValue<Color>> Colors
        {
            get
            {
                if (color == null)
                    color = new();
                return color;
            }
        }

        public bool HasColorSerialized()
        {
            return (color != null && color.Count != 0);
        }

        public MaterialPropertyBlock SetColorsMpb(MaterialPropertyBlock mpb)
        {
            if (!HasColorSerialized())
                return mpb;

            foreach(MbpValue<Color> MbpColor in Colors)
            {
                mpb.SetColor(MbpColor.nameID, MbpColor.value);
            }

            return mpb;
        }



        [SerializeField] private List<MbpValue<Vector4>> vectors;
        public List<MbpValue<Vector4>> Vectors
        {
            get
            {
                if (vectors == null)
                    vectors = new();
                return vectors;
            }
        }

        public bool HasVectorsSerialized()
        {
            return (vectors != null && vectors.Count != 0);
        }

        public MaterialPropertyBlock SetVectorsMpb(MaterialPropertyBlock mpb)
        {
            if (!HasVectorsSerialized())
                return mpb;

            foreach (MbpValue<Vector4> MbpVector in Vectors)
            {
                mpb.SetVector(MbpVector.nameID, MbpVector.value);
            }

            return mpb;
        }

        public MaterialPropertyBlock SetValuesMpb(MaterialPropertyBlock mpb)
        {
            mpb = SetColorsMpb(mpb);
            mpb = SetVectorsMpb(mpb);
            return mpb;
        }
    }

}
