using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsOnBuilding : MonoBehaviour
{

    [SerializeField] private float minHeight = 0;
    [SerializeField] private int amountAds;
    [SerializeField] private List<Vector2> possibleHeights = new();

    [SerializeField] private List<Ads> ads = new();

    public bool init = false;


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
        if (init)
            return;

        if (this.transform.childCount == 0)
        {
            Place();
            init = true;
        }
    }

    public void Place()
    {

        for(int i = ads.Count - 1; i != 0; i--)
        {
            Ads ad = ads[i];
            minSizeAds = Mathf.Max(ad.size.y, minSizeAds);
        }


        if ((this.transform.localScale.x + minSizeAds) - (minHeight - minSizeAds) >= minSizeAds * 2.5f)
        {
            possibleHeights.Clear();
            possibleHeights.Add(new Vector2(minHeight + minSizeAds, this.transform.localScale.x + minSizeAds));
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

            if ((heightAd - minSizeAds * 2) - heightGrab.x >= minSizeAds)
            {
                Vector2 newLow = new Vector2(heightGrab.x, (heightAd - minSizeAds * 2));
                possibleHeights.Add(newLow);
            }

            if (heightGrab.y - (heightAd + minSizeAds * 2) >= minSizeAds)
            {
                Vector2 newHigh = new Vector2((heightAd + minSizeAds * 2), heightGrab.y);
                possibleHeights.Add(newHigh);
            }

            amountAdsLeft--;
            PlaceAd(heightAd);

        }
    }


    private void PlaceAd(float height)
    {
        Ads ad = ads.RandomItem();

        float width = ad.size.x;



        float posX = CenteralizedRandom.Range((-(this.transform.localScale.z / 2)) + (width / 2), (this.transform.localScale.z / 2) - (width / 2));



        GameObject go = Instantiate(ad.prefab, this.transform);

        go.transform.localRotation = Quaternion.Euler(ad.rotation);
        go.transform.localPosition = new Vector3(-(height / (transform.localScale.x)), -0.01f, posX / transform.localScale.z);
        go.transform.localScale = new Vector3((width / transform.localScale.z), .1f, ad.size.y / transform.localScale.x);

 

    }
}
