using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenuManager : MonoBehaviour {
    public static StartMenuManager Instance { get; private set; }

    public string playerName;
    public string bestPlayerName;
    public int bestScore;

    [SerializeField] Text bestScoreText;
    [SerializeField] TMP_InputField inputName;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        Load();
        bestScoreText.text = $"Best Score: {bestPlayerName} : {bestScore}";
    }

    public void StartNew() {
        SceneManager.LoadScene(1);
    }

    public void Quit() {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif

    }

    [System.Serializable]
    class SaveData {
        public string PlayerName;
        public int score;
    }
    public void Save(int score) {
        SaveData data = new SaveData();
        data.PlayerName = playerName;
        data.score = score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load() {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayerName = data.PlayerName;
            bestScore = data.score;
        }
    }

    public void SetPlayerName() {
        playerName = inputName.text;
    }
}
