using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IScreen
{
    public event Action OnShow;
    public event Action OnHide;
    void Hide();
    void Show();
}