using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum Language
{
    Korean,
    English
}
public class LoclaizationManager : MonoBehaviour
{
    public Language language = Language.Korean;
    public LocalizationData[] DataStore;
    private LocalizationData localizationData;
    [SerializeField]private ButtonActiveController buttonActiveController;
    public List<Image> langImages= new List<Image>();
    public List<Button> langBtns= new List<Button>();
    public List<GameObject> textObs = new List<GameObject>();

    // index 5 6 7 (cashingOnBtn) 이미지를 2 3 4 버튼 select , pressed 교체해줘야함
    private void Start()
    {
        language = Language.English;
        //0 블루 , 1 레드 , 2 블루그린 , 3그린
        localizationData = DataStore[JsonManager.instance.textJson.SceneIndex];
        //localizationData = DataStore[2];
        localizationData.languageSprite = langImages;
        localizationData.Localiztion(language);
        SwitchLang();
    }
    public void SwitchLang()
    {
        if (language == Language.Korean)
        {
            language = Language.English;
        }
        else
        {
            language = Language.Korean;
        }
        localizationData.Localiztion(language);
        //buttonActiveController.ToggleRect(language);
        LocalizionToText(language);
        ChangeOnBtnSprite();
    }
    private void ChangeOnBtnSprite()
    {
        for (int i = 0; i < langBtns.Count; i++)
        {
            var state = langBtns[i].spriteState;
            //state.pressedSprite = langImages[i + 5].sprite;
            state.selectedSprite = langImages[i + 5].sprite;
            langBtns[i].spriteState = state;
        }
    }
    private void LocalizionToText(Language language)
    {
        for (int i = 0; i < textObs.Count; i++)
        {
            textObs[i].SetActive(false);
        }
        switch (language)
        {
            case Language.Korean:
                textObs[0].SetActive(true);
                textObs[1].SetActive(true);
                //textObs[4].SetActive(true);
                break;
            case Language.English:
                textObs[2].SetActive(true);
                textObs[3].SetActive(true);
                break;
        }
    }

}
