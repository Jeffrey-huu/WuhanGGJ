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

    void Start()
    {
        for (int i = eventarr.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1); // 生成随机索引
            // 交换当前元素和随机元素
            mevent temp = eventarr[i];
            eventarr[i] = eventarr[randomIndex];
            eventarr[randomIndex] = temp;
        }
    }
    public void AddAssetToEvent(int eventindex, int asset)
    {
        if (eventarr[eventindex].isfinished == false)
        {
            eventarr[eventindex].isfinished = true;
            finishedNum++;
        }
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
        else if (eventName == "全球共享经济网络")
        {
            ActivateBuff(5, (int)1.5 * asset);
        }
        else if (eventName == "生物科技革命")
        {
            ActivateBuff(3, (int)2.5 * asset);
        }
        else if (eventName == "全球虚拟现实经济")
        {
            if (EmotionBar.instance.emotionValue < 60)
            {
                EmotionBar.instance.AddEmotion((int)(0.2 * asset));
            }
            else
            {
                EmotionBar.instance.AddEmotion((int)(0.1 * asset));
            }
        }
        else if (eventName == "太空矿业的大规模发展")
        {
            bubbleController.AddAsset(5 * asset);
        }
        else if (eventName == "AI叛变导致经济瘫痪")
        {
            if (EmotionBar.instance.emotionValue >= 80)
            {
                bubbleController.AddAsset((int)(0.8 * asset));
            }
            else
            {
                bubbleController.AddMarket(10 * asset);

            }
        }
        else if (eventName == "大规模网络攻击")
        {
            EmotionBar.instance.AddEmotion((int)(0.05 * asset));
        }
        else if (eventName == "气候灾难导致粮食危机")
        {
            bubbleController.AddAsset(2 * asset);
        }
        else if (eventName == "基因武器的滥用")
        {
            if (asset >= 20)
            {
                bubbleController.AddAsset((int)(1.5 * asset));
            }
            else
            {
                EmotionBar.instance.DecreaseEmotion((int)(0.2 * asset));
            }
        }
        else if (eventName == "外太空殖民失败的连锁反应")
        {
            EmotionBar.instance.AddEmotion((int)(0.05 * asset));
        }


    }
    public void EventOccur(int eventindex)
    {
        string eventName = eventarr[eventindex].name;
        if (eventName == "AI叛变导致经济瘫痪")
        {
            bubbleController.DecreaseMarket(100);
        }
        else if (eventName == "大规模网络攻击")
        {
            bubbleController.DecreaseMarket(100);
        }
        else if (eventName == "气候灾难导致粮食危机")
        {
            EmotionBar.instance.DecreaseEmotion(20);
        }
        else if (eventName == "基因武器的滥用")
        {
            EmotionBar.instance.AddEmotion(10);
        }
        else if (eventName == "外太空殖民失败的连锁反应")
        {
            bubbleController.DecreaseMarket((int)(bubbleController.marketAsset * 0.1));
        }
    }

    public void EventEnd(int eventindex)
    {
        if (eventarr[eventindex].isfinished)
        {
            return;
        }
        string eventName = eventarr[eventindex].name;
        if (eventName == "AI叛变导致经济瘫痪")
        {
            EmotionBar.instance.DecreaseEmotion(10);
        }
        else if (eventName == "大规模网络攻击")
        {
            EmotionBar.instance.DecreaseEmotion(10);
        }
        else if (eventName == "气候灾难导致粮食危机")
        {
            bubbleController.DecreaseAsset((int)(bubbleController.personAsset * 0.1));
        }
        else if (eventName == "基因武器的滥用")
        {
            bubbleController.DecreaseAsset((int)(bubbleController.marketAsset * 0.1));
        }
        else if (eventName == "外太空殖民失败的连锁反应")
        {
            EmotionBar.instance.DecreaseEmotion(10);
        }
        eventarr[eventindex].isfinished = true;
        finishedNum++;
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