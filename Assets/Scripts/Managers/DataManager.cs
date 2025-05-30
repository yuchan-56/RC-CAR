using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict(); //MakeDict 함수 구현 강제 , Data 객체들은 Dictionary에 모음.
}



public class DataManager
 {
    // public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>(); // 스탯관련 데이터들을 담은 Dictionary (key, value= 스탯 객체)

        public Dictionary<int, int> currentLevel = new Dictionary<int, int>();
        public string Stage;
        public Define.WholeGameData gameData;
        public Define.Items Items;
        public Define.VolumeData volumeData = new Define.VolumeData();

    public void Init()
    {
#if UNITY_EDITOR
        PlayerPrefs.SetInt("FirstOpen", 0);
        PlayerPrefs.SetInt("StageData", 1);
#endif
        // 데이터 PlayerPref로 저장
        if (!PlayerPrefs.HasKey("FirstOpen") || PlayerPrefs.GetInt("FirstOpen") == 0)
        {
            Debug.Log("ManagerInit?");
            PlayerPrefs.SetInt("FirstOpen", 1);
            PlayerPrefs.SetInt("StageData", 1);
            PlayerPrefs.Save();
        }

    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}"); // text 파일이 textAsset에 담긴다.
                                                                                 // TextAsset 타입은 텍스트파일 에셋이라고 생각하면 됨!
        return JsonUtility.FromJson<Loader>(textAsset.text); //JSON 데이터를 불러와서 리턴
    }

}

#region Stage
[System.Serializable]
public class StageData
{
    public int currentStage = 1;
    public void SaveCurrentStage(int stageNumber)
    {
        StageData data = new StageData();
        data.currentStage = stageNumber;

        JsonManager jsonManager = new JsonManager();
        jsonManager.Save(data, "StageData.json");
    }
    public int LoadCurrentStage()
    {
        JsonManager jsonManager = new JsonManager();
        StageData data = jsonManager.Load<StageData>("StageData.json");

        if (data == null)
        {
            
            return 1; // 기본 스테이지
        }

        return data.currentStage;
    }
}


#endregion
#region Stat

[Serializable]
public class Stat // // MonoBehavior 를 상속하지 않았기 때문에 직렬화해서 insperctor창에서 편집
{
    public int LEVEL; // ID
    public int ATK;
    public int DEF;
    public int GOLD;
    public int CRIT1;
    public int CRIT2;
}

[Serializable]
public class StatData : ILoader<int, Stat>
{
    public List<Stat> stats = new List<Stat>();  // json 파일에서 여기로 담김

    public Dictionary<int, Stat> MakeDict() // 오버라이딩
    {
        Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
        foreach (Stat stat in stats) // 리스트에서 Dictionary로 옮기는 작업
            dict.Add(stat.LEVEL, stat); // level을 ID(Key)로 
        return dict;
    }
}

#endregion