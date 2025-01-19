using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SubBubbleController : MonoBehaviour 
{
    private SpriteRenderer sr;
    private Animator anim;

    public GameObject target; 

    public float mergeDistance = 10f;
    public int addAsset;

    static public bool BreakAble = false;


    [SerializeField] private float initialScale = 0.8f;
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



    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        additiveAsset = Random.Range(0, validRange);
    }

    void CheckIsCollided()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position)
                    -BubbleController.instance.GetRendererRadius();
            Debug.Log($"Distance to target: {distance}");

            // 你可以在这里添加其他逻辑，比如判断是否在某个范围内
            if (distance <= mergeDistance)
            {
                Debug.Log("Target is within merge distance!");
                Destroy(gameObject);
                // 执行合并逻辑
            }
        }
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "TargetBubble" && BreakAble)
        {
            Debug.Log("Collision detected!");
            Debug.Log("Target is within merge distance!");
            BubbleController.instance.AddAsset(addAsset);
            Destroy(gameObject);
            // CheckIsCollided();
        }
    }

    private void Update()
    {
        // UpdateBubbleScale();
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

    public void OnPointerExit(PointerEventData eventData)
    {
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
                    float useScale = duration / maxPressDuration;
                    int usedAsset = Mathf.RoundToInt(useScale * maxAssetCanUseOneTrans);
                    usedAsset = Mathf.Clamp(usedAsset, 0, personAsset);
                    assetBar.asset_ui.AddAsset(usedAsset);
                }
            }
        }
    }
}
