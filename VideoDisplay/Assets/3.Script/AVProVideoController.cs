using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AVProVideoController : MonoBehaviour
{
    [SerializeField] private ButtonActiveController buttonController;
    [SerializeField] private HomeReturn homeReturn;
    public MediaPlayer mediaPlayer;  // AVPro MediaPlayer
    [SerializeField] private Slider playVar;
    [SerializeField] private TMP_Text timeText;
    public Image PlayBtn;
    public Sprite[] btnimage; 
    private string video1 = "video1.mp4";
    private string video2 = "video2.mp4";
    private string video3 = "video3.mp4";
    
    private bool isGuidePlaying = false;
    private string currentFile;
    private bool isHandling;
    private bool isPlay = false;

    [SerializeField] private GameObject backGround;

    void Start()
    {
        // 버튼 이벤트 연결
        buttonController.buttons[0].onClick.AddListener(() => PlayGuide(video1));
        buttonController.buttons[1].onClick.AddListener(() => PlayGuide(video2));
        buttonController.buttons[2].onClick.AddListener(() => PlayGuide(video3));
        buttonController.button1_BG.onClick.AddListener(() => PlayGuide(video1));
        buttonController.button2_BG.onClick.AddListener(() => PlayGuide(video2));
        buttonController.button3_BG.onClick.AddListener(() => PlayGuide(video3));

        // 이벤트 리스너 등록
        mediaPlayer.Events.AddListener(OnMediaEvent);

        // 기본 영상 재생
        //PlayDefaultLoop();
    }
    private void Update()
    {
        if (mediaPlayer == null || !mediaPlayer.Control.IsPlaying() || isHandling)
            return;
        float currentTime = (float)mediaPlayer.Control.GetCurrentTime();  // ms 단위
        float totalTime = (float)mediaPlayer.Info.GetDuration();          // ms 단위

        if (totalTime > 0f)
        {
            // 3️⃣ 비율 계산
            float normalized = currentTime / totalTime;

            // 4️⃣ 슬라이더에 적용
            playVar.value = normalized;  // 0.0 ~ 1.0
            float remainingTime = totalTime - currentTime;
            timeText.text = FormatTime(remainingTime);
        }
        else
        {
            timeText.text = "00:00:00";
            playVar.value = 0f;
        }
    }
    void PlayDefaultLoop()
    {
        
        isGuidePlaying = false;
        //mediaPlayer.Control.Stop();
        currentFile = video1;
        string path = Path.Combine(Application.streamingAssetsPath, video1);
        mediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, path, autoPlay: true);
        Debug.Log("경로 확인: " + path);
        Debug.Log(File.Exists(path));  // true 나오면 OK
        mediaPlayer.Control.SetLooping(true);
    }

    public void PlayGuide(string guideFileName)
    {
        if(currentFile == guideFileName)
        {
            return;
        }
        currentFile = guideFileName;
        SetActiveBG(false);
        isPlay = true;
        //isGuidePlaying = true;
        mediaPlayer.Control.Stop();

        string path = Path.Combine(Application.streamingAssetsPath, guideFileName);
        mediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, path, autoPlay: true);
        mediaPlayer.Control.SetLooping(false);
    }

    void OnMediaEvent(MediaPlayer mp, MediaPlayerEvent.EventType evtType, ErrorCode error)
    {
        if (evtType == MediaPlayerEvent.EventType.FinishedPlaying )
        {
            EndVideo();
        }
    }
    private void EndVideo()
    {
        //PlayDefaultLoop();
        buttonController.ClearButtonSelection();
        SetActiveBG(true);
        isPlay = false;
        currentFile = "";
    }
    public void homeBtn()
    {
        mediaPlayer.Control.Stop();
        homeReturn.startReturnCount = false;
        EndVideo();
    }
   
    private void SetActiveBG(bool isActive)
    {
        backGround.SetActive(isActive);
    }
    public void onPointerDown()
    {
        isHandling = true;
        homeReturn.startReturnCount = false;
        mediaPlayer.Control.Pause();


    }
    public void onPointerUp()
    {
        isHandling = false;

        isPlay = true;
        homeReturn.startReturnCount = false;
        PlayBtn.sprite = btnimage[1];
        mediaPlayer.Control.Play();
    }
    private void OnPlayClicked()
    {
        mediaPlayer.Control.Play();
    }
    private void OnPauseClicked()
    {
        mediaPlayer.Control.Pause();
    }
    public void OnClickPlay()
    {
        if (isPlay)
        {
            isPlay = false;
            mediaPlayer.Control.Pause();
            PlayBtn.sprite = btnimage[0];
            homeReturn.lapseTime = 0;
            homeReturn.startReturnCount = true;
            //sprite교체
        }
        else
        {
            isPlay = true;
            mediaPlayer.Control.Play();
            homeReturn.startReturnCount = false;
            PlayBtn.sprite = btnimage[1];
        }
    }

    public void SetCurrentTime(float value)
    {
        //if (!isHandling) return;
        float totalTime = (float)mediaPlayer.Info.GetDuration();
        mediaPlayer.Control.Seek(totalTime * value);
        //if( Mathf.Approximately(totalTime * value, totalTime))
        //{
        //    mediaPlayer.Control.Seek(totalTime-0.05f);
        //}
        if (totalTime - totalTime * value < 0.1f && !isHandling)
        {
            homeBtn();
            Debug.Log($"끝까지왔다면 1로 보정") ;
            // 끝 처리
        }
        //Debug.Log(totalTime - (totalTime* value));
        float remainder = totalTime - (totalTime * value);
        timeText.text = FormatTime(remainder);
    }

    // 🕒 시간 포맷 함수
    private string FormatTime(float ms)
    {
        ms = ms * 1000;
        int minutes = Mathf.FloorToInt(ms / 60000f);
        int seconds = Mathf.FloorToInt((ms % 60000f) / 1000f);
        int milliseconds = Mathf.FloorToInt(ms % 1000f);

        return $"{minutes:00}:{seconds:00}";
    }
}
