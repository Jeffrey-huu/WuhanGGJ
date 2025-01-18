using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 右下角资产UI
public class UI_Asset : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPressed = false;
    private float pressStartTime = 0f;
    static UI_Asset instance;

    public RectTransform tr;
    //最大值时的y轴坐标和高度
    [SerializeField] private float maxY = -175;
    [SerializeField] private float minY = -515;
    [SerializeField] private float maxH = 720;


    // 用户当前资产
    [SerializeField] private int currentAsset = 1000;
    [SerializeField] private float maxPressDuration = 3.0f;
    [SerializeField] private float targetAsset= 2000;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
        instance = this;
        UpdateAnim();
    }

    void UpdateAnim()
    {
        float height=(currentAsset/targetAsset)*maxH;
        // float height=Mathf.Lerp(tr.sizeDelta.x,(currentAsset/targetAsset)*maxH,Time.deltaTime*5);
        float scale = (maxY-minY)/maxH;
        float y =minY + height*scale;
        // float y = Mathf.Lerp(tr.anchoredPosition.y,minY + height*scale,Time.deltaTime*5);
        tr.anchoredPosition = new Vector2(tr.anchoredPosition.x, y);
        tr.sizeDelta = new Vector2(tr.sizeDelta.x, height);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pressStartTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed)
        {
            isPressed = false;
            float longPressDuration = Time.time - pressStartTime;
            longPressDuration = Mathf.Clamp(longPressDuration, 0, maxPressDuration);
            OnLongPress(longPressDuration);
        }
    }

    public void AddAsset(int num)
    {
        currentAsset = (int)Mathf.Clamp(currentAsset+num,0,targetAsset);
        UpdateAnim();
        if(currentAsset > targetAsset)
        {
            Debug.Log("Game WIN!!!");
        }
    }

    public void DecreaseAsset(int num)
    {
        currentAsset = (int)Mathf.Clamp(currentAsset-num,0,targetAsset);
        UpdateAnim();
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
                DecreaseAsset(usedAsset);
            }
        }
    }
}