using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BubbleController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float initialScale = 0.4f;
    [SerializeField] private int maxAssetLowerBound = 1000;
    [SerializeField] private int validRange = 100;
    [SerializeField] private float maxScale;

    private bool isPressed = false;
    private float pressStartTime = 0f;
    [SerializeField] private float maxPressDuration = 3.0f;

    public int currentAsset => personAsset + marketAsset;
    public int personAsset = 15;
    public int marketAsset = 100;

    private float lerpSpeed = 0.1f;
    private int additiveAsset;

    private bool isNearBurst = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        additiveAsset = Random.Range(0, validRange);
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        Debug.Log(additiveAsset);
        UpdateBubbleScale();
    }

    void FixedUpdate()
    {
        UpdateBubbleScale();
    }

    private void Update()
    {
        CheckIsNearBurst();
    }

    private void BubbleAnimationTrigger()
    {
        Destroy(gameObject);
    }

    private void CheckIsNearBurst()
    {
        if (!isNearBurst && currentAsset >= maxAssetLowerBound)
        {
            isNearBurst = true;
            anim.SetBool("isNearBurst", isNearBurst);
        }

        if (isNearBurst && currentAsset < maxAssetLowerBound)
        {
            isNearBurst = false;
            anim.SetBool("isNearBurst", isNearBurst);
        }
    }

    //Control Signal - 1
    public void AddAsset(int num)
    {
        personAsset += num;
        CheckValid();
    }

    public void AddMarket(int num)
    {
        marketAsset += num;
        CheckValid();
    }

    //Control Signal - 2
    public void DecreaseAsset(int num)
    {
        personAsset -= num;
        CheckValid();
    }

    public void DecreaseMarket(int num)
    {
        marketAsset -= num;
        CheckValid();
    }

    private void UpdateBubbleScale()
    {
        float additiveScale = (float)currentAsset / maxAssetLowerBound * (maxScale - initialScale);
        float targetScale = additiveScale + initialScale;

        if (targetScale > maxScale)
            targetScale = maxScale;

        if (additiveScale > (maxScale - initialScale) / 2)
            anim.speed = 1.5f;

        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetScale, targetScale, targetScale), lerpSpeed);
    }

    private void CheckValid()
    {
        if (currentAsset < maxAssetLowerBound)
        {
            return;
        }

        Debug.Log(currentAsset);

        if (currentAsset > maxAssetLowerBound + additiveAsset)
        {
            anim.SetBool("isBurst", true);
        }
    }

    //平移到目标点
    public void SmoothMove(Vector3 destination)
    {
        destination.z = 0;
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * 5);
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

    protected virtual void OnLongPress(float duration)
    {
        SendLongPressDuration(duration);
    }

    private void SendLongPressDuration(float duration)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                GameObject target = result.gameObject;
                var asset = target.GetComponent<UI_Asset>();
                if (asset != null)
                {
                    float useScale = duration / maxPressDuration;
                    int usedAsset = Mathf.RoundToInt(useScale * currentAsset);
                    asset.AddAsset(usedAsset);
                    DecreaseAsset(usedAsset);
                }
            }
        }
    }
}
