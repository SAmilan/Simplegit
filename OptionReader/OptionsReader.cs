using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class OptionsReader : MonoBehaviour
{
    string filedestination = null;
    public static OptionsReader instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        filedestination = Application.streamingAssetsPath + "/OptionData.json";
    }

    public void SaveFile()
    {
        OptionTemplate Data = GameData.Instance.GameOptions;
        string jsonString = JsonUtility.ToJson(Data);
        File.WriteAllText(filedestination, jsonString);
        Debug.Log("File sucessfully saved");
    }

    public OptionTemplate LoadFile()
    {
       OptionTemplate data = null;
        if (File.Exists(filedestination))
        {
            string jsonString = File.ReadAllText(filedestination);
            data = JsonUtility.FromJson<OptionTemplate>(jsonString);
            Debug.Log("data loaded");
        }
        else
        {
            SaveFile();
            Debug.Log("New File Created");
            return GameData.Instance.GameOptions;
        }
        return data;
    }
}
