using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Asset : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPressed = false;
    private float pressStartTime = 0f;
    public EventManager eventManager;
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

    // ���������Ĳ���
    protected virtual void OnLongPress(float duration)
    {
        // ���ͳ�������ʱ��� BubbleController
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
            EventController eventController = target.GetComponent<EventController>();
            if (eventController != null)
            {
                int eventindex = eventController.eventindex;
                float useScale = duration / maxPressDuration;
                int usedAsset = Mathf.RoundToInt(useScale * currentAsset);
                eventManager.AddAssetToEvent(eventindex, usedAsset);
                currentAsset -= usedAsset;
            }
        }
    }
}