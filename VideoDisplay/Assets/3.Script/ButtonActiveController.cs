using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonActiveController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AVProVideoController videoController;

    public List<Button> buttons = new List<Button>();
    public Button button1_BG;           // 체험 방법
    public Button button2_BG;           // 원리
    public Button button3_BG;           // 더알아보기
    private Button currentBtn;
    // Start is called before the first frame update
    void Start()
    {
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

    
}
