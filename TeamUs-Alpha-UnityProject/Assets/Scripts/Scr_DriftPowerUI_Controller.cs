using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_DriftPowerUI_Controller : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private List<Image> boxes;
    [SerializeField] private Scr_Movement_Controller scr_Movement_Controller;
    private float percent;


    [Header("Shape Settings")]
    [SerializeField] private float totalWidthInPixels;
    [SerializeField] private float heightInPixels;
    [SerializeField] private float spacingInPixels;

    void Start()
    {
        //Size and seperate boxes based on paramaters
        float boxesWidthInPixels = totalWidthInPixels - spacingInPixels * boxes.Count;
        float percentTreshold = scr_Movement_Controller.driftPercentTresholdRead;
        float box0Width = boxesWidthInPixels * percentTreshold;
        float otherBoxWidth = (boxesWidthInPixels - box0Width) / (boxes.Count - 1);
        boxes[0].rectTransform.sizeDelta = new Vector2(box0Width, heightInPixels * .3f);
        for (int i = 1; i < boxes.Count; i++)
        {
            boxes[i].rectTransform.sizeDelta = new Vector2(otherBoxWidth, heightInPixels);
            boxes[i].rectTransform.localPosition = new Vector2(i * spacingInPixels + (i - 1) * otherBoxWidth + box0Width, 0);
        }
    }
    void Update()
    {
        percent = scr_Movement_Controller.driftPercentRead;
        for (int i = 0; i < boxes.Count; i++)//Fill in masks according to drifting percentage
        {
            Image fill = boxes[i].transform.GetChild(0).GetComponent<Image>();
            float startPosInPixels = boxes[i].rectTransform.localPosition.x;
            Vector2 fillScaleInPixels = new Vector2(boxes[i].rectTransform.rect.width, boxes[i].rectTransform.rect.height);

            float boxPercentStart = (startPosInPixels / totalWidthInPixels);
            float boxPercentEnd = (startPosInPixels + fillScaleInPixels.x) / totalWidthInPixels;
            float myPercent = Mathf.Clamp((percent - boxPercentStart) / (boxPercentEnd - boxPercentStart), 0, 1);
            float width = Mathf.Lerp(0, fillScaleInPixels.x, myPercent);

            fill.rectTransform.sizeDelta = new Vector2(width, fillScaleInPixels.y);
        }
    }
}
