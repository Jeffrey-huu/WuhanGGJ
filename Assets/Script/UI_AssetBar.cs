using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AssetBar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private bool isPressed = false;
    private float pressStartTime = 0f;
    public UI_Asset asset_ui;
    public EventManager eventManager;

    public Slider progressBar;


    // �û���ǰ�ʲ�
    [SerializeField] private float maxPressDuration = 2.0f;
    [SerializeField] private int maxAssetCanUseOneTrans = 45;
    private float longPressDuration = 0;
    private bool isEnter = false;


    private void FixedUpdate()
    {
        if (isPressed)
        {
            progressBar.value = (Time.time - pressStartTime) / maxPressDuration;
            if (asset_ui.currentAsset < maxAssetCanUseOneTrans)
            {

                float maxScale = (float)asset_ui.currentAsset / maxAssetCanUseOneTrans;
                progressBar.value = (progressBar.value > maxScale ? maxScale : progressBar.value);
            }
            SmallBubble.instance.SetScale(progressBar.value);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioSystem.instance.PlayLongPressSound();
        isPressed = true;
        isEnter = true;
        pressStartTime = Time.time;
        SmallBubble.instance.SetVisiable(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isEnter)
        {
            OnPointerExit(eventData);
        }
        progressBar.value = 0;
        OnLongPress(longPressDuration);
        longPressDuration = 0;
        SmallBubble.instance.SetVisiable(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AudioSystem.instance.StopPlayLongPressSound();
        isPressed = false;
        isEnter = false;
        longPressDuration = Time.time - pressStartTime;
        longPressDuration = Mathf.Clamp(longPressDuration, 0, maxPressDuration);
    }

    protected virtual void OnLongPress(float duration)
    {
        SendLongPressDuration(duration);
    }

    private void SendLongPressDuration(float duration)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                GameObject target = hit.collider.gameObject;
                BubbleController bubbleController = target.GetComponent<BubbleController>();
                if (bubbleController != null)
                {
                    AudioSystem.instance.PlayAssetPutDownSound();
                    float useScale = duration / maxPressDuration;
                    int usedAsset = Mathf.RoundToInt(useScale * maxAssetCanUseOneTrans);
                    usedAsset = Mathf.Clamp(usedAsset, 0, asset_ui.currentAsset);
                    bubbleController.AddAsset(usedAsset);
                    asset_ui.DecreaseAsset(usedAsset);
                }
                EventController eventController = target.GetComponent<EventController>();
                if (eventController != null)
                {
                    AudioSystem.instance.PlayAssetPutDownSound();
                    int eventindex = eventController.eventindex;
                    float useScale = duration / maxPressDuration;
                    int usedAsset = Mathf.RoundToInt(useScale * maxAssetCanUseOneTrans);
                    usedAsset = Mathf.Clamp(usedAsset, 0, asset_ui.currentAsset);
                    eventManager.AddAssetToEvent(eventindex, usedAsset);
                    asset_ui.DecreaseAsset(usedAsset);
                }
            }
        }
    }
}
