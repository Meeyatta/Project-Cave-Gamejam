using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("End Game UI")]
    public GameObject endGameUI;
    public TextMeshProUGUI endGameText;
    public TextMeshProUGUI statsText;
    public Button restartButton;
    public Button quitButton;

    [Header("Game Stats")]
    public float gameStartTime;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameStartTime = Time.time;

        if (endGameUI == null)
        {
            CreateEndGameUI();
        }

        if (endGameUI != null)
            endGameUI.SetActive(false);
    }

    public void TriggerGameEnd()
    {
        Debug.Log("Player escaped the cave!");

        // Calculate completion time
        float gameTime = Time.time - gameStartTime;
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        // Show end screen
        ShowEndGameUI(minutes, seconds);

        // Pause game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ShowEndGameUI(int minutes, int seconds)
    {
        if (endGameUI == null) return;

        endGameUI.SetActive(true);

        if (endGameText != null)
        {
            endGameText.text = "Congratulations!\nYou escaped the cave!";
        }

        if (statsText != null)
        {
            statsText.text = $"Completion Time: {minutes:00}:{seconds:00}";
        }
    }

    void CreateEndGameUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // Create end game panel
        endGameUI = new GameObject("EndGamePanel");
        endGameUI.transform.SetParent(canvas.transform, false);

        Image bgImage = endGameUI.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.8f);

        RectTransform panelRect = endGameUI.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        // Title text
        GameObject titleGO = new GameObject("EndGameText");
        titleGO.transform.SetParent(endGameUI.transform, false);

        endGameText = titleGO.AddComponent<TextMeshProUGUI>();
        endGameText.text = "Congratulations!\nYou escaped the cave!";
        endGameText.fontSize = 48;
        endGameText.color = Color.white;
        endGameText.alignment = TextAlignmentOptions.Center;
        endGameText.fontStyle = FontStyles.Bold;

        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.7f);
        titleRect.anchorMax = new Vector2(0.5f, 0.7f);
        titleRect.pivot = new Vector2(0.5f, 0.5f);
        titleRect.sizeDelta = new Vector2(800, 200);

        // Stats text
        GameObject statsGO = new GameObject("StatsText");
        statsGO.transform.SetParent(endGameUI.transform, false);

        statsText = statsGO.AddComponent<TextMeshProUGUI>();
        statsText.text = "";
        statsText.fontSize = 24;
        statsText.color = Color.yellow;
        statsText.alignment = TextAlignmentOptions.Center;

        RectTransform statsRect = statsGO.GetComponent<RectTransform>();
        statsRect.anchorMin = new Vector2(0.5f, 0.5f);
        statsRect.anchorMax = new Vector2(0.5f, 0.5f);
        statsRect.pivot = new Vector2(0.5f, 0.5f);
        statsRect.sizeDelta = new Vector2(600, 100);

        // Restart button
        GameObject restartGO = new GameObject("RestartButton");
        restartGO.transform.SetParent(endGameUI.transform, false);

        restartButton = restartGO.AddComponent<Button>();
        Image restartImg = restartGO.AddComponent<Image>();
        restartImg.color = new Color(0.2f, 0.7f, 0.2f, 1f);

        RectTransform restartRect = restartGO.GetComponent<RectTransform>();
        restartRect.anchorMin = new Vector2(0.35f, 0.25f);
        restartRect.anchorMax = new Vector2(0.35f, 0.25f);
        restartRect.pivot = new Vector2(0.5f, 0.5f);
        restartRect.sizeDelta = new Vector2(200, 60);

        GameObject restartTextGO = new GameObject("RestartText");
        restartTextGO.transform.SetParent(restartGO.transform, false);

        TextMeshProUGUI restartText = restartTextGO.AddComponent<TextMeshProUGUI>();
        restartText.text = "Play Again";
        restartText.fontSize = 18;
        restartText.color = Color.white;
        restartText.alignment = TextAlignmentOptions.Center;

        RectTransform restartTextRect = restartTextGO.GetComponent<RectTransform>();
        restartTextRect.anchorMin = Vector2.zero;
        restartTextRect.anchorMax = Vector2.one;
        restartTextRect.sizeDelta = Vector2.zero;

        restartButton.onClick.AddListener(RestartGame);

        // Quit button
        GameObject quitGO = new GameObject("QuitButton");
        quitGO.transform.SetParent(endGameUI.transform, false);

        quitButton = quitGO.AddComponent<Button>();
        Image quitImg = quitGO.AddComponent<Image>();
        quitImg.color = new Color(0.7f, 0.2f, 0.2f, 1f);

        RectTransform quitRect = quitGO.GetComponent<RectTransform>();
        quitRect.anchorMin = new Vector2(0.65f, 0.25f);
        quitRect.anchorMax = new Vector2(0.65f, 0.25f);
        quitRect.pivot = new Vector2(0.5f, 0.5f);
        quitRect.sizeDelta = new Vector2(200, 60);

        GameObject quitTextGO = new GameObject("QuitText");
        quitTextGO.transform.SetParent(quitGO.transform, false);

        TextMeshProUGUI quitText = quitTextGO.AddComponent<TextMeshProUGUI>();
        quitText.text = "Quit Game";
        quitText.fontSize = 18;
        quitText.color = Color.white;
        quitText.alignment = TextAlignmentOptions.Center;

        RectTransform quitTextRect = quitTextGO.GetComponent<RectTransform>();
        quitTextRect.anchorMin = Vector2.zero;
        quitTextRect.anchorMax = Vector2.one;
        quitTextRect.sizeDelta = Vector2.zero;

        quitButton.onClick.AddListener(QuitGame);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
