using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    public Text _CurrentScoreUI;
    public Text _BestScoreUI;
    public Text _ResultScoreUI;
    public Text _NotifyUI;

    protected int _CurrentScore = 0;
    protected int _BestScore = 0;
    protected float _NotifyTime = 0.0f;

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
    public void Update()
    {
        if( _NotifyTime > 0.0f )
        {
            _NotifyUI.color = new Color(0, 0, 0, (_NotifyTime + 0.01f / 1.5f));
            _NotifyTime -= Time.deltaTime;

            if( _NotifyTime <= 0.0f )
            {
                _NotifyUI.gameObject.SetActive(false);
            }
        }
    }

    protected void UpdateScoreText()
    {
        _BestScoreUI.text = "Best Score :" + _BestScore;
        _CurrentScoreUI.text = "Current Score : " + _CurrentScore;
        _ResultScoreUI.text = _CurrentScore.ToString();
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


    public void ShowNotify(string _text)
    {
        _NotifyUI.text = _text;
        _NotifyUI.gameObject.SetActive(true);

        _NotifyTime = 1.5f;
    }
}
