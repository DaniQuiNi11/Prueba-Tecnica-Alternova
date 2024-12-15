using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlertManager : MonoBehaviour
{
    public static AlertManager Instance;

    public AlertPanel alertPanel;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }

    public void ShowAlert(ALERT type, string title, string description, Dictionary<string, UnityAction> actions, bool closeButton = true)
    {
        if (Instance.alertPanel != null)
        {
            Instance.alertPanel.ShowAlert(type, title, description, actions, closeButton);
        }
        else
        {
            Debug.LogError("AlertPanel no está asignado en AlertManager.");
        }
    }

    public void HideAlert()
    {
        if (Instance.alertPanel != null)
        {
            Instance.alertPanel.HideAlert();
        }
    }
}
