using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class ScoreScreen : Screen
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject userScorePref;
    [SerializeField] private Transform scoreContainer;

    public override void Start()
    {
        base.Start();

        backButton.onClick.AddListener(GoToMenu);
    }

    public override void Show()
    {
        base.Show();

        ScoreList scoreList = ScoreService.LoadUserScoreList("UserScores");
        scoreList.scores = scoreList.scores.OrderByDescending(score => score.points).ToList();
        DisplayScores(scoreList);
    }

    private void DisplayScores(ScoreList userScoreList)
    {
        if (userScoreList.scores.Count == 0)
        {
            AlertManager.Instance.ShowAlert(ALERT.GOOD, "Bien Hecho!", "No hay datos disponibles", new Dictionary<string, UnityAction>());
            return;
        }

        foreach (Transform child in scoreContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (UserScore score in userScoreList.scores)
        {
            GameObject userObject = Instantiate(userScorePref, scoreContainer);
            userObject.GetComponent<ScoreItem>().Initialize(score);
        }
    }

    private void GoToMenu()
    {
        ScreenManager.Instance.ShowScreen(SCREEN.MENU);
    }
}
