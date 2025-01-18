using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] public mevent[] eventarr;

    public int finishedNum = 0;
    public UI_Asset uI_Asset;
    public BubbleController bubbleController;
    public void AddAssetToEvent(int eventindex, int asset)
    {
        Debug.Log(eventarr[eventindex].name + " add " + asset);
        string eventName = eventarr[eventindex].name;
        if (eventName == "清洁能源革命")
        {
            Debug.Log(asset);
            if (asset >= 20)
            {
                EmotionBar.instance.AddEmotion(5);
            }
            else
            {
                uI_Asset.AddAsset(6 * asset);
            }
        }
        else if (eventName == "AI驱动的全球共享经济网络")
        {

        }
        if (eventarr[eventindex].isfinished == false)
        {
            eventarr[eventindex].isfinished = true;
            finishedNum++;
        }

    }
}
