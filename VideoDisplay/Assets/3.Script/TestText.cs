using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestText : MonoBehaviour
{
    public TMP_Text[] texts;
    public List<TMP_Text> TextcolorCashs = new List<TMP_Text>();
    public List<TMP_Text> sliderColorCashs = new List<TMP_Text>();
    public Image sliderFill;
    public Image sliderHandleFill;

    private void Start()
    {
        TextJson textData = JsonManager.instance.textJson;

        texts[0].enableAutoSizing = textData.isAutoSize_Ko;
        texts[2].enableAutoSizing = textData.isAutoSize_En;
        if (!textData.isAutoSize_Ko)
        {
            texts[0].fontSize = textData.fontSize_Ko;
        }
        if (!textData.isAutoSize_En)
        {
            texts[2].fontSize = textData.fontSize_En;
        }
        if (!textData.isAutoSize_Sub) 
        {
            texts[4].fontSize = textData.fontSize_sub;
        }
        else
        {
            texts[4].fontSize = texts[0].fontSize / 2;
        }

            texts[0].text = textData.koTitle;
        texts[2].text = textData.enTitle;
        texts[1].text = textData.koTitle2;
        texts[3].text = textData.enTitle2;
        texts[4].text = textData.enTitle3;
        float temp = texts[0].rectTransform.anchoredPosition.y - texts[0].rectTransform.sizeDelta.y;
        Debug.Log($"temp 크기 : {texts[0].rectTransform.sizeDelta.y}");
        StartCoroutine(TryGetSize_co());
        texts[4].rectTransform.anchoredPosition = new Vector2(texts[4].rectTransform.anchoredPosition.x, temp);
        ChangeColor(JsonManager.instance.textJson.SceneIndex);
        //ChangeColor(3);
    }
    private void Update()
    {
        float temp = texts[0].rectTransform.anchoredPosition.y - texts[0].rectTransform.sizeDelta.y;
        //Debug.Log($"temp 크기 : {texts[0].rectTransform.sizeDelta.y}");
    }
    public void ChangeColor(int index)
    {
        Color temp = TextcolorCashs[index].color;
        Debug.Log(temp);
        texts[0].color = TextcolorCashs[index].color;
        texts[2].color = TextcolorCashs[index].color;
        sliderFill.color = sliderColorCashs[index].color;
        sliderHandleFill.color = sliderColorCashs[index].color;
    }
    IEnumerator TryGetSize_co()
    {
        yield return new WaitForEndOfFrame();

            if (texts[0].rectTransform.sizeDelta.y != 0)
            {
                float temp = texts[0].rectTransform.anchoredPosition.y - texts[0].rectTransform.sizeDelta.y;
                texts[4].rectTransform.anchoredPosition = new Vector2(texts[4].rectTransform.anchoredPosition.x, temp);
                
            }
    }
}
