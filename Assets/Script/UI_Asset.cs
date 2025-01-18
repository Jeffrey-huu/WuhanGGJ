using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Asset : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPressed = false;
    private float pressStartTime = 0f;
    [SerializeField] private int currentAsset = 1000;
    [SerializeField] private float maxPressDuration = 3.0f;

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

    // 长按触发的操作
    protected virtual void OnLongPress(float duration)
    {
        // 发送长按持续时间给 BubbleController
        SendLongPressDurationToBubbleController(duration);
    }

    private void SendLongPressDurationToBubbleController(float duration)
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
                currentAsset -= usedAsset;
            }
        }
    }
}