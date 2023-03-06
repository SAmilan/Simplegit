using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset m_GamersNameSheet;
    [SerializeField]
    List<string> m_PlayerNames = new List<string>();
    public static NameManager instane;

    private void Awake()
    {
        instane = this;
    }
   
    private void Start()
    {
        string[] nameData = m_GamersNameSheet.text.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        m_PlayerNames.AddRange(nameData);
        m_PlayerNames.RemoveAt(Constants.INT_ZERO);
    }

    public string GetPlayerName()
    {
        string singlePlayerName = m_PlayerNames[Random.Range(Constants.INT_ZERO, m_PlayerNames.Count - Constants.INT_ONE)];
        return singlePlayerName;
    }
    public static string GetPlayer1Name()
    {
        string name = Constants.GUEST_NAME;
        var args = System.Environment.GetCommandLineArgs();
        if(args.Length > Constants.INT_ONE)
        {
            name = args[1].ToString();
        }
        return name;
    }
    public static string GetPlayer2Name()
    {
        string name = Constants.GUEST_NAME;
        var args = System.Environment.GetCommandLineArgs();
        if (args.Length > Constants.INT_TWO)
        {
            name = args[2].ToString();
        }
        return name;
    }

}
