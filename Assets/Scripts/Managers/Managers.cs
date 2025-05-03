using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }


    UI_Manager _ui = new UI_Manager();
    ResourceManager _resource = new ResourceManager();
    GameManager _game = new GameManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    SceneManagerEx _scene = new SceneManagerEx();
    DataManager _data = new DataManager(); //DataManager�� ���ļ� �߰�
    JsonManager _json = new JsonManager();
    SpeechManager _speech = new SpeechManager();
    public static GameManager Game { get { return Instance._game; } }
    public static UI_Manager UI { get { return Instance._ui; } }
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static DataManager Data { get { return Instance._data; } } 
    public static JsonManager Json {  get { return Instance._json; } }
    public static SpeechManager Speech {  get { return Instance._speech; } }


    public static SceneManagerEx Scene { get { return Instance._scene; } }


    void Start()
    {
        Init();

    }
    // Update is called once per frame
    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {

        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Manager");
            if (go == null)
            {

                go = new GameObject { name = "@Manager" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            s_instance._pool.Init();
            s_instance._data.Init();

            if (!PlayerPrefs.HasKey("FirstOpen"))
            {
                PlayerPrefs.SetInt("FirstOpen", 1);
                PlayerPrefs.SetInt("StageData", 1);
                PlayerPrefs.Save();
            }
            
        }
    }

    public static void Clear()
    {
        Input.Clear();
        UI.Clear();
        Pool.Clear();
    }

}
