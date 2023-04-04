using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.IO;
using TMPro;

public class MainManager : MonoBehaviour
{

    public static MainManager Instance; //added

    [Header("Game Data")]
    public int m_Points;
    public int highscore;  //added
    public string currentPlayer; //added
    public string bestPlayer; //added

    [Header("Game Assets")]
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text ScoreText;
    public TextMeshProUGUI highscoreText; //added
    public GameObject GameOverText;

    [Header("Game Flow")]
    public bool m_Started = false;
    public bool m_GameOver = false;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CheckForHighscore(int score) {
        if (score > highscore) {
            highscore = score;
            bestPlayer = currentPlayer;
            showHighscore();
            SaveHighscore();
        }
    }
    public void showHighscore() {

        highscoreText.text = $"Highscore: {highscore} ({bestPlayer})";
    }

    [System.Serializable]
    class SaveData
    {
        public int highscore;
        public string bestPlayer;
    }

    public void SaveHighscore()
    {
        SaveData data = new SaveData();
        data.highscore = highscore;
        data.bestPlayer = bestPlayer;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/mysavefile.json", json);
    }

    public void LoadHighscore()
    {
        string path = Application.persistentDataPath + "/mysavefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highscore = data.highscore;
            bestPlayer = data.bestPlayer;

            showHighscore();
        }
    }

    public bool sceneLoaded = false;
    private void loadScene()
    {
        GameOverText = GameObject.Find("MainCanvas").GetComponent<MainCanvas>().GameOverText;
        GameOverText.SetActive(false);
        highscoreText = GameObject.Find("HighscoreText").GetComponent<TextMeshProUGUI>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        Ball = GameObject.Find("Ball").GetComponent<Rigidbody>();
        BrickPrefab = GameObject.Find("MainCanvas").GetComponent<MainCanvas>().BrickPrefab;

        m_Points = 0;
        LoadHighscore();

#region------previousCode------ 
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
#endregion

        sceneLoaded = true; //added

    }

    // previous code
    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("main")) // added
        {
            if (!m_Started)
            {
                if (!sceneLoaded) loadScene();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Started = true;
                    float randomDirection = Random.Range(-1.0f, 1.0f);
                    Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                    forceDir.Normalize();

                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }
            }
            else if (m_GameOver)
            {
                CheckForHighscore(m_Points); //added
                sceneLoaded = false; // added

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_GameOver = false; // added
                    m_Started = false; // added
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                   
                }
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
