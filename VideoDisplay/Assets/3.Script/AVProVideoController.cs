using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AVProVideoController : MonoBehaviour
{
    [SerializeField] private ButtonActiveController buttonController;
    public MediaPlayer mediaPlayer;  // AVPro MediaPlayer
    [SerializeField] private Slider playVar;

    private string video1 = "video1.mp4";
    private string video2 = "video2.mp4";
    private string video3 = "video3.mp4";
    
    private bool isGuidePlaying = false;
    private string currentFile;

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
        if (mediaPlayer == null || !mediaPlayer.Control.IsPlaying())
            return;
        float currentTime = (float)mediaPlayer.Control.GetCurrentTime();  // ms 단위
        float totalTime = (float)mediaPlayer.Info.GetDuration();          // ms 단위

        if (totalTime > 0f)
        {
            // 3️⃣ 비율 계산
            float normalized = currentTime / totalTime;

            // 4️⃣ 슬라이더에 적용
            playVar.value = normalized;  // 0.0 ~ 1.0
        }
        else
        {
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
        //isGuidePlaying = true;
        mediaPlayer.Control.Stop();

        string path = Path.Combine(Application.streamingAssetsPath, guideFileName);
        mediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, path, autoPlay: true);
        mediaPlayer.Control.SetLooping(false);
    }

    void OnMediaEvent(MediaPlayer mp, MediaPlayerEvent.EventType evtType, ErrorCode error)
    {
        if (evtType == MediaPlayerEvent.EventType.FinishedPlaying)
        {
            //PlayDefaultLoop();
            buttonController.ClearButtonSelection();
            SetActiveBG(true);
            currentFile = "";
        }
    }
   
    private void SetActiveBG(bool isActive)
    {
        backGround.SetActive(isActive);
    }
  
}
