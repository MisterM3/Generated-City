using UnityEngine;

public abstract class PartStrategy : ScriptableObject
{

    [SerializeField] protected GameObject sidePrefab;

    public abstract void RandomizeValues();
    public abstract GameObject MakeBuildingPart(Vector3 size);

    protected void MakeBuilding(Vector3 size, Transform parent)
    {
        float width = size.x;
        float height = size.y;
        float lenght = size.z;

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
}
