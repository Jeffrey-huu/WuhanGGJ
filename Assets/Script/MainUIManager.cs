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
    [SerializeField] private float lerpSpeed = 5f;  // ��ֵ��ƽ���ٶ�

    private void Start()
    {
        // ��ʼ����ʾֵΪ��ǰ�ʲ�
        currentDisplayedValue = bubbleController.currentAsset;
    }

    void Update()
    {
        // ��̬��ֵ������ʾֵ
        currentDisplayedValue = Mathf.Lerp(currentDisplayedValue, bubbleController.currentAsset, Time.deltaTime * lerpSpeed);

        // ����UI��ʾ
        bubbleValue.text = Mathf.RoundToInt(currentDisplayedValue).ToString();
    }
}
