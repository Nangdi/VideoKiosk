using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;   // 싱글톤으로 전역 접근

    [Header("Audio Source for SFX")]
    [SerializeField] private AudioSource sfxSource;

    [Header("UI Button Sound Clips")]
    public AudioClip clickSound;      // 버튼 클릭
    public AudioClip hoverSound;      // 버튼 위에 올렸을 때 (선택사항)
    public AudioClip confirmSound;    // 확인, 선택 등

    [Header("SFX Volume (0~1)")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    public void SetVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    private void Awake()
    {
        // 싱글톤 세팅
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SetVolume(0.5f);
    }

    /// <summary>
    /// 버튼 클릭 사운드
    /// </summary>
    /// //버튼에 직접참조
    public void PlayClick()
    {
        Debug.Log("버튼사운드재생");
        PlaySFX(clickSound);
    }

    /// <summary>
    /// 지정한 AudioClip 재생
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.Stop();
        sfxSource.PlayOneShot(clip);
    }
}
