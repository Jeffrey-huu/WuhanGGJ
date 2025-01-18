using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 右下角资产UI
public class UI_Asset : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private bool isPressed = false;
    private float pressStartTime = 0f;
    static UI_Asset instance;
    public EventManager eventManager;

    public Slider progressBar;

    public RectTransform tr;
    //最大值时的y轴坐标和高度
    [SerializeField] private float maxY = -175;
    [SerializeField] private float minY = -515;
    [SerializeField] private float maxH = 720;


    // 用户当前资产
    [SerializeField] private int currentAsset = 1000;
    [SerializeField] private float maxPressDuration = 1.0f;
    [SerializeField] private float targetAsset = 2000;
    private float longPressDuration = 0;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
        instance = this;
    }

    private void FixedUpdate()
    {
        UpdateAnim();

        if (isPressed)
        {
            progressBar.value = (Time.time - pressStartTime) / maxPressDuration;
        }
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


    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pressStartTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        progressBar.value = 0;
        OnLongPress(longPressDuration);
        longPressDuration = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
        longPressDuration = Time.time - pressStartTime;
        longPressDuration = Mathf.Clamp(longPressDuration, 0, maxPressDuration);
    }

    public void AddAsset(int num)
    {
        currentAsset = (int)Mathf.Clamp(currentAsset+num,0,targetAsset);

        if(currentAsset > targetAsset)
        {
            Debug.Log("Game WIN!!!");
        }
    }

    public void DecreaseAsset(int num)
    {
        currentAsset = (int)Mathf.Clamp(currentAsset-num,0,targetAsset);
    }

    protected virtual void OnLongPress(float duration)
    {
        SendLongPressDuration(duration);
    }

    private void SendLongPressDuration(float duration)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
        {
            GameObject target = hit.collider.gameObject;
            BubbleController bubbleController = target.GetComponent<BubbleController>();
            if (bubbleController != null)
            {
                float useScale = duration / maxPressDuration;
                int usedAsset = Mathf.RoundToInt(useScale * currentAsset);
                bubbleController.AddAsset(usedAsset);
                DecreaseAsset(usedAsset);
            }
            EventController eventController = target.GetComponent<EventController>();
            if (eventController != null)
            {
                int eventindex = eventController.eventindex;
                float useScale = duration / maxPressDuration;
                int usedAsset = Mathf.RoundToInt(useScale * currentAsset);
                eventManager.AddAssetToEvent(eventindex, usedAsset);
                DecreaseAsset(usedAsset);
            }
        }
    }
}