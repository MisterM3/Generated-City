using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AdsOnBuilding : MonoBehaviour
{

    [SerializeField] private float minHeight = 0;
    [SerializeField] private int amountAds;
    [SerializeField] private List<Vector2> possibleHeights = new();

    [SerializeField] private List<Ads> ads = new();


    private float minSizeAds = 0;

    [System.Serializable]
    private struct Ads
    {
        public GameObject prefab;
        public Vector2 size;
        public Vector3 rotation;
    }

    public void OnEnable()
    {
        if (UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this.gameObject))
            return;

        if (this.transform.childCount == 0)
        {
            Place();
        }
    }

    public void Place()
    {

        float deviation = 0;

        foreach(Ads ad in ads)
        {
            minSizeAds = Mathf.Max(ad.size.y / 2, minSizeAds);
        }


        if ((this.transform.localScale.y + minSizeAds) - (minHeight - minSizeAds) >= minSizeAds)
        {
            possibleHeights.Add(new Vector2(minHeight + deviation, this.transform.localScale.x + deviation));
            PlaceAds();
        }


    }


    private void PlaceAds()
    {

        int amountAdsLeft = amountAds;

        while(amountAdsLeft > 0 && possibleHeights.Count != 0)
        {

            Vector2 heightGrab = possibleHeights.RandomItem();

            possibleHeights.Remove(heightGrab);

            float heightAd = CenteralizedRandom.Range(heightGrab.x, heightGrab.y);

            if ((heightAd - minSizeAds/2) - heightGrab.x >= minSizeAds)
            {
                Vector2 newLow = new Vector2(heightGrab.x, (heightAd - minSizeAds / 2));
                possibleHeights.Add(newLow);
            }

            if (heightGrab.y - (heightAd + minSizeAds/2) >= minSizeAds)
            {
                Vector2 newHigh = new Vector2((heightAd + minSizeAds / 2), heightGrab.y);
                possibleHeights.Add(newHigh);
            }

            PlaceAd(heightAd);

        }
    }


    private void PlaceAd(float height)
    {
        Ads ad = ads.RandomItem();

        float width = ad.size.x;

        float posX = CenteralizedRandom.Range(-this.transform.localScale.z / 2 + width / 2, this.transform.localScale.z - width / 2);

        GameObject go = Instantiate(ad.prefab, this.transform);
        go.transform.position = new Vector3(height, 0, posX);
        go.transform.rotation = Quaternion.Euler(ad.rotation);
    }
}
