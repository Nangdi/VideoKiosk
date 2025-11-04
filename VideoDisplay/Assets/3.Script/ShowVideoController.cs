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
    float time = 0;
    void Start()
    {
        GetShowFile();
        StopShow();
        showMediaPlayer.Events.AddListener(OnMediaEvent);
        StartCoroutine(WaitUntilNextHour());
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
            // 다음 정각 시각 계산
            DateTime nextHour = now.AddHours(1).Date.AddHours(now.Hour + 1);
            double secondsToNextHour = (nextHour - now).TotalSeconds;

            Debug.Log($"[HourlyVideoPlayer] 다음 정각까지 {secondsToNextHour}초 대기");

            yield return new WaitForSeconds((float)secondsToNextHour);
            if (!mediaPlayer.Control.IsPlaying())
            {
                PlayVideo();

            }
            else
            {
                Debug.Log($"패널사용중으로 비디오재생 x");
            }

            // 영상 재생 후 다시 다음 정각까지 대기
        }
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
