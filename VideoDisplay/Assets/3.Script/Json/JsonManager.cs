using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class GameSettingData
{
    public float btnSoundVolume = 0.5f;
    public float returnTime = 30f;
   
}
public class TextJson 
{
    public string koTitle = "각운동량\n실험";
    public string koTitle2 = "각운동량실험";
    public string enTitle = "Angular\nMomentum\nExperiment\n";
    public string enTitle2 = "Angular Momentum Experiment";
    public bool isAutoSize_Ko = true;
    public float fontSize_Ko = 150;
    public bool isAutoSize_En = false;
    public float fontSize_En = 104;
    public int SceneIndex = 0;
}

public class PortJson
{
    public string com = "COM4";
    public int baudLate = 19200;
}

public class JsonManager : MonoBehaviour
{

    public static JsonManager instance;
    public GameSettingData gameSettingData;
    public TextJson textJson;
    public PortJson portJson;
    private string gameDataPath;
    private string portPath;
    private string textPath;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        portPath = Path.Combine(Application.streamingAssetsPath, "port.json");
        textPath = Path.Combine(Application.streamingAssetsPath, "titleText.json");
        gameDataPath = Path.Combine(Application.persistentDataPath, "gameSettingData.json");

        gameSettingData = LoadData(gameDataPath, gameSettingData);
        textJson = LoadData(textPath, textJson);
        portJson= LoadData(portPath, portJson);
    }
    void Start()
    {
        // 화면 방향 세로로 강제
        Screen.orientation = ScreenOrientation.Portrait;

        // 전체화면 모드에서 세로 비율 강제 (Standalone용)
        Screen.SetResolution(1080, 1920, true);
    }
    //저장할 json 객체 , 경로설정
    public static void SaveData<T>(T jsonObject, string path) where T : new()
    {
        if (jsonObject == null)
            jsonObject = new T();  // 기본 생성자로 객체 초기화
        string json = JsonUtility.ToJson(jsonObject, true);
        File.WriteAllText(path, json);
        Debug.Log($"저장됨: {path}");
    }

    public static T LoadData<T>(string path, T data) where T : new()
    {
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning("JSON 파일이 존재하지 않습니다.");
                SaveData(data, path);
            }
            Debug.Log("JSON로드");
            string json = File.ReadAllText(path);
            T jsonData = JsonUtility.FromJson<T>(json);
            return jsonData;
        }

        //예시 실행코드
        //JsonManager.LoadData(파일경로 , 데이터클래스);

    }
}
