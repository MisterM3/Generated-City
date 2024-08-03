using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlatBuilding", menuName = "ScriptableObjects/BuildingParts/FlatTopBuilding", order = 2)]
public class SquareFlatTop : PartStrategy
{

    [SerializeField] GameObject topPrefab;
    [SerializeField] GameObject cornerPrefab;

    [SerializeField] FlatIndent indentValues;
    [SerializeField] float minIndent = .5f;
    [SerializeField] float maxIndent = 2.5f;

    public override GameObject MakeBuildingPart(Vector3 size)
    {

        GameObject parent = new("BottomBuilding");
        Transform parentTf = parent.transform;

        MakeBuilding(size, parentTf);
        MakeCornersFlat(size, parentTf);
        MakeFlatSides(size, parentTf);

        float xPos = -indentValues.leftIndent + indentValues.rightIndent;
        float zPos = -indentValues.forwardIndent + indentValues.backwardsIndent;

        parentTf.localPosition = new Vector3(xPos / 2, parentTf.localPosition.y, zPos / 2);

        return parent;
    }

    private void MakeCornersFlat(Vector3 size, Transform parentTf)
    {
        float width = size.x;
        float height = size.y;
        float lenght = size.z;

        float objectWidth = size.x - indentValues.leftIndent - indentValues.rightIndent;
        float objectLenght = size.z - indentValues.forwardIndent - indentValues.backwardsIndent;

        GameObject leftSlideCorner = Instantiate(cornerPrefab, parentTf);
        leftSlideCorner.transform.position = new Vector3((objectWidth / 2), height, -(objectLenght / 2));
        leftSlideCorner.transform.rotation = Quaternion.Euler(0, 180, 0);
        leftSlideCorner.transform.localScale = new Vector3(indentValues.leftIndent / 2, leftSlideCorner.transform.localScale.y, indentValues.backwardsIndent / 2);

        GameObject rightSlideCorner = Instantiate(cornerPrefab, parentTf);
        rightSlideCorner.transform.position = new Vector3((objectWidth / 2), height, (objectLenght / 2));
        rightSlideCorner.transform.rotation = Quaternion.Euler(0, 90, 0);
        rightSlideCorner.transform.localScale = new Vector3(indentValues.forwardIndent / 2, rightSlideCorner.transform.localScale.y, indentValues.leftIndent / 2);

        GameObject forwardSlideCorner = Instantiate(cornerPrefab, parentTf);
        forwardSlideCorner.transform.position = new Vector3(-(objectWidth / 2), height, objectLenght / 2);
        forwardSlideCorner.transform.rotation = Quaternion.Euler(0, 0, 0);
        forwardSlideCorner.transform.localScale = new Vector3(indentValues.rightIndent / 2, forwardSlideCorner.transform.localScale.y, indentValues.forwardIndent / 2);

        GameObject backSlideCorner = Instantiate(cornerPrefab, parentTf);
        backSlideCorner.transform.position = new Vector3(-(objectWidth / 2), height, -objectLenght / 2);
        backSlideCorner.transform.rotation = Quaternion.Euler(0, -90, 0);
        backSlideCorner.transform.localScale = new Vector3(indentValues.backwardsIndent / 2, backSlideCorner.transform.localScale.y, indentValues.rightIndent / 2);
    }

    private void MakeFlatSides(Vector3 size, Transform parentTf)
    {
        float height = size.y;

        float objectWidth = size.x - indentValues.leftIndent - indentValues.rightIndent;
        float objectLenght = size.z - indentValues.forwardIndent - indentValues.backwardsIndent;

        GameObject leftSlide = Instantiate(topPrefab, parentTf);
        leftSlide.transform.position = new Vector3((objectWidth / 2), height, 0);
        leftSlide.transform.rotation = Quaternion.Euler(0, 180, 0);
        leftSlide.transform.localScale = new Vector3(indentValues.leftIndent / 2, leftSlide.transform.localScale.y, objectLenght / 2);

        GameObject rightSlide = Instantiate(topPrefab, parentTf);
        rightSlide.transform.position = new Vector3(-objectWidth / 2, height, 0);
        rightSlide.transform.rotation = Quaternion.Euler(0, 0, 0);
        rightSlide.transform.localScale = new Vector3(indentValues.rightIndent / 2, rightSlide.transform.localScale.y, objectLenght / 2);

        GameObject forwardSlide = Instantiate(topPrefab, parentTf);
        forwardSlide.transform.position = new Vector3(0, height, objectLenght / 2);
        forwardSlide.transform.rotation = Quaternion.Euler(0, 90, 0);
        forwardSlide.transform.localScale = new Vector3(indentValues.forwardIndent / 2, forwardSlide.transform.localScale.y, objectWidth / 2);

        GameObject backSlide = Instantiate(topPrefab, parentTf);
        backSlide.transform.position = new Vector3(0, height, -objectLenght / 2);
        backSlide.transform.rotation = Quaternion.Euler(0, -90, 0);
        backSlide.transform.localScale = new Vector3(indentValues.backwardsIndent / 2, forwardSlide.transform.localScale.y, objectWidth / 2);
    }

    private void MakeBuilding(Vector3 size, Transform parent)
    {
        float width = size.x - indentValues.leftIndent - indentValues.rightIndent;
        float height = size.y;
        float lenght = size.z - indentValues.forwardIndent - indentValues.backwardsIndent;

        GameObject left = Instantiate(sidePrefab, parent);
        left.transform.position = new Vector3(-width / 2, 0, 0);
        left.transform.localScale = new Vector3(height / 2, 1, lenght / 2);
        GameObject right = Instantiate(sidePrefab, parent);
        right.transform.position = new Vector3(width / 2, 0, 0);
        right.transform.rotation = Quaternion.Euler(0, 180, -90);
        right.transform.localScale = new Vector3(height / 2, 1, lenght / 2);
        GameObject forward = Instantiate(sidePrefab, parent);
        forward.transform.position = new Vector3(0, 0, -lenght / 2);
        forward.transform.rotation = Quaternion.Euler(0, -90, -90);
        forward.transform.localScale = new Vector3(height / 2, 1, width / 2);
        GameObject back = Instantiate(sidePrefab, parent);
        back.transform.position = new Vector3(0, 0, lenght / 2);
        back.transform.rotation = Quaternion.Euler(0, 90, -90);
        back.transform.localScale = new Vector3(height / 2, 1, width / 2);
    }

    public override void RandomizeValues()
    {
        indentValues.RandomizeInit(minIndent, maxIndent);
    }


    [System.Serializable]
    private class FlatIndent
    {
        [Min(0)] public float leftIndent = 0;
        [Min(0)] public float rightIndent = 0;
        [Min(0)] public float forwardIndent = 0;
        [Min(0)] public float backwardsIndent = 0;

        public void RandomizeInit(float minIndent, float maxIndentPerSide)
        {
            if (CenteralizedRandom.CoinToss())
                leftIndent = CenteralizedRandom.Range(minIndent, maxIndentPerSide);
            if (CenteralizedRandom.CoinToss())
                rightIndent = CenteralizedRandom.Range(minIndent, maxIndentPerSide);
            if (CenteralizedRandom.CoinToss())
                forwardIndent = CenteralizedRandom.Range(minIndent, maxIndentPerSide);
            if (CenteralizedRandom.CoinToss())
                backwardsIndent = CenteralizedRandom.Range(minIndent, maxIndentPerSide);
        }

    }
}
