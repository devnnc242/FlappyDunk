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
    //[SerializeField] private Text highScoreText;

    [Header("Combo UI")]
    [SerializeField] private Text comboText;

    [SerializeField] private float comboHideDelay = 1f;

    private Coroutine _hideComboCoroutine;

    protected override void Awake()
    {
        //MakeSingleton(false);
    }

    private void Start()
    {
        UpdateScore(ScoreManager.Ins.Score);
        UpdateHighScore(ScoreManager.Ins.HighScore);

        comboText.gameObject.SetActive(false);

        ScoreManager.Ins.OnScoreChanged += UpdateScore;
        ScoreManager.Ins.OnHighScoreChanged += UpdateHighScore;
        ScoreManager.Ins.OnComboChanged += UpdateCombo;
    }

    private void OnDestroy()
    {
        if (ScoreManager.Ins == null) return;

        ScoreManager.Ins.OnScoreChanged -= UpdateScore;
        ScoreManager.Ins.OnHighScoreChanged -= UpdateHighScore;
        ScoreManager.Ins.OnComboChanged -= UpdateCombo;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    private void UpdateHighScore(int highScore)
    {
        //highScoreText.text = $"Best: {highScore}";
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