using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private int _score;

    private int _cleanCombo = 1;

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

        Debug.Log("Score: " + _score);
    }
}
