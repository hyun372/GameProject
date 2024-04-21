using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreTxt;

    public GameObject[] lifeStacks;

    public GameObject pausePanel;

    public GameObject gameOverPanel;
    public TMP_Text resultTitleTxt;
    public TMP_Text highScoreTxt;
    public TMP_Text resultScoreTxt;

    public GameObject bombImg;

    private void Awake()
    {
        pausePanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        scoreTxt.text = GameManagerGP.Instance._score.ToString();

        foreach (GameObject stack in lifeStacks)
        {
            stack.SetActive(false);
        }
        for (int i = 0; i < GameManagerGP.Instance._currentLife; i++)
        {
            lifeStacks[i].SetActive(true);
        }

        if (GameManagerGP.Instance._isGameOver)
        {
            if (GameManagerGP.Instance._isVictory)
            {
                resultTitleTxt.text = "Victory";
            }
            else
            {
                resultTitleTxt.text = "Game Over";
            }
        }

        highScoreTxt.text = GameManagerGP.Instance._highScore.ToString();
        resultScoreTxt.text = GameManagerGP.Instance._score.ToString();

        bombImg.SetActive(GameManagerGP.Instance._hasBomb);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        GameManagerGP.Instance._isPlaying = false;
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        GameManagerGP.Instance._isPlaying = true;
    }

    public void Restart()
    {
        GameManagerGP.Instance.Restart();
        Resume();
    }

    public void TimeMultiplier()
    {
        GameManagerGP.Instance.timeMultiplier = !GameManagerGP.Instance.timeMultiplier;
    }

    public void Invincibility()
    {
        GameManagerGP.Instance._isDamageable = !GameManagerGP.Instance._isDamageable;
    }

    public void ItemRate()
    {
        if (GameManagerGP.Instance._itemDropRate == 30)
            GameManagerGP.Instance._itemDropRate = 90;
        else
            GameManagerGP.Instance._itemDropRate = 30;
    }
}
