using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlopeBuilding", menuName = "ScriptableObjects/BuildingParts/SlopeBuilding", order = 3)]
public class SlopeBuilding : PartStrategy
{


    [SerializeField] GameObject slopeSidePrefab;

    private float indentation = 0;
    [SerializeField] private float minIndentation = .5f;
    [SerializeField] private float maxIndentation = 2.5f;
    private float heightOffset = 0;
    [SerializeField] private float minHeightOffset = .5f;
    [SerializeField] private float maxHeightOffset = 2.5f;
    private SlopeSide slopeSide = SlopeSide.Left;

    public override GameObject MakeBuildingPart(Vector3 size)
    {
        GameObject parent = new("BottomBuilding");
        Transform parentTf = parent.transform;
        MakeSlope(size, parentTf);

        return parent;
    }

    public override void RandomizeValues()
    {
        indentation = CenteralizedRandom.Range(minIndentation, maxIndentation);
        heightOffset = CenteralizedRandom.Range(minHeightOffset, maxHeightOffset);
        slopeSide = CenteralizedRandom.RandomizeEnum<SlopeSide>();
    }

    private enum SlopeSide
    {
        Left,
        Right,
        Foward,
        Back,
        Horizontal,
        Vertical
    }


    public void MakeSlope(Vector3 size, Transform parent)
    {
        float width = size.x;
        float height = size.y;
        float lenght = size.z;

        Vector3 scale = Vector3.one;
        Vector3 position = Vector3.zero;

        MakeSlopeSide(slopeSide, size, parent);
        MakeOtherSides(slopeSide, size, parent);

        switch (slopeSide)
        {
            case SlopeSide.Left:
                scale = new Vector3(width - indentation, height - heightOffset, lenght);
                position = new Vector3(-indentation / 2, parent.localPosition.y, parent.localPosition.z);
                break;
            case SlopeSide.Right:
                scale = new Vector3(width - indentation, height - heightOffset, lenght);
                position = new Vector3(indentation / 2, parent.localPosition.y, parent.localPosition.z);
                break;
            case SlopeSide.Foward:
                scale = new Vector3(width, height - heightOffset, lenght - indentation);
                position = new Vector3(parent.localPosition.x, parent.localPosition.y, -indentation / 2);
                break;
            case SlopeSide.Back:
                scale = new Vector3(width, height - heightOffset, lenght - indentation);
                position = new Vector3(parent.localPosition.x, parent.localPosition.y, indentation / 2);
                break;
            case SlopeSide.Horizontal:
                scale = new Vector3(width - (indentation * 2), height - heightOffset, lenght);
                break;
            case SlopeSide.Vertical:
                scale = new Vector3(width, height - heightOffset, lenght - (indentation * 2));
                break;
        }

        MakeBuilding(scale, parent);
        parent.position = position;
    }


    private void MakeSlopeSide(SlopeSide side, Vector3 size, Transform parent)
    {

        float width = size.x;
        float height = size.y;
        float lenght = size.z;

        float angle = Mathf.Atan(heightOffset / indentation);
        float sideScale = heightOffset / Mathf.Sin(angle);

        Vector3 sideHorizontal = new Vector3(sideScale / 2, 1, lenght / 2);
        Vector3 sideVertical = new Vector3(sideScale / 2, 1, width / 2);


        if (side == SlopeSide.Left || side == SlopeSide.Horizontal)
        {
            GameObject leftSlope = Instantiate(sidePrefab, parent);
            if (side == SlopeSide.Left)
                leftSlope.transform.position = new Vector3(((width - indentation) / 2), height - heightOffset, 0);
            if (side == SlopeSide.Horizontal)
                leftSlope.transform.position = new Vector3(((width) / 2 - indentation), height - heightOffset, 0);
            leftSlope.transform.rotation = Quaternion.Euler(0, 180, -Mathf.Rad2Deg * angle);
            leftSlope.transform.localScale = sideHorizontal;
        }

        if (side == SlopeSide.Right || side == SlopeSide.Horizontal)
        {
            GameObject rightSlope = Instantiate(sidePrefab, parent);
            if (side == SlopeSide.Right)
                rightSlope.transform.position = new Vector3(-((width - indentation) / 2), height - heightOffset, 0);
            if (side == SlopeSide.Horizontal)
                rightSlope.transform.position = new Vector3(-((width) / 2 - indentation), height - heightOffset, 0);
            rightSlope.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * angle);
            rightSlope.transform.localScale = sideHorizontal;
        }

