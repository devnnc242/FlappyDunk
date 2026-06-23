using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public event Action<GameState> OnStateChanged;
    public GameState currentState { get; private set; }

    public bool IsReady => currentState == GameState.Ready;
    public bool IsPlaying => currentState == GameState.Playing;
    public bool IsPaused => currentState == GameState.Paused;
    public bool IsGameOver => currentState == GameState.GameOver;

    protected override bool DontDestroy => base.DontDestroy;

    private void Start()
    {
        SetState(GameState.Ready);
    }

    #region State
    public void StartGame()
    {
        if (!IsReady) return;

        SetState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (!IsPlaying) return;

        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (!IsPaused) return;

        SetState(GameState.Playing);
    }

    public void GameOver(string reason)
    {
        if (IsGameOver) return;

        Debug.Log("Game Over: " + reason);

        SetState(GameState.GameOver);
    }

    private void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Ready:
            case GameState.Playing:
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }

        OnStateChanged?.Invoke(newState);
    }
    #endregion

    #region  Scene
    public void RestartScene()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int buildIndex)
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(buildIndex);
    }
    #endregion

    #region Application
    private void OnApplicationPause(bool pause)
    {
        if (pause && IsPlaying)
        {
            PauseGame();
        }
    }
    #endregion
}
