using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    public Text _CurrentScoreUI;
    public Text _BestScoreUI;

    protected int _CurrentScore = 0;
    protected int _BestScore = 0;

    //protected int _

    const string best_score_key = "Best Score";

    public int Score
    {
        get { return _CurrentScore; }

        set
        {

            _CurrentScore = value;
            
            if( _CurrentScore > _BestScore)
            {
                _BestScore = _CurrentScore;

                PlayerPrefs.SetInt(best_score_key, _BestScore);

                UpdateScoreText();
            }
        }
    }

    public void Start()
    {
        _BestScore = PlayerPrefs.GetInt(best_score_key, 0);

        UpdateScoreText();
    }

    protected void UpdateScoreText()
    {
        _BestScoreUI.text = "최고 점수 : " + _BestScore;
        _CurrentScoreUI.text = "현재 점수 : " + _CurrentScore;

    }

    public void Clear()
    {
        _CurrentScore = 0;
    }

    public void AddScore( int _increased_score )
    {
        Score += _increased_score;

        UpdateScoreText();
    }
}
