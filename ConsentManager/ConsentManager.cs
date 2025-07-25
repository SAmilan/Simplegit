using GoogleMobileAds.Ump.Api;
using System;
using System.Collections;
using UnityEngine;

public class ConsentManager : MonoBehaviour
{


    [Header("IronSource App Key")]
    public string ironSourceAppKey = "20f876b05";

    [SerializeField] GoogleAdmobManager googleAdmobManager;
    // [SerializeField] AdManager IronSourceAds;

    public bool CanRequestAds => ConsentInformation.CanRequestAds();

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f); // optional delay

        GatherConsent((string error) =>
        {
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Consent Error: {error}");
            }

            if (CanRequestAds)
            {
                Debug.Log("Consent granted. Initializing Ads.");
                googleAdmobManager.InitializeGoogleAds();
                // IronSourceAds.InitializeIronsource();
            }
            else
            {
                Debug.LogWarning("Consent not granted. Ads will not be initialized.");
            }
        });
    }

    public void GatherConsent(Action<string> onComplete)
    {
        Debug.Log("Gathering consent...");

        var requestParameters = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,

#if UNITY_EDITOR
            ConsentDebugSettings = new ConsentDebugSettings
            {
                DebugGeography = DebugGeography.EEA,
                TestDeviceHashedIds = new System.Collections.Generic.List<string> { GetDeviceID() }
            }
#endif
        };

        ConsentInformation.Update(requestParameters, (FormError updateError) =>
        {
            if (updateError != null)
            {
                onComplete?.Invoke(updateError.Message);
                return;
            }

            ConsentStatus consentStatus = ConsentInformation.ConsentStatus;
            bool isFormAvailable = ConsentInformation.IsConsentFormAvailable();

            if (consentStatus == ConsentStatus.Required && isFormAvailable)
            {
                ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
                {
                    if (formError != null)
                    {
                        onComplete?.Invoke(formError.Message);
                    }
                    else
                    {
                        onComplete?.Invoke(null); // Success
                    }
                });
            }
            else
            {
                onComplete?.Invoke(null); // No need to show form
            }
        });
    }

    public void ResetConsentInformation()
    {
        ConsentInformation.Reset();
        Debug.Log("Consent information reset.");
    }

    public static string GetDeviceID()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");

        string android_id = clsSecure.CallStatic<string>("getString", objResolver, "android_id");

        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(android_id);

        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
#else
        return "test-device-id-editor";
#endif
    }



    /*
        [Header("IronSource App Key")]
        public string ironSourceAppKey = "20f876b05";
        [SerializeField] GoogleAdmobManager googleAdmobManager;
        // [SerializeField] AdManager IronSourceAds;
        public bool CanRequestAds => ConsentInformation.CanRequestAds();


        *//*  IEnumerator Start()
          {
              if (CanRequestAds)
              {
                  //Debug.Log("xxx can request ads");
                  googleAdmobManager.InitializeGoogleAds();
                  //  IronSourceAds.InitializeIronsource();
              }
              yield return new WaitForSeconds(0.5f);
              GatherConsent((string error) =>
              {
                  if (error != null)
                  {
                      //Debug.Log("xxx  failed to gather concent");
                  }

                  if (CanRequestAds)
                  {
                      //Debug.Log("xxx can request ads thats why initializing ads");
                      googleAdmobManager.InitializeGoogleAds();
                      //  IronSourceAds.InitializeIronsource();
                  }
              });
          }*//*


        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f); // small delay is fine

            GatherConsent((string error) =>
            {
                if (error != null)
                {
                    Debug.LogError($"Consent error: {error}");
                }

                if (CanRequestAds)
                {
                    googleAdmobManager.InitializeGoogleAds();
                }
                else
                {
                    Debug.LogWarning("Ads cannot be requested due to missing consent.");
                }
            });
        }


        public void GatherConsent(Action<string> onComplete)
        {
            //Debug.Log("xxx Gathering consent.");

            var requestParameters = new ConsentRequestParameters
            {
                // False means users are not under age.
                TagForUnderAgeOfConsent = false,
               // ConsentDebugSettings = new ConsentDebugSettings
                //{
                    // For debugging consent settings by geography.
                  //  DebugGeography = DebugGeography.EEA,
                    //TestDeviceHashedIds = new() { "d81fa6392b199c72a56756c9e8c9465f" }
                //}
            };

            ConsentInformation.Update(requestParameters, (FormError updateError) =>
            {
                if (updateError != null)
                {
                    onComplete(updateError.Message);
                    return;
                }

                //// Determine the consent-related action to take based on the ConsentStatus.
                //if (CanRequestAds)
                //{
                //    Debug.Log("xxx have consent");
                //    googleAdmobManager.InitializeGoogleAds();
                //    IronSourceAds.InitializeIronsource();
                //    // Consent has already been gathered or not required.
                //    // Return control back to the user.
                //    onComplete(null);
                //    return;
                //}

                // Consent not obtained and is required.
                // Load the initial consent request form for the user.
                ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
                {
                    onComplete?.Invoke(showError?.Message);
                });
            });
        }

        /// <summary>
        /// Reset ConsentInformation for the user.
        /// </summary>
        public void ResetConsentInformation() => ConsentInformation.Reset();

        public static string GetDeviceID()
        {
            // Get Android ID
            AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");

            string android_id = clsSecure.CallStatic<string>("getString", objResolver, "android_id");

            // Get bytes of Android ID
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(android_id);

            // Encrypt bytes with md5
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            string device_id = hashString.PadLeft(32, '0');

            return device_id;
        }*/
}