        if (side == SlopeSide.Foward || side == SlopeSide.Vertical)
        {
            GameObject forwardSlope = Instantiate(sidePrefab, parent);
            if (side == SlopeSide.Foward)
                forwardSlope.transform.position = new Vector3(0, height - heightOffset, (lenght - indentation) / 2);
            if (side == SlopeSide.Vertical)
                forwardSlope.transform.position = new Vector3(0, height - heightOffset, (lenght) / 2 - indentation);
            forwardSlope.transform.rotation = Quaternion.Euler(0, 90, -Mathf.Rad2Deg * angle);
            forwardSlope.transform.localScale = sideVertical;
        }

        if (side == SlopeSide.Back || side == SlopeSide.Vertical)
        {
            GameObject backSlope = Instantiate(sidePrefab, parent);
            if (side == SlopeSide.Back)
                backSlope.transform.position = new Vector3(0, height - heightOffset, -(lenght - indentation) / 2);
            if (side == SlopeSide.Vertical)
                backSlope.transform.position = new Vector3(0, height - heightOffset, -((lenght) / 2 - indentation));
            backSlope.transform.rotation = Quaternion.Euler(0, -90, -Mathf.Rad2Deg * angle);
            backSlope.transform.localScale = sideVertical;
        }

        MakeTriangles(side, size, parent);

    }

    private void MakeTriangles(SlopeSide side, Vector3 size, Transform parent)
    {

        float width = size.x;
        float height = size.y;
        float lenght = size.z;


        

        Vector3 scale = new Vector3(indentation/2, heightOffset/2, 1);

        if (side == SlopeSide.Left || side == SlopeSide.Horizontal)
        {
            GameObject triangleLeft = Instantiate(slopeSidePrefab, parent);
            GameObject triangleRight = Instantiate(slopeSidePrefab, parent);
            triangleLeft.transform.localScale = scale;
            triangleRight.transform.localScale = scale;

            float xPos = 0;

            if (side == SlopeSide.Horizontal)
                xPos = ((width) / 2 - indentation);
            else
                xPos = ((width - indentation) / 2);

            triangleLeft.transform.position = new Vector3(xPos, height - heightOffset, lenght / 2);
            triangleRight.transform.position = new Vector3(xPos, height - heightOffset, -lenght / 2);

            triangleLeft.transform.rotation = Quaternion.Euler(-180, 180, 0);
            triangleRight.transform.rotation = Quaternion.Euler(-180, 180, 0);

        }

        if (side == SlopeSide.Right || side == SlopeSide.Horizontal)
        {
            GameObject triangleLeft = Instantiate(slopeSidePrefab, parent);
            GameObject triangleRight = Instantiate(slopeSidePrefab, parent);
            triangleLeft.transform.localScale = scale;
            triangleRight.transform.localScale = scale;

            float xPos = 0;

            if (side == SlopeSide.Horizontal)
                xPos = -((width) / 2 - indentation);
            else
                xPos = -((width - indentation) / 2);

            triangleLeft.transform.position = new Vector3(xPos, height - heightOffset, -lenght / 2);
            triangleRight.transform.position = new Vector3(xPos, height - heightOffset, lenght / 2);

            triangleLeft.transform.rotation = Quaternion.Euler(-180, 0, 0);
            triangleRight.transform.rotation = Quaternion.Euler(-180, 0, 0);
        }
        if (side == SlopeSide.Foward || side == SlopeSide.Vertical)
        {
            GameObject triangleLeft = Instantiate(slopeSidePrefab, parent);
            GameObject triangleRight = Instantiate(slopeSidePrefab, parent);
            triangleLeft.transform.localScale = scale;
            triangleRight.transform.localScale = scale;

            float zPos = 0;

            if (side == SlopeSide.Vertical || side == SlopeSide.Vertical)
                zPos = ((lenght) / 2 - indentation);
            else
                zPos = ((lenght - indentation) / 2);

            triangleLeft.transform.position = new Vector3(-width / 2, height - heightOffset, zPos);
            triangleRight.transform.position = new Vector3(width / 2, height - heightOffset, zPos);

            triangleLeft.transform.rotation = Quaternion.Euler(-180, 90, 0);
            triangleRight.transform.rotation = Quaternion.Euler(-180, 90, 0);
        }
        if (side == SlopeSide.Back || side == SlopeSide.Vertical)
        {
            GameObject triangleLeft = Instantiate(slopeSidePrefab, parent);
            GameObject triangleRight = Instantiate(slopeSidePrefab, parent);
            triangleLeft.transform.localScale = scale;
            triangleRight.transform.localScale = scale;

            float zPos = 0;

            if (side == SlopeSide.Vertical)
                zPos = -((lenght) / 2 - indentation);
            else
                zPos = -((lenght - indentation) / 2);

            triangleLeft.transform.position = new Vector3(width / 2, height - heightOffset, zPos);
            triangleRight.transform.position = new Vector3(-width / 2, height - heightOffset, zPos);

            triangleLeft.transform.rotation = Quaternion.Euler(-180, -90, 0);
            triangleRight.transform.rotation = Quaternion.Euler(-180, -90, 0);
        }
    }

    private void MakeOtherSides(SlopeSide side, Vector3 size, Transform parent)
    {
        float width = size.x;
        float height = size.y;
        float lenght = size.z;

        Vector3 sideVertical = new Vector3(heightOffset / 2, 1, (lenght - indentation) / 2);
        Vector3 sideHorizontal = new Vector3(heightOffset / 2, 1, (width - indentation) / 2 );

        if (side == SlopeSide.Vertical)
            sideVertical = new Vector3(heightOffset / 2, 1, (lenght) / 2 - indentation);
        if (side == SlopeSide.Horizontal)
            sideHorizontal = new Vector3(heightOffset / 2, 1, (width) / 2 - indentation);


        if (side == SlopeSide.Left || side == SlopeSide.Horizontal || side == SlopeSide.Right)
        {
            GameObject forward = Instantiate(sidePrefab, parent);
            forward.transform.position = new Vector3(0, height - heightOffset, (lenght / 2));
            forward.transform.rotation = Quaternion.Euler(0, 90, -90);
            forward.transform.localScale = sideHorizontal;

            GameObject back = Instantiate(sidePrefab, parent);
            back.transform.position = new Vector3(0, height - heightOffset, -(lenght / 2));
            back.transform.rotation = Quaternion.Euler(0, -90, -90);
            back.transform.localScale = sideHorizontal;


            sideVertical = new Vector3(heightOffset / 2, 1, (lenght) / 2);

            if (slopeSide == SlopeSide.Right)
            {
                GameObject left = Instantiate(sidePrefab, parent);
                left.transform.position = new Vector3(((width - indentation) / 2), height - heightOffset, 0);
                left.transform.rotation = Quaternion.Euler(0, 180, -90);
                left.transform.localScale = sideVertical;
            }
            else if (slopeSide == SlopeSide.Left)
            {
                GameObject right = Instantiate(sidePrefab, parent);
                right.transform.position = new Vector3(-((width - indentation) / 2), height - heightOffset, 0);
                right.transform.rotation = Quaternion.Euler(0, 0, -90);
                right.transform.localScale = sideVertical;
            }

        }

       

        if (side == SlopeSide.Foward || side == SlopeSide.Vertical || side == SlopeSide.Back)
        {
            GameObject left = Instantiate(sidePrefab, parent);
            left.transform.position = new Vector3((width / 2), height - heightOffset, 0);
            left.transform.rotation = Quaternion.Euler(0, 180, -90);
            left.transform.localScale = sideVertical;

            GameObject right = Instantiate(sidePrefab, parent);
            right.transform.position = new Vector3(-(width / 2), height - heightOffset, 0);
            right.transform.rotation = Quaternion.Euler(0, 0, -90);
            right.transform.localScale = sideVertical;

            sideHorizontal = new Vector3(heightOffset / 2, 1, (width) / 2);

            if (slopeSide == SlopeSide.Foward)
            {

                GameObject back = Instantiate(sidePrefab, parent);
                back.transform.position = new Vector3(0, height - heightOffset, -((lenght - indentation) / 2));
                back.transform.rotation = Quaternion.Euler(0, -90, -90);
                back.transform.localScale = sideHorizontal;
            }
            else if (slopeSide == SlopeSide.Back)
            {
                GameObject forward = Instantiate(sidePrefab, parent);
                forward.transform.position = new Vector3(0, height - heightOffset, ((lenght - indentation) / 2));
                forward.transform.rotation = Quaternion.Euler(0, 90, -90);
                forward.transform.localScale = sideHorizontal;
            }
        }
    }

}
