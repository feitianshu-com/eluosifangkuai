using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 添加这一行
using UnityEngine.UI; // 添加这一行引用UI命名空间

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("游戏状态")]
    public bool isGameStarted = false;
    public bool isGameOver = false;
    public bool isGameStop = false;

    [Header("UI元素")]
    public GameObject countdownPanel;
    public TMP_Text countdownText;
    public GameObject gameOverPanel;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public Button stopStartButton;


    [Header("游戏配置")]
    public float startDelay = 3f; // 开始前的倒计时秒数
    private int score = 0;
    private float countdownTime;

    [ContextMenu("测试显示倒计时")]
    public void TestShowCountdown()
    {
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(true);
        }

        if (countdownText != null)
        {
            // 设置文本内容
            countdownText.text = "Test";

            // 设置文本颜色为红色（可调整为其他颜色）
            countdownText.color = Color.red;

            // 确保文本可见（字体大小、对齐方式等）
            countdownText.fontSize = 100;
            countdownText.alignment = TextAlignmentOptions.Center;

            Debug.Log("已设置测试文本，检查是否显示");
        }
        else
        {
            Debug.LogError("countdownText未设置，请在Inspector中关联!");
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 初始化UI
        if (countdownPanel != null) countdownPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        stopStartButton.gameObject.SetActive(false);

        // 开始倒计时
        countdownTime = startDelay;
        UpdateCountdownText();

        // 禁用方块生成器
        TetrominoSpawner spawner = FindObjectOfType<TetrominoSpawner>();
        if (spawner != null) spawner.enabled = false;
    }

    private void Update()
    {
        if (!isGameStarted && countdownTime > 0)
        {
            // 倒计时
            countdownTime -= Time.deltaTime;
            UpdateCountdownText();

            if (countdownTime <= 0)
            {
                StartGame();
            }
        }
    }

    private void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            int seconds = Mathf.CeilToInt(countdownTime);
            Debug.Log($"{seconds} ------------");
            countdownText.text = seconds.ToString();

            // 倒计时结束时显示"开始"
            if (seconds <= 0)
            {
                countdownText.text = "Start!";
                stopStartButton.gameObject.SetActive(true); // 完全隐藏按钮及其子组件
            }
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        isGameOver = false;

        // 隐藏倒计时面板
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }

        // 启用方块生成器
        TetrominoSpawner spawner = FindObjectOfType<TetrominoSpawner>();
        if (spawner != null)
        {
            spawner.enabled = true;
            spawner.SpawnRandomTetromino();
        }
    }

    public void AddScore(int points)
    {
        if (!isGameOver)
        {
            score += points;
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score;
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        // 显示游戏结束面板
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null)
            {
                finalScoreText.text = "Final Score: " + score;
            }
        }

        // 禁用方块生成器
        TetrominoSpawner spawner = FindObjectOfType<TetrominoSpawner>();
        if (spawner != null)
        {
            spawner.enabled = false;
        }
    }

    public void RestartGame()
    {
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StopStartGame()
    {
        TMP_Text buttonText = stopStartButton.GetComponentInChildren<TMP_Text>();
        if (isGameStop)
        {
            isGameStop = false;
            buttonText.text = "Stop";
        }
        else
        {
            isGameStop = true;
            buttonText.text = "Start";
        }
    }
}
