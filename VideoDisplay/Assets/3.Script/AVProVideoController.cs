using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AVProVideoController : MonoBehaviour
{
    public MediaPlayer mediaPlayer;  // AVPro MediaPlayer
    public Button button1;           // 체험 방법
    public Button button2;           // 원리
    public Button button3;           // 더알아보기

    public Button button1_BG;           // 체험 방법
    public Button button2_BG;           // 원리
    public Button button3_BG;           // 더알아보기


    private string video1 = "video1.mp4";
    private string video2 = "video2.mp4";
    private string video3 = "video3.mp4";
    
    private bool isGuidePlaying = false;
    private string currentFile;

    [SerializeField] private GameObject backGround;

    void Start()
    {
        // 버튼 이벤트 연결
        button1.onClick.AddListener(() => PlayGuide(video1));
        button2.onClick.AddListener(() => PlayGuide(video2));
        button3.onClick.AddListener(() => PlayGuide(video3));
        button1_BG.onClick.AddListener(() => PlayGuide(video1));
        button2_BG.onClick.AddListener(() => PlayGuide(video2));
        button3_BG.onClick.AddListener(() => PlayGuide(video3));

        // 이벤트 리스너 등록
        mediaPlayer.Events.AddListener(OnMediaEvent);

        // 기본 영상 재생
        //PlayDefaultLoop();
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

    void PlayGuide(string guideFileName)
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
            ClearButtonSelection();
            SetActiveBG(true);
            currentFile = "";
        }
    }
    void ClearButtonSelection()
    {
        EventSystem.current.SetSelectedGameObject(button1.gameObject);
    }
    private void SetActiveBG(bool isActive)
    {
        backGround.SetActive(isActive);
    }
}
