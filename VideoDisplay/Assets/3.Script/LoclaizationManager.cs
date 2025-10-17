using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public LocalizationData localizationData;
    public List<Image> langImages= new List<Image>();
    public List<Button> langBtns= new List<Button>();
    // index 5 6 7 (cashingOnBtn) 이미지를 2 3 4 버튼 select , pressed 교체해줘야함
    private void Start()
    {
        language = Language.Korean;
        localizationData.languageSprite = langImages;
        localizationData.Localiztion(language);
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
        ChangeOnBtnSprite();
    }
    private void ChangeOnBtnSprite()
    {
        for (int i = 0; i < langBtns.Count; i++)
        {
            var state = langBtns[i].spriteState;
            //state.pressedSprite = langImages[i + 5].sprite;
            state.selectedSprite = langImages[i + 5].sprite;
            Debug.Log(langImages[i + 3].sprite.name);
            langBtns[i].spriteState = state;
        }
    }
}
