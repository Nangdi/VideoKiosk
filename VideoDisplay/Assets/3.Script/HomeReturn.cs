using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeReturn : MonoBehaviour
{
    [SerializeField] AVProVideoController videoController;
    public float lapseTime;
    public bool startReturnCount =false;
    float returnTime = 30f;
    // Update is called once per frame
    private void Start()
    {
        returnTime = JsonManager.instance.gameSettingData.returnTime;
    }
    void Update()
    {
        if (!startReturnCount) return;
        lapseTime += Time.deltaTime;
        if(lapseTime >= returnTime)
        {
            lapseTime = 0;
            //홈버튼 및 bool 초기화
            videoController.homeBtn();
        }
    }
    public void ResetTimer()
    {
        lapseTime = 0;
        startReturnCount = false;
    }
}
