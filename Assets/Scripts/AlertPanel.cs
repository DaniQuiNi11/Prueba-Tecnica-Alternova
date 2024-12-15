using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

public class AlertPanel : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text messageText;
    public Image borderColor;
    public Image lineColor;
    public Color[] colors;
    public GameObject alertPanel;

    public GameObject actionBtnPrefab;
    public Transform actionBtnsContainer;
    public GameObject closeBtn;

    public virtual void ShowAlert(ALERT type, string title, string description, Dictionary<string, UnityAction> actions, bool closeButton)
    {
        AlertModel alertModel = new AlertModel
        {
            Title = title,
            Description = description,
            HasCloseButton = closeButton,
            Actions = actions
        };
        InstantiateButtons(alertModel);

        borderColor.color = colors[(int)type];
        lineColor.color = colors[(int)type];

        titleText.text = title;
        messageText.text = description;
        alertPanel.SetActive(true);
    }

    protected virtual void InstantiateButtons(AlertModel popUpModel)
    {
        for (int i = 0; i < actionBtnsContainer.transform.childCount; i++)
        {
            Destroy(actionBtnsContainer.transform.GetChild(i).gameObject);
        }


        if (popUpModel.Actions.Count <= 0)
            popUpModel.Actions = DefaultAction();

        foreach (var act in popUpModel.Actions)
        {
            GameObject newActionBtn = Instantiate(actionBtnPrefab, actionBtnsContainer.transform) as GameObject;

            newActionBtn.GetComponentInChildren<TMP_Text>().text = act.Key;
            newActionBtn.GetComponent<Button>().onClick.AddListener(act.Value);
        }
    }

    private Dictionary<string, UnityAction> DefaultAction()
    {
        Dictionary<string, UnityAction> actions = new Dictionary<string, UnityAction>
        {
            { "Cerrar", HideAlert }
        };

        return actions;
    }
    public void HideAlert()
    {
        gameObject.SetActive(false);
    }
}


public class AlertModel
{
    public string Title;
    public string Description;
    public bool HasCloseButton;
    public Dictionary<string, UnityAction> Actions;
}