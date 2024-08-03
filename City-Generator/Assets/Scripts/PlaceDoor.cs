using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDoor : MonoBehaviour
{

    [SerializeField] List<GameObject> prefabs;
    [SerializeField] float amountFromSides;
    [SerializeField] float scaleDoor = 2;
    [SerializeField] float changePlaceDoor = .75f;


    public void PlaceDoorItem()
    {

        if (CenteralizedRandom.Range(0, 1.0f) > changePlaceDoor)
            return;

        if (prefabs != null && prefabs.Count != 0)
        {

            float xMinPos = -this.transform.localScale.z/2 + amountFromSides;
            float xMaxPos = this.transform.localScale.z/2 - amountFromSides;

            float xPos = CenteralizedRandom.Range(xMinPos, xMaxPos);

            GameObject go = Instantiate(prefabs.RandomItem(), this.transform);

            Transform tf = go.transform;
            Transform parent = tf.parent;


            tf.localScale = new Vector3(scaleDoor / parent.lossyScale.z, scaleDoor / parent.lossyScale.y, scaleDoor / parent.lossyScale.x);
            tf.localPosition = new Vector3(-tf.localScale.z, -.01f, xPos / this.transform.localScale.z);

        }
    }

}
