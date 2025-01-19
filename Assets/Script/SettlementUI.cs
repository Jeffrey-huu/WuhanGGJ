using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public GameObject failImg;
    public GameObject SuccessImg;

    public bool gameOver=false;
    static public SettlementUI instance;

    void Awake()
    {
        instance = this;
        gameOver=false;
        gameObject.SetActive(false);
    }
    public void ShowUI()
    {
        gameObject.SetActive(true);
        settlementUI.gameObject.SetActive(true);
        UpdateText();
    }

    public void Fail()
    {
        if(gameOver)return;
        AudioSystem.instance.PlayGameFailedSound();
        gameOver=true;
        FailUI();
        // Invoke("FailUI", 1f);
    }

    public void FailUI()
    {
        HideText();
        gameObject.SetActive(true);
        failImg.SetActive(true);
        SuccessImg.SetActive(false);
    }

    public void Succeed()
    {
        if(gameOver)return;
        AudioSystem.instance.PlayGameSuccessfulSound();
        gameOver=true;
        SucceedUI();
        // Invoke("SucceedUI", 1f);
    }

    public void SucceedUI()
    {
        HideText();
        gameObject.SetActive(true);
        failImg.SetActive(false);
        SuccessImg.SetActive(true);
    }

    public void HideText()
    {
        text1.text = "";
        text2.text = "";
        text_total.text = "";
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
        HideText();
        //1.关闭游戏
        if(gameOver)
        {
            Debug.Log("Close UI");
            SceneManager.LoadScene(0);
            return;
        }

        //2.平时
        settlementUI.gameObject.SetActive(false);
        uI_Asset.AddAsset(bubbleController.currentAsset / 5);
        bubbleController.AddAsset(EmotionBar.instance.emotionValue * 3);
        AudioSystem.instance.PlayNextTurnSound();
        foreach (Transform _event in eventgroup.transform)
        {
            int eventindex = _event.GetComponent<EventController>().eventindex;
            eventManager.EventEnd(eventindex);
        }
        eventManager.BuffEffect();
    }
}
