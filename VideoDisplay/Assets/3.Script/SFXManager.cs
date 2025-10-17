using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;   // �̱������� ���� ����

    [Header("Audio Source for SFX")]
    [SerializeField] private AudioSource sfxSource;

    [Header("UI Button Sound Clips")]
    public AudioClip clickSound;      // ��ư Ŭ��
    public AudioClip hoverSound;      // ��ư ���� �÷��� �� (���û���)
    public AudioClip confirmSound;    // Ȯ��, ���� ��

    [Header("SFX Volume (0~1)")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    public void SetVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    private void Awake()
    {
        // �̱��� ����
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SetVolume(0.5f);
    }

    /// <summary>
    /// ��ư Ŭ�� ����
    /// </summary>
    /// //��ư�� ��������
    public void PlayClick()
    {
        Debug.Log("��ư�������");
        PlaySFX(clickSound);
    }

    /// <summary>
    /// ������ AudioClip ���
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.Stop();
        sfxSource.PlayOneShot(clip);
    }
}
