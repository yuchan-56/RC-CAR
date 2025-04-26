using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict(); //MakeDict �Լ� ���� ���� , Data ��ü���� Dictionary�� ����.
}



public class DataManager
 {
    // public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>(); // ���Ȱ��� �����͵��� ���� Dictionary (key, value= ���� ��ü)

        public Dictionary<int, int> currentLevel = new Dictionary<int, int>();
        public string Stage;
        public Define.WholeGameData gameData;
        public Define.Items Items;
        public Define.VolumeData volumeData = new Define.VolumeData();

    public void Init()
    {
        // ������ PlayerPref�� ����
        if (PlayerPrefs.GetInt("FirstPlay", 0) == 0)
        {
            PlayerPrefs.SetInt("StageData", 1);
            PlayerPrefs.SetInt("FirstPlay", 1); // �������ʹ� 1�� ����
            PlayerPrefs.Save();
        }
        else
        {
            // �̹� ������ �� ����
            Debug.Log("�̹� ������ �� ����!");
        }

    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}"); // text ������ textAsset�� ����.
                                                                                 // TextAsset Ÿ���� �ؽ�Ʈ���� �����̶�� �����ϸ� ��!
        return JsonUtility.FromJson<Loader>(textAsset.text); //JSON �����͸� �ҷ��ͼ� ����
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
            Debug.LogWarning("StageData �ε� ����, �⺻�� ��ȯ");
            return 1; // �⺻ ��������
        }

        return data.currentStage;
    }
}


#endregion
#region Stat

[Serializable]
public class Stat // // MonoBehavior �� ������� �ʾұ� ������ ����ȭ�ؼ� insperctorâ���� ����
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
    public List<Stat> stats = new List<Stat>();  // json ���Ͽ��� ����� ���

    public Dictionary<int, Stat> MakeDict() // �������̵�
    {
        Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
        foreach (Stat stat in stats) // ����Ʈ���� Dictionary�� �ű�� �۾�
            dict.Add(stat.LEVEL, stat); // level�� ID(Key)�� 
        return dict;
    }
}

#endregion