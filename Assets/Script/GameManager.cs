using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SettlementUI settlementUI;
    public UI_Asset uI_Asset;
    public BubbleController bubbleController;
    public EventUIManager eventUIManager;
    public EventManager eventManager;
    public GameObject eventgroup;
    public int emotionValueLT;
    public int assetLT;
    public int RoundNum;

    void Start()
    {
        RoundNum = 0;
        assetLT = uI_Asset.currentAsset;
        emotionValueLT = EmotionBar.instance.emotionValue;
    }

    public void NextRound()
    {
        settlementUI.ShowUI();
        assetLT = uI_Asset.currentAsset;
        emotionValueLT = EmotionBar.instance.emotionValue;

        AudioSystem.instance.PlayNextTurnSound();
        Debug.Log("next turn");

        eventUIManager.GenerateEvent();
        RoundNum++;
    }

    public void Victory()
    {
        settlementUI.ShowUI();
        settlementUI.Succeed();
        Debug.Log("victory");
    }
    public void Defeat()
    {
        settlementUI.ShowUI();
        settlementUI.Fail();
        Debug.Log("defeat");
    }
}
