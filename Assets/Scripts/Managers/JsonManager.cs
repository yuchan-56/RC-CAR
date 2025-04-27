using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonManager
{
    public void Save<T>(T data, string name = null)
    {
        //안드로이드에서의 저장 위치를 다르게 해주어야 한다
        //Application.dataPath를 이용하면 어디로 가는지는 구글링 해보길 바란다
        //안드로이드의 경우에는 데이터조작을 막기위해 2진데이터로 변환을 해야한다
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name + ".json";
        }
        string savePath = Application.dataPath;
        string appender = $"/userData/{name}";
#if UNITY_EDITOR_WIN

#endif
#if UNITY_ANDROID
        savePath = Application.persistentDataPath;
 
#endif
        StringBuilder builder = new StringBuilder(savePath);
        builder.Append(appender);
        string jsonText = JsonUtility.ToJson(data, true);
        //이러면은 일단 데이터가 텍스트로 변환이 된다
        //jsonUtility를 이용하여 data인 WholeGameData를 json형식의 text로 바꾸어준다
        //파일스트림을 이렇게 지정해주고 저장해주면된당 끗
        FileStream fileStream = new FileStream(builder.ToString(), FileMode.Create);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonText);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
    }

    public T Load<T>(string name = null) where T : new()
    {
        T data;
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name + ".json";
        }

        string loadPath = Application.dataPath;
        string directory = "/userData";
        string appender = $"/{name}";

#if UNITY_ANDROID
        loadPath = Application.persistentDataPath;
#endif

        StringBuilder builder = new StringBuilder(loadPath);
        builder.Append(directory);

        if (!Directory.Exists(builder.ToString()))
        {
            Directory.CreateDirectory(builder.ToString());
        }

        builder.Append(appender);

        if (File.Exists(builder.ToString()))
        {
            FileStream stream = new FileStream(builder.ToString(), FileMode.Open);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Close();
            string jsonData = Encoding.UTF8.GetString(bytes);

            data = JsonUtility.FromJson<T>(jsonData);
        }
        else
        {
            // Resources에서 기본 파일 찾기
            TextAsset textAsset = Resources.Load<TextAsset>($"Data/{typeof(T).Name}");

            if (textAsset != null)
            {
                data = JsonUtility.FromJson<T>(textAsset.text);
                Save(data); // 읽은 기본 데이터를 저장
            }
            else
            {
                
                data = new T(); // 새 인스턴스 생성
                Save(data); // 저장
            }
        }

        return data;
    }

    public void ResetStageData()
    {
        string path = Application.dataPath + "/userData/StageData.json";

        if (File.Exists(path))
        {
            File.Delete(path);
            
            // 기본값으로 새로 저장
            JsonManager manager = new JsonManager();
            var newData = new StageData(); // 기본값 객체
            manager.Save(newData);
        }
    }

}
