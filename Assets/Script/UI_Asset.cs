using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 右下角资产UI
public class UI_Asset : MonoBehaviour
{
    public GameManager gameManager;
    public EventManager eventManager;

    public UI_TotalAsset totalAsset;

    public RectTransform tr;
    //最大值时的y轴坐标和高度
    [SerializeField] private float maxY = -175;
    [SerializeField] private float minY = -515;
    [SerializeField] private float maxH = 720;

    // 用户当前资产
    [SerializeField] public int currentAsset = 1000;
    [SerializeField] private float targetAsset = 2000;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        UpdateAnim();
    }

    void UpdateAnim()
    {
        float height = (currentAsset / targetAsset) * maxH;

        // 使用Lerp平滑过渡并更新整个sizeDelta
        float newWidth = tr.sizeDelta.x;  // 保持当前宽度不变
        float newHeight = Mathf.Lerp(tr.sizeDelta.y, height, Time.deltaTime * 5);
        tr.sizeDelta = new Vector2(newWidth, newHeight);  // 更新整个sizeDelta

        // 计算目标y位置
        float scale = (maxY - minY) / maxH;
        float targetY = minY + height * scale;

        // 使用Lerp平滑过渡并更新整个anchoredPosition
        float newX = tr.anchoredPosition.x;  // 保持当前x位置不变
        float newY = Mathf.Lerp(tr.anchoredPosition.y, targetY, Time.deltaTime * 5);
        tr.anchoredPosition = new Vector2(newX, newY);  // 更新整个anchoredPosition
    }

    public void AddAsset(int num)
    {
        currentAsset = (int)Mathf.Clamp(currentAsset + num, 0, targetAsset);

        if (totalAsset.currentAsset > targetAsset)
        {
            gameManager.Victory();
        }
    }

    public void DecreaseAsset(int num)
    {
        currentAsset = (int)Mathf.Clamp(currentAsset - num, 0, targetAsset);
    }
}