using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUIManager : MonoBehaviour
{
    public BubbleController bubbleController;
    public TextMeshProUGUI bubbleValue;

    private float currentDisplayedValue;
    [SerializeField] private float lerpSpeed = 5f;  // 插值的平滑速度

    private void Start()
    {
        // 初始化显示值为当前资产
        currentDisplayedValue = bubbleController.currentAsset;
    }

    void Update()
    {
        // 动态插值更新显示值
        currentDisplayedValue = Mathf.Lerp(currentDisplayedValue, bubbleController.currentAsset, Time.deltaTime * lerpSpeed);

        // 更新UI显示
        bubbleValue.text = Mathf.RoundToInt(currentDisplayedValue).ToString();
    }
}
