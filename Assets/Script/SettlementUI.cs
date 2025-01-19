using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettlementUI : MonoBehaviour
{
    public BubbleController bubbleController;
    public GameManager gameManager;
    public EventManager eventManager;
    public UI_Asset uI_Asset;
    public Canvas settlementUI;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text_total;
    public GameObject eventgroup;
    void Start()
    {
        settlementUI.gameObject.SetActive(false);
    }
    public void ShowUI()
    {
        settlementUI.gameObject.SetActive(true);
        UpdateText();
    }
    public void UpdateText()
    {
        int[] eventindexs = new int[2];
        int i = 0;
        foreach (Transform _event in eventgroup.transform)
        {
            int eventindex = _event.GetComponent<EventController>().eventindex;
            eventindexs[i] = eventindex;
            i++;
        }
        if (eventManager.eventarr[eventindexs[0]].isfinished)
        {
            text1.text = eventManager.eventarr[eventindexs[0]].settlements[0];
        }
        else
        {
            eventManager.eventarr[eventindexs[0]].isfinished = true;
            eventManager.finishedNum++;
            text1.text = eventManager.eventarr[eventindexs[0]].settlements[1];
        }
        if (eventManager.eventarr[eventindexs[1]].isfinished)
        {
            text2.text = eventManager.eventarr[eventindexs[1]].settlements[0];
        }
        else
        {
            eventManager.eventarr[eventindexs[1]].isfinished = true;
            eventManager.finishedNum++;
            text2.text = eventManager.eventarr[eventindexs[1]].settlements[1];
        }
        string deltaEmotionValue = (EmotionBar.instance.emotionValue - gameManager.emotionValueLT).ToString();
        string deltaAsset = (uI_Asset.currentAsset - gameManager.assetLT).ToString();
        text_total.text = $"虚妄之眼AI统计: \n世界情绪值变化: {deltaEmotionValue}, 个人资产变化: {deltaAsset}";
    }
    public void CloseUI()
    {
        settlementUI.gameObject.SetActive(false);
        uI_Asset.AddAsset(bubbleController.currentAsset / 5);
        bubbleController.AddAsset(EmotionBar.instance.emotionValue * 3);
        foreach (Transform _event in eventgroup.transform)
        {
            int eventindex = _event.GetComponent<EventController>().eventindex;
            eventManager.EventEnd(eventindex);
        }
        eventManager.BuffEffect();

    }
}
