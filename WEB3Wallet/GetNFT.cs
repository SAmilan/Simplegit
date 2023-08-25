using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Threading.Tasks;

[System.Serializable]
public class NFTsAttributes
{
    public string trait_type;
    public string value;
}
[System.Serializable]
public class WeaponNFTMetaData
{
    public WeaponAttribute[] attributes;
}
[System.Serializable]
public class WeaponAttribute
{
    public string trait_type;
    public string value;
}


[System.Serializable]
public struct GameSmartContract
{
    public string contractAddress;
    public NFTType type;
}

public class GetNFT : MonoBehaviour
{

    #region Public_Vars
    public static GetNFT instance;
    public List<PlayerInventoryWeaponData> PlayerCurrentInventory = new List<PlayerInventoryWeaponData>();
  
    #endregion

    #region Private_Vars
    [SerializeField]
    private NFTType m_NFTType;
    private string m_OwnerAddress = null;
    public static string OwnerAddress => instance.m_OwnerAddress;
    [SerializeField]
    private List<GameSmartContract> m_OwnedNFts = new List<GameSmartContract>();
    [SerializeField]
    private List<GameSmartContract> m_OwnedWeaponsNFts = new List<GameSmartContract>();

    public List<NFT> nftsToLoad = new List<NFT>();
    public List<NFT> WeaponNftsToLoad = new List<NFT>();
    [SerializeField]
    private TextMeshProUGUI m_MetaData;
    [SerializeField]
    private bool m_IsConnected = false;
    [SerializeField] GameObject LoadingPanel;
    [SerializeField]
    private List<WeaponNft> m_WeaponData = new List<WeaponNft>();
    [SerializeField]
    private List<WeaponNFTMetaData> m_WeaponNftMetaData = new List<WeaponNFTMetaData>();
    #endregion

    #region Unity_callbacks

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        Prefab_ConnectWalletNative.s_OnAddressRecieved += OnAdressRecieved;
        Prefab_ConnectWallet.s_OnAddressRecieved += OnAdressRecieved;
    }

    private void OnDisable()
    {
        Prefab_ConnectWalletNative.s_OnAddressRecieved -= OnAdressRecieved;
        Prefab_ConnectWallet.s_OnAddressRecieved -= OnAdressRecieved;
    }

    #endregion

    /// <summary>
    /// Load NFTs
    /// myNFTs button click
    /// </summary>
    public async void LoadNFTs()
    {
        if (!m_IsConnected && m_OwnerAddress != null)
            return;
        nftsToLoad.Clear();
        m_WeaponNftMetaData.Clear();
        try
        {
            
            foreach (GameSmartContract ownedQuery in m_OwnedNFts)
            {
                Contract tempContract = ThirdwebManager.Instance.SDK.GetContract(ownedQuery.contractAddress);

                List<NFT> tempNFTList = await tempContract.ERC721.GetOwned(m_OwnerAddress);

                nftsToLoad.AddRange(tempNFTList);
                ShowListings();
            }

            foreach (GameSmartContract ownedQuery in m_OwnedWeaponsNFts)
            {
                Contract tempContract = ThirdwebManager.Instance.SDK.GetContract(ownedQuery.contractAddress);

                List<NFT> tempNFTList = await tempContract.ERC721.GetOwned(m_OwnerAddress);

               

                WeaponNftsToLoad.AddRange(tempNFTList);
              
            }
            foreach (NFT weaponNfts in WeaponNftsToLoad)
            {
                Debug.Log("The weapon nfts is: " + weaponNfts.metadata.ToString());
                Debug.Log("The weapon url is: " + weaponNfts.metadata.uri);
                string uri = weaponNfts.metadata.uri;
                string weaponDataJson = JsonUtility.ToJson(weaponNfts.metadata);
                Debug.Log("The Weapon Target Json is: "+ weaponDataJson);
                WeaponNFTMetaData weaponNftMetadata = JsonUtility.FromJson<WeaponNFTMetaData>(weaponDataJson);
                m_WeaponNftMetaData.Add(weaponNftMetadata);
                string currentWeaponName = "";
                foreach(WeaponAttribute attributes in weaponNftMetadata.attributes)
                {
                    if(attributes.trait_type == "Weapon Name")
                    {
                        currentWeaponName = attributes.value;
                    }
                }
                PlayerInventoryWeaponData currentData = GameData.Instance.PlayerWeaponData.Find(element => element.WeaponName == currentWeaponName);
                if (currentData != null)
                    PlayerCurrentInventory.Add(currentData);
                //StartCoroutine(LoadJSONFromURI2(weaponNfts.metadata.uri));
                // StartCoroutine(LoadJSONFromURI2(uri));
            }

            Debug.Log("NFTs loaded!");
            //SceneManager.LoadScene("PhotonRoomConnect");
            SceneManager.LoadScene("WeaponCustomization");

        }
        catch (Exception e)
        {
            print($"Error Loading OwnedQuery NFTs: {e.Message}");
        }
    }

    private async Task LoadJSONFromURI(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonString = www.downloadHandler.text;
                WeaponNft data = JsonUtility.FromJson<WeaponNft>(jsonString);
                PlayerInventoryWeaponData currentData = GameData.Instance.PlayerWeaponData.Find(element => element.WeaponName == data.name);
                m_WeaponData.Add(data);
                if (currentData != null)
                    PlayerCurrentInventory.Add(currentData);
            }
            else
            {
                Debug.LogError("Failed to load JSON: " + www.error);
            }
        }
    }

    private IEnumerator LoadJSONFromURI2(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonString = www.downloadHandler.text;
            WeaponNft data = JsonUtility.FromJson<WeaponNft>(jsonString);
            PlayerInventoryWeaponData currentData = GameData.Instance.PlayerWeaponData.Find(element => element.WeaponName == data.name);
            m_WeaponData.Add(data);
            if (currentData != null)
                PlayerCurrentInventory.Add(currentData);
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + www.error);
        }

        www.Dispose();
    }





    private void ShowListings()
    {
        int counter = 0;
        foreach (NFT nft in nftsToLoad)
        {
            m_MetaData.text += " " + nft.metadata.ToString();
            string json = JsonUtility.ToJson(nft);
            string nftJson = JsonUtility.ToJson(nft.metadata.attributes);

            Debug.Log("The attributes json is" + nft.metadata.ToString());
        }
    }

    private void OnAdressRecieved(string walletAddress)
    {
        m_IsConnected = true;
        LoadingPanel.SetActive(true);
        m_OwnerAddress = walletAddress;
        LoadNFTs();
        Debug.Log("The owner address is: " + walletAddress);
    }
}
