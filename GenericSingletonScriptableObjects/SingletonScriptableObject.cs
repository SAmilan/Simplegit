using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingletonScriptableObject<T>: ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                T[] assets = null;
                assets = (instance == null) ? Resources.LoadAll<T>("") : assets;

                if(assets == null || assets.Length < 1)
                {
                    throw new System.Exception("Could not find any scriptable objects");
                }else if(assets.Length > 1)
                {
                    Debug.Log("Multipe instances of the scriptable object found in the Resources");
                }
                instance = assets[0];
            }
            return instance;

        }
    }
        
    
    
}
