using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BubbleController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float initialScale = 0.4f;
    [SerializeField] private int maxAssetLowerBound = 1000;
    [SerializeField] private int validRange = 100;
    [SerializeField] private float maxScale;

    public int currentAsset = 0;
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
        currentAsset += num;
        CheckValid();
    }

    //Control Signal - 2
    public void DecreaseAsset(int num)
    {
        currentAsset -= num;
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
}