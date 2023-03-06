using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestmanager : MonoBehaviour
{
    #region ACTIONS
    public static Action<string, Action<Sprite>> s_GetSpriteFromURL;
    public static Action<string, Action<TextAsset>> s_GetTextAssetsFromURL;
    public static Action<string, Action<AudioClip>> s_GetAudioFromURL;
    #endregion

    #region UNITY_CALLBACK
    void Awake() => DontDestroyOnLoad(this);

    void OnEnable()
    {
        s_GetSpriteFromURL += GetSpriteFromURL;
        s_GetTextAssetsFromURL += GetTextAssetsFromURL;
        s_GetAudioFromURL += GetAudioFromURL;
    }

    void OnDisable()
    {
        s_GetSpriteFromURL -= GetSpriteFromURL;
        s_GetTextAssetsFromURL -= GetTextAssetsFromURL;
        s_GetAudioFromURL -= GetAudioFromURL;
    }
    #endregion

    #region Get Sprite
    private void GetSpriteFromURL(string url, Action<Sprite> returnSprite)
    {
        Sprite _sprite = null;
        StartCoroutine(Coroutine());

        IEnumerator Coroutine()
        {
            yield return StartCoroutine(GetTexture(url,
                (string error) => { Debug.LogError(error); },
                (Texture2D tex) =>
                {
                    _sprite = Sprite.Create(tex, new Rect(Constant.ZERO, Constant.ZERO, tex.width, tex.height), Vector2.one * Constant.HALF, Constant.HUNDREAD);
                }));
            yield return new WaitForEndOfFrame();
            returnSprite?.Invoke(_sprite);
        }
    }

    IEnumerator GetTexture(string URL, Action<string> onError, Action<Texture2D> onSuccess)
    {
        using UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(URL);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            DownloadHandlerTexture handlerTexture = unityWebRequest.downloadHandler as DownloadHandlerTexture;
            onSuccess?.Invoke(handlerTexture.texture);
        }
        else
            onError?.Invoke(unityWebRequest.error);
    }
    #endregion

    #region Get Text Assets
    private void GetTextAssetsFromURL(string url, Action<TextAsset> returnTextAssets)
    {
        TextAsset textAsset = null;
        StartCoroutine(Coroutine());

        IEnumerator Coroutine()
        {
            yield return StartCoroutine(GetTextAssets(url,
                (string error) => { Debug.LogError(error); },
                (TextAsset text) => { textAsset = text; }));
            yield return new WaitForEndOfFrame();
            returnTextAssets?.Invoke(textAsset);
        }
    }

    IEnumerator GetTextAssets(string URL, Action<string> onError, Action<TextAsset> onSuccess)
    {
        using UnityWebRequest unityWebRequest = UnityWebRequest.Get(URL);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            TextAsset textAsset = new TextAsset(unityWebRequest.downloadHandler.text);
            onSuccess?.Invoke(textAsset);
        }
        else
            onError?.Invoke(unityWebRequest.error);
    }
    #endregion

    #region Get Audio Clip
    private void GetAudioFromURL(string url, Action<AudioClip> returnSprite)
    {
        AudioClip audioClip = null;
        StartCoroutine(Coroutine());

        IEnumerator Coroutine()
        {
            yield return StartCoroutine(GetAudio(url,
                (string error) => { Debug.LogError(error); },
                (AudioClip clip) => { audioClip = clip; }));
            yield return new WaitForEndOfFrame();
            returnSprite?.Invoke(audioClip);
        }
    }

    IEnumerator GetAudio(string URL, Action<string> onError, Action<AudioClip> onSuccess)
    {
        using UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(URL, AudioType.UNKNOWN);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            DownloadHandlerAudioClip downloadHandlerAudio = unityWebRequest.downloadHandler as DownloadHandlerAudioClip;
            onSuccess?.Invoke(downloadHandlerAudio.audioClip);
        }
        else
            onError?.Invoke(unityWebRequest.error);
    }
    #endregion
}