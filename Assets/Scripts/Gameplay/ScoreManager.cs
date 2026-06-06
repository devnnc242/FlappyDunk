using System;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private int _score;
    private int _highScore;

    private int _cleanCombo = 1;

    public int Score => _score;
    public int HighScore => _highScore;

    public event Action<int> OnScoreChanged;
    public event Action<int> OnHighScoreChanged;

    public event Action<int> OnComboChanged;

    protected override void Awake()
    {
        MakeSingleton(false);

        _highScore = PlayerPrefs.GetInt(Constant.HIGH_SCORE, 0);
    }

    public void ProcessScore(bool touchedRim)
    {
        int scoreToAdd;

        if (touchedRim)
        {
            _cleanCombo = 1;

            scoreToAdd = 1;
        }
        else
        {
            _cleanCombo++;

            scoreToAdd = _cleanCombo;
        }

        _score += scoreToAdd;

        OnScoreChanged?.Invoke(_score);

        OnComboChanged?.Invoke(_cleanCombo);

        CheckHighScore();

        //Debug.Log($"Score: {_score} (+{scoreToAdd}, x{_cleanCombo})");
    }

    private void CheckHighScore()
    {
        if (_score < _highScore) return;

        _highScore = _score;

        PlayerPrefs.SetInt(Constant.HIGH_SCORE, _highScore);
        PlayerPrefs.Save();

        OnHighScoreChanged?.Invoke(_highScore);
    }

    public void ResetScore()
    {
        _score = 0;
        _cleanCombo = 1;

        OnScoreChanged?.Invoke(_score);
    }
}
