using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Score UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    [Header("Combo UI")]
    [SerializeField] private Text comboText;

    [SerializeField] private float comboHideDelay = 1f;

    private Coroutine _hideComboCoroutine;

    private ScoreManager _scoreManager;
    private GameManager _gameManager;

    protected override void Awake()
    {
        base.Awake();

        _scoreManager = ScoreManager.Ins;
        _gameManager = GameManager.Ins;
    }

    private void OnEnable()
    {
        if (_scoreManager != null)
        {
            _scoreManager.OnScoreChanged += UpdateScore;
            _scoreManager.OnHighScoreChanged += UpdateHighScore;
            _scoreManager.OnComboChanged += UpdateCombo;
        }

        if (_gameManager != null)
        {
            _gameManager.OnStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDisable()
    {
        if (_scoreManager != null)
        {
            _scoreManager.OnScoreChanged -= UpdateScore;
            _scoreManager.OnHighScoreChanged -= UpdateHighScore;
            _scoreManager.OnComboChanged -= UpdateCombo;
        }

        if (_gameManager != null)
        {
            _gameManager.OnStateChanged -= HandleGameStateChanged;
        }
    }

    private void Start()
    {
        UpdateScore(_scoreManager.Score);
        UpdateHighScore(_scoreManager.HighScore);

        comboText.gameObject.SetActive(false);

        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void HandleGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Ready:
                pausePanel.SetActive(false);
                gameOverPanel.SetActive(false);
                break;

            case GameState.Playing:
                pausePanel.SetActive(false);
                gameOverPanel.SetActive(false);
                break;

            case GameState.Paused:
                pausePanel.SetActive(true);
                break;

            case GameState.GameOver:
                gameOverPanel.SetActive(true);
                break;
        }
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    private void UpdateHighScore(int highScore)
    {
        if (highScoreText == null) return;

        highScoreText.text = $"Best: {highScore}";
    }

    private void UpdateCombo(int combo)
    {
        if (combo <= 1)
        {
            comboText.gameObject.SetActive(false);
            return;
        }

        comboText.gameObject.SetActive(true);
        comboText.text = $"x{combo}";

        if (_hideComboCoroutine != null)
        {
            StopCoroutine(_hideComboCoroutine);
        }

        _hideComboCoroutine = StartCoroutine(HideComboRoutine());
    }

    private IEnumerator HideComboRoutine()
    {
        yield return new WaitForSeconds(comboHideDelay);

        comboText.gameObject.SetActive(false);
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }
}