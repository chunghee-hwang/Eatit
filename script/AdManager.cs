using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    public string android_interstitial_id;
    public InterstitialAd interstitialAd;
    public bool adLoadFail= false;


    public IEnumerator RequestInterstitialAd()
    {
            string adUnitId = string.Empty;
#if UNITY_ANDROID
            adUnitId = android_interstitial_id;
#endif
            interstitialAd = new InterstitialAd(adUnitId);
            AdRequest request = new AdRequest.Builder()
                .AddTestDevice("D771D1BDF570E92BA86E6FCD5DE5CD5B")
                .Build();
            interstitialAd.LoadAd(request);
        interstitialAd.OnAdFailedToLoad += FailtoLoadAd;
        yield return null;
    }



    public void ShowInterstitialAd()
    {
            if (!interstitialAd.IsLoaded())
            {
                RequestInterstitialAd();
                return;
            }
            interstitialAd.Show();
    }

    public void FailtoLoadAd(object sender, EventArgs args)
    {
        Debug.Log("sender : " +sender+", args : "+ args);
        Debug.Log("failed to load ad");
        adLoadFail = true;
    }
 
    

}
