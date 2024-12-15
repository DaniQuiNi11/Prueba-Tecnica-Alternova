using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : Screen
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button scoreButton;

    public override void Start()
    {
        base.Start();
        playButton.onClick.AddListener(PlayGame);
        scoreButton.onClick.AddListener(ShowScore);
    }
    
    private void PlayGame()
    {
        ScreenManager.Instance.ShowScreen(SCREEN.GAME);
    }

    private void ShowScore()
    {
        ScreenManager.Instance.ShowScreen(SCREEN.SCORE);
    }
}
