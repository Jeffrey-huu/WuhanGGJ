using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] public mevent[] eventarr;

    private List<Buff> activeBuffs = new List<Buff>();

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
            ActivateBuff(5, (int)1.5 * asset);
        }
        if (eventarr[eventindex].isfinished == false)
        {
            eventarr[eventindex].isfinished = true;
            finishedNum++;
        }
        else if (eventName == "生物科技革命：延长寿命的突破")
        {
            ActivateBuff(3, (int)2.5 * asset);
        }
        else if (eventName == "全球虚拟现实经济的形成")
        {
            if (EmotionBar.instance.emotionValue < 60)
            {
                EmotionBar.instance.AddEmotion((int)0.2 * asset);
            }
            else
            {
                EmotionBar.instance.AddEmotion((int)0.1 * asset);
            }
        }
        else if (eventName == "太空矿业的大规模发展")
        {
            bubbleController.AddAsset(5 * asset);
        }


    }

    public void BuffEffect()
    {
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            Buff buff = activeBuffs[i];
            uI_Asset.AddAsset(buff.extraScore);
            buff.duration--;

            if (buff.duration <= 0)
            {
                activeBuffs.RemoveAt(i);
            }
        }
    }
    void ActivateBuff(int duration, int extraScore)
    {
        Buff newBuff = new Buff(duration, extraScore);
        activeBuffs.Add(newBuff);
    }
}

public class Buff
{
    public int duration;
    public int extraScore;

    public Buff(int duration, int extraScore)
    {
        this.duration = duration;
        this.extraScore = extraScore;
    }
}