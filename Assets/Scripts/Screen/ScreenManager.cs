using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;

    public List<Screen> screens;
    [SerializeField] private Screen homeScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ShowScreen(homeScreen.ScreenId);
    }

    public void ShowScreen(SCREEN screenToShow)
    {
        foreach (var screen in screens)
        {
            screen.Initialize();
            if (screen.ScreenId == screenToShow)
            {
                screen.Show();
            }
            else
            {
                screen.Hide();
            }
        }
    }
}
