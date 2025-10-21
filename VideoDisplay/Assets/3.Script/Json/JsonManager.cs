using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class GameSettingData
{
    public float btnSoundVolume = 0.5f;
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
    public PortJson portJson;
    private string gameDataPath;
    private string portPath;
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
        gameDataPath = Path.Combine(Application.persistentDataPath, "gameSettingData.json");

        gameSettingData = LoadData(gameDataPath, gameSettingData);
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
