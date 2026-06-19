using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool IsGameOver { get; private set; }

    public GameState currentState { get; private set; }
    public bool IsPlaying => currentState == GameState.Playing;
    public bool IsReady => currentState == GameState.Ready;

    protected override void Awake()
    {
        //MakeSingleton(false);
    }

    private void Start()
    {
        SetState(GameState.Ready);
    }

    #region State
    public void StartGame()
    {
        if (currentState != GameState.Ready) return;

        SetState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (currentState != GameState.Playing) return;

        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (currentState != GameState.Paused)
            return;

        SetState(GameState.Playing);
    }

    public void GameOver(string reason)
    {
        if (IsGameOver) return;

        IsGameOver = true;

        SetState(GameState.GameOver);

        Debug.Log("Game Over: " + reason);
    }

    private void SetState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Ready:
                //Time.timeScale = 0f;

                UIManager.Ins.HidePausePanel();
                UIManager.Ins.HideGameOverPanel();

                break;

            case GameState.Playing:
                //Time.timeScale = 1f;

                UIManager.Ins.HidePausePanel();
                UIManager.Ins.HideGameOverPanel();

                break;

            case GameState.Paused:
                Time.timeScale = 0f;

                UIManager.Ins.ShowPausePanel();

                break;

            case GameState.GameOver:
                Time.timeScale = 0f;

                UIManager.Ins.ShowGameOverPanel();

                break;
        }


    }
    #endregion

    #region  Scene
    public void RestartScene()
    {
        Time.timeScale = 1f;
        IsGameOver = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region Application
    private void OnApplicationPause(bool pause)
    {
        if (pause && currentState == GameState.Playing)
        {
            PauseGame();
        }
    }
    #endregion

    public void Menu(int index)
    {
        SceneManager.LoadScene(index);
    }
}
