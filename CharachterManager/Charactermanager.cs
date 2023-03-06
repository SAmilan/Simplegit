using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Charactermanager : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic skeletonGraphic;
    [SerializeField] private GameObject CharacterObject;

    [SerializeField] private List<ActorSkeletonDataHolder> ActorSkeletonDatas = new();

    public static Action<string, string, string> s_PlayActorAnim;

    void Awake() => OnCreateSpineAnimation();

    void OnEnable() => s_PlayActorAnim += OnActorAnim;

    void OnDisable() => s_PlayActorAnim -= OnActorAnim;

    private void OnCreateSpineAnimation()
    {
        List<ActorDataHolder> actorDatas = DataManager.instance.actorDatas;
        for (int actorIndex = Constant.ZERO; actorIndex < actorDatas.Count; actorIndex++)
        {
            ActorDataHolder actorData = actorDatas[actorIndex];
            //create material
            Material material = new(Shader.Find("Spine/Skeleton"));
            Texture2D texture = actorData.sprite.texture;
            texture.name = Path.GetFileNameWithoutExtension(actorData.actor.asset.files[Constant.TWO].url);
            material.mainTexture = texture;

            //create spine atlas
            SpineAtlasAsset spineAtlasAsset = new()
            {
                atlasFile = actorData.atlasText,
                materials = new Material[] { material }
            };

            ActorSkeletonDatas.Add(new(actorData.actor.title,
                SkeletonDataAsset.CreateRuntimeInstance(actorData.resource, new AtlasAssetBase[] { spineAtlasAsset }, true, 0.01f)));
        }
    }

    private void OnActorAnim(string type, string actorName, string animState)
    {
        if (type == "actor")
        {
            skeletonGraphic.skeletonDataAsset = ActorSkeletonDatas.Find(actorData => actorData.ActorName == actorName).ActorSkeleton;
            skeletonGraphic.Initialize(true);
            CharacterObject.SetActive(true);
            skeletonGraphic.AnimationState.SetAnimation(Constant.ZERO, animState, true);
        }
        else CharacterObject.SetActive(false);
    }
}
