using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BottomPartBuilding : MonoBehaviour, IGenerateBuildingPart
{

    private Vector3 size;

    public IndentShape indentShape = IndentShape.None;

    [SerializeField] List<BuildingMakerCollection> partBottomCollection = new();


    [System.Serializable]
    private struct BuildingMakerCollection
    {
        public IndentShape shape;
        public PartStrategy strategy;
    }

    public void SetDimensions(Vector3 dimensions)
    {
        size = dimensions;
    }

    public void RandomizeValues()
    {
        indentShape = CenteralizedRandom.RandomizeEnum<IndentShape>();

        foreach (BuildingMakerCollection bm in partBottomCollection)
        {
            bm.strategy.RandomizeValues();
        }

    }

    public GameObject GenerateBuildingPart()
    {
        PartStrategy bottomStrategy = null;

     foreach (BuildingMakerCollection bm in partBottomCollection)
        {
            if (bm.shape == indentShape)
            {
                bottomStrategy = bm.strategy;
                break;
            }
        }

        GameObject go = bottomStrategy.MakeBuildingPart(size);

        for (int i = go.transform.childCount - 1; i >= 0; i--)
        {
            if (go.transform.GetChild(i).TryGetComponent<AdsOnBuilding>(out AdsOnBuilding ads))
            {
                ads.Place();
            }

            if (go.transform.GetChild(i).TryGetComponent<PlaceDoor>(out PlaceDoor door))
            {
                door.PlaceDoorItem();
            }
        }

        return go;
    }

}

public enum IndentShape
{
    None,
    Flat,
    Slope,
}
