using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ���½��ʲ�UI
public class UI_TotalAsset : MonoBehaviour
{
    public EventManager eventManager;
    public UI_Asset pocessAsset;
    public BubbleController bc;

    public RectTransform tr;
    //���ֵʱ��y������͸߶�
    [SerializeField] private float maxY = -175;
    [SerializeField] private float minY = -515;
    [SerializeField] private float maxH = 720;

    // �û���ǰ�ʲ�
    [SerializeField] public int currentAsset = 1000;
    [SerializeField] private float targetAsset = 2000;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        currentAsset = bc.personAsset + pocessAsset.currentAsset;
        UpdateAnim();
    }

    void UpdateAnim()
    {
        float height = (currentAsset / targetAsset) * maxH;

        // ʹ��Lerpƽ�����ɲ���������sizeDelta
        float newWidth = tr.sizeDelta.x;  // ���ֵ�ǰ��Ȳ���
        float newHeight = Mathf.Lerp(tr.sizeDelta.y, height, Time.deltaTime * 5);
        tr.sizeDelta = new Vector2(newWidth, newHeight);  // ��������sizeDelta

        // ����Ŀ��yλ��
        float scale = (maxY - minY) / maxH;
        float targetY = minY + height * scale;

        // ʹ��Lerpƽ�����ɲ���������anchoredPosition
        float newX = tr.anchoredPosition.x;  // ���ֵ�ǰxλ�ò���
        float newY = Mathf.Lerp(tr.anchoredPosition.y, targetY, Time.deltaTime * 5);
        tr.anchoredPosition = new Vector2(newX, newY);  // ��������anchoredPosition
    }
}