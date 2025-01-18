using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BubbleController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private SpriteRenderer sr;
    private Animator anim;

    public Slider progressBar;

    [SerializeField] private float initialScale = 0.4f;
    [SerializeField] private int maxAssetLowerBound = 1000;
    [SerializeField] private int validRange = 100;
    [SerializeField] private float maxScale;

    private bool isPressed = false;
    private float pressStartTime = 0f;
    [SerializeField] private float maxPressDuration = 1.0f;
    private float longPressDuration = 0;

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

        AudioSystem.instance.PlayBubbleIdleSound();
    }

    private void Update()
    {
        CheckIsNearBurst();
        UpdateBubbleScale();

        if (isPressed)
        {
            progressBar.value = (Time.time - pressStartTime) / maxPressDuration;
        }
    }

    private void AudioManage()
    {
        if (currentAsset >= maxAssetLowerBound)
        {
            AudioSystem.instance.PlayBubbleNearBurstSound();
        }
        else if (currentAsset >= maxAssetLowerBound / 2)
        {
            AudioSystem.instance.PlayBubbleFastIdleSound();
        }
        else
        {
            AudioSystem.instance.PlayBubbleIdleSound();
        }
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
        AudioManage();
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
        AudioManage();
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
        {
            anim.speed = 1.5f;
        } 
        else
        {
            anim.speed = 1f;
        }
            
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
            AudioSystem.instance.PlayGameFailedSound();
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
