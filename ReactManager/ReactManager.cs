using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ReactManager : MonoBehaviour
{
    #region Private_Vars
#if UNITY_WEBGL == true && UNITY_EDITOR == false
      [DllImport("__Internal")]
      private static extern void SendProductToken(string productToken);
#endif

    #endregion

    #region Public_Vars
    public static ReactManager instance;
    #endregion


    #region Unity_Callbacks
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    #region Public_Methods
    public void SendProduct(string productToken)
    {
       
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    Debug.Log("The product is called: "+productToken);
    SendProductToken (productToken);
#endif
    }

    #endregion




}
