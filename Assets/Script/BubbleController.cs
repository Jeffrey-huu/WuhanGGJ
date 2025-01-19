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
    public GameManager gameManager;
    private SpriteRenderer sr;
    private Animator anim;

    public Slider progressBar;

    static public BubbleController instance;

    [SerializeField] private float initialScale = 0.4f;
    [SerializeField] private int maxAssetLowerBound = 1000;
    [SerializeField] private int validRange = 100;
    [SerializeField] private float maxScale;

    private bool isPressed = false;
    private bool isEnter = false;
    private float pressStartTime = 0f;
    [SerializeField] private float maxPressDuration = 1.0f;
    private float longPressDuration = 0;

    public int currentAsset => personAsset + marketAsset;
    public int personAsset = 15;
    public int marketAsset = 100;
    [SerializeField] private int maxAssetCanUseOneTrans = 45;

    private float lerpSpeed = 0.1f;
    private int additiveAsset;

    private bool isNearBurst = false;

    public float radius ;

    void Awake()
    {
        instance = this;
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
    }


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        additiveAsset = Random.Range(0, validRange);
        transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        Debug.Log(additiveAsset);
        UpdateBubbleScale();

        transform.position = new Vector3(0, 0, 0);

        AudioSystem.instance.PlayBubbleIdleSound();
    }

    private void Update()
    {
        //始终维持位置，防止rigidBody作怪
        transform.position = new Vector3(0, 0, 0);

        CheckIsNearBurst();
        UpdateBubbleScale();

        if(currentAsset>200)
        {
            SubBubbleController.BreakAble=true;
        }

        if (isPressed)
        {
            progressBar.value = (Time.time - pressStartTime) / maxPressDuration;
            if (personAsset < maxAssetCanUseOneTrans)
            {
                float maxScale = (float)personAsset / maxAssetCanUseOneTrans;
                progressBar.value = (progressBar.value > maxScale ? maxScale : progressBar.value);
            }
            float scale = progressBar.value;
            SmallBubble.instance.SetScale(scale);
        }
        GetRendererRadius();
    }

    public float GetRendererRadius()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }

        if (sr != null)
        {
            Bounds bounds = sr.bounds;
            Vector3 extents = bounds.extents;
            radius = Mathf.Max(extents.x, extents.y, extents.z);
        }
        else
        {
            radius = 0f;
            Debug.LogWarning("SpriteRenderer is not found on this GameObject.");
        }
        return radius;
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
        if (currentAsset < 0)
        {
            gameManager.Defeat();
            marketAsset = 0;
            personAsset = 0;
            return;
        }
        if (currentAsset < maxAssetLowerBound)
        {
            return;
        }

        Debug.Log(currentAsset);

        if (currentAsset > maxAssetLowerBound + additiveAsset)
        {
            anim.SetBool("isBurst", true);
            AudioSystem.instance.PlayBubbleBurstSound();
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
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                GameObject target = result.gameObject;
                var assetBar = target.GetComponent<UI_AssetBar>();
                if (assetBar != null)
                {
                    AudioSystem.instance.PlayAssetPutDownSound();
                    float useScale = duration / maxPressDuration;
                    int usedAsset = Mathf.RoundToInt(useScale * maxAssetCanUseOneTrans);
                    usedAsset = Mathf.Clamp(usedAsset, 0, personAsset);
                    assetBar.asset_ui.AddAsset(usedAsset);
                    DecreaseAsset(usedAsset);
                }
            }
        }
    }
}
