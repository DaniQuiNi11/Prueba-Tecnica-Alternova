using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Screen : MonoBehaviour, IScreen
{
    public SCREEN ScreenId;

    public event Action OnShow;
    public event Action OnHide;

    protected GameObject MainScreen;

    public virtual void Start()
    {
        
    }

    public virtual void Initialize()
    {
        MainScreen = transform.GetChild(0).gameObject;
    }

    public virtual void Hide()
    {
        MainScreen.SetActive(false);
        OnHide?.Invoke();
    }

    public virtual void Show()
    {
        MainScreen.SetActive(true);
        OnShow?.Invoke();
    }
}
