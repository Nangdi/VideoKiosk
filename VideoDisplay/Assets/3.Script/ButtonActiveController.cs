using RenderHeads.Media.AVProVideo;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

[SerializeField]
public class BtnSize
{
    public Vector2 koPos;
    public Vector2 enPos;
    public Vector2 koSize;
    public Vector2 enSize;

    public BtnSize(Vector2 koPos, Vector2 koSize, Vector2 enPos, Vector2 enSize)
    {
        this.koPos = koPos;
        this.koSize = koSize;
        this.enPos = enPos;
        this.enSize = enSize;
    }
 }

public class ButtonActiveController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AVProVideoController videoController;
    [SerializeField] private LoclaizationManager loclaizationManager;

    [SerializeField] private RectTransform[] target;
    public List<Button> buttons = new List<Button>();
    public Button button1_BG;           // 체험 방법
    public Button button2_BG;           // 원리
    public Button button3_BG;           // 더알아보기
 
    private Button currentBtn;

    public List<BtnSize> btnsizes = new List<BtnSize>();   
    // Start is called before the first frame update
    void Start()
    {
        btnsizes.Add(new BtnSize(new Vector2(-255, 0), new Vector2(225, 70), new Vector2(-305, 0), new Vector2(140, 70)));
        btnsizes.Add(new BtnSize(new Vector2(0, 0), new Vector2(220, 70), new Vector2(-45, 0), new Vector2(260, 70)));
        btnsizes.Add(new BtnSize(new Vector2(260, 0), new Vector2(220, 70), new Vector2(245, 0), new Vector2(245, 70)));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentBtn != null)
                currentBtn.Select();
        }
    }
    public void ClearButtonSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
    // Update is called once per frame
    public void MappingBtn(int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if(i == index)
            {
                buttons[i].Select();
                currentBtn = buttons[i];
                break;
            }
        }
      
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].gameObject == eventData.selectedObject)
            {
                currentBtn = buttons[i];
            }
            else
            {
                if(currentBtn != null)
                EventSystem.current.SetSelectedGameObject(currentBtn.gameObject);
            }
        }
        Debug.Log($"currentBtn : {currentBtn}");
    }
    public void ToggleRect(Language language)
    {
        switch (language)
        {
            case Language.Korean:
                for (int i = 0; i < btnsizes.Count; i++)
                {
                    target[i].anchoredPosition = btnsizes[i].koPos;
                    target[i].sizeDelta = btnsizes[i].koSize;

                }
                break;
            case Language.English:
                for (int i = 0; i < btnsizes.Count; i++)
                {
                    target[i].anchoredPosition = btnsizes[i].enPos;
                    target[i].sizeDelta = btnsizes[i].enSize;

                }
                break;
        }
    }

}
