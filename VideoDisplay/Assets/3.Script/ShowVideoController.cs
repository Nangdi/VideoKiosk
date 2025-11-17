using RenderHeads.Media.AVProVideo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class ShowVideoController : MonoBehaviour
{
    public MediaPlayer showMediaPlayer;  // show MediaPlayer
    public MediaPlayer mediaPlayer;  // AVPro MediaPlayer
    public CanvasGroup showCanvasGroup;
    private string showFile= "show.mp4";
    public int intervalMinutes = 10; // 기본은 60분(정각). Inspector에서 수정 가능
    float time = 0;
    void Start()
    {
        GetShowFile();
        StopShow();
        showMediaPlayer.Events.AddListener(OnMediaEvent);
        intervalMinutes = JsonManager.instance.timeSetting.showIntervalMinutes;
        if (JsonManager.instance.timeSetting.useShowVideo)
        {
            StartCoroutine(WaitUntilNextHour());

        }
    }
    private void Update()
    {
        //time += Time.deltaTime;
        //if (time > 10)
        //{
        //    time = 0;
        //    PlayVideo();
        //}
        if (Input.anyKeyDown)
        {
            StopShow();
        }
    }
    private IEnumerator WaitUntilNextHour()
    {
        while (true)
        {
            DateTime now = DateTime.Now;

            // 현재 시간에서 intervalMinutes만큼 증가한 "다음 실행 시각" 계산
            DateTime next = now.AddMinutes(intervalMinutes);

            // next 를 정밀하게 "interval 단위 정렬" 하려면 아래처럼 계산
            int nextMinuteBlock = ((now.Minute / intervalMinutes) + 1) * intervalMinutes;

            // 만약 60 이상이면 다음 시간으로 넘긴다
            if (nextMinuteBlock >= 60)
            {
                next = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0)
                        .AddHours(1)
                        .AddMinutes(nextMinuteBlock - 60);
            }
            else
            {
                next = new DateTime(now.Year, now.Month, now.Day, now.Hour, nextMinuteBlock, 0);
            }

            double secondsToNext = (next - now).TotalSeconds;

            Debug.Log($"[HourlyVideoPlayer] 다음 실행까지 {secondsToNext}초 남음 (interval={intervalMinutes}분)");

            yield return new WaitForSeconds((float)secondsToNext);

            // 조건 실행
            if (!mediaPlayer.Control.IsPlaying())
            {
                PlayVideo();
            }
            else
            {
                Debug.Log($"패널 사용중 → 비디오 재생 X");
            }

            // 계속 반복
        }
        // 영상 재생 후 다시 다음 정각까지 대기
    
    }

    private void PlayVideo()
    {
        Debug.Log($"[HourlyVideoPlayer] {DateTime.Now:HH:mm:ss} - 영상 재생 시작");
        showCanvasGroup.DOFade(1, 0);
        showMediaPlayer.Control.Play();
    }
    private void GetShowFile()
    {
        Debug.Log($"이름: {showFile}");
        string path = Path.Combine(Application.streamingAssetsPath, showFile);
        Debug.Log($"경로: {path}");

        showMediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, path, autoPlay: false);
        showMediaPlayer.Control.SetLooping(false);
    }
    void OnMediaEvent(MediaPlayer mp, MediaPlayerEvent.EventType evtType, ErrorCode error)
    {
        if (evtType == MediaPlayerEvent.EventType.FinishedPlaying)
        {
            //PlayDefaultLoop();
            Debug.Log("영상 재생 완료");

            StopShow();

        }
    }
    private void StopShow()
    {
        showMediaPlayer.Control.Stop();
        showMediaPlayer.Control.Rewind();  // 💡 이게 ‘처음으로 되돌리기’ 기능입니다
        showCanvasGroup.DOFade(0, 0);
    }
}
