using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class GameScreen : Screen
{
    public GameManager gameManager;

    private GetGameService getGameService;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private RectTransform gridParent;

    [SerializeField] private TMP_Text timerText;

    private int rows = 0;
    private int cols = 0;
    private float cellWidth;
    private float cellHeight;

    [SerializeField] private GameObject endPanel;

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private Button sendButton;
    [SerializeField] private TMP_Text totalTimeText;
    [SerializeField] private TMP_Text totalClicksText;
    [SerializeField] private TMP_Text totalPairsText;
    [SerializeField] private TMP_Text totalScoreText;

    private int finalScore;

    public override void Start()
    {
        OnShow += Init;
        GameManager.Instance.OnTimeUpdated += UpdateTimerDisplay;
    }

    protected virtual void Init()
    {
        gameManager.OnGameFinished += GameFinished;
        getGameService = new GetGameService(GameManager.Instance.dataPath);
        getGameService.GetGame(OnGameDataLoaded, OnGameDataError);

        finalScore = 0;
        endPanel.SetActive(false);
        inputName.text = "";
        sendButton.onClick.RemoveAllListeners();
        sendButton.onClick.AddListener(SaveScore);
    }

    private void OnGameDataLoaded(BlockDataList blockDataList)
    {
        Dictionary<string, UnityAction> goodActions = new Dictionary<string, UnityAction>
        {
            { "Confirmar", () => ConfirmAction(blockDataList) }
        };

        AlertManager.Instance.ShowAlert(ALERT.GOOD, "¡MUY BIEN!", "Validación del archivo exitosa.", goodActions);

        GameManager.Instance.SetTotalPairs(blockDataList.blocks.Count/2);
    }


    void CalculateGridSize(List<BlockData> blocks)
    {
        foreach (var block in blocks)
        {
            rows = Mathf.Max(rows, block.R);
            cols = Mathf.Max(cols, block.C);
        }
    }

    void ConfigureGrid()
    {
        cellWidth = gridParent.rect.width / cols;
        cellHeight = gridParent.rect.height / rows;
    }

    public void InitializeGrid(List<BlockData> blocks)
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
        foreach (var block in blocks)
        {
            var cellObject = Instantiate(cellPrefab, gridParent);
            var cell = cellObject.GetComponent<CardItem>();
            cell.Initialize(block.number);

            RectTransform cellRect = cellObject.GetComponent<RectTransform>();
            Vector2 cellPosition = CalculateCellPosition(block.R, block.C);
            cellRect.anchoredPosition = cellPosition;

            cellRect.sizeDelta = new Vector2(cellWidth, cellHeight);
        }
    }

    private void OnGameDataError(ValidationResult result)
    {
        string title = "Error";
        string message = "";

        switch (result)
        {
            case ValidationResult.ERROR_INVALID_ELEMENT_COUNT:
                title = "UPS...";
                message = "El número total de elementos es inválido. Debe estar entre 4 y 64.";
                break;

            case ValidationResult.ERROR_INVALID_DIMENSIONS:
                title = "Auch!";
                message = "Las dimensiones de la rejilla son inválidas. Las filas y columnas deben estar entre 2 y 8.";
                break;

            case ValidationResult.ERROR_ODD_ELEMENT_COUNT:
                title = "Error!";
                message = "El número total de elementos debe ser par para que todos los elementos tengan una pareja.";
                break;

            case ValidationResult.ERROR_INVALID_OCCURRENCES:
                title = "Auch!";
                message = "Un elemento tiene un número incorrecto de ocurrencias. Cada elemento debe contener un único par.";
                break;

            case ValidationResult.ERROR_DUPLICATE_POSITION:
                title = "Algo no está bien...";
                message = "Hay una posición duplicada. Cada posición debe ser única.";
                break;
        }

        Dictionary<string, UnityAction> actions = new Dictionary<string, UnityAction>
        {
            { "Cerrar", CancelAction }
        };
        AlertManager.Instance.ShowAlert(ALERT.ERROR, title, message, actions);
    }

    public void ConfirmAction(BlockDataList blockDataList)
    {
        AlertManager.Instance.HideAlert();

        CalculateGridSize(blockDataList.blocks);
        ConfigureGrid();
        InitializeGrid(blockDataList.blocks);

        GameManager.Instance.StartTimer();
    }

    private void UpdateTimerDisplay(float currentTime)
    {
        timerText.text = Mathf.FloorToInt(currentTime).ToString();
    }
    public void CancelAction()
    {
        AlertManager.Instance.HideAlert();
        ScreenManager.Instance.ShowScreen(SCREEN.MENU);
    }

    Vector2 CalculateCellPosition(int row, int column)
    {
        float x = (column - 1) * cellWidth + cellWidth / 2 - gridParent.rect.width / 2;
        float y = -((row - 1) * cellHeight + cellHeight / 2 - gridParent.rect.height / 2);

        return new Vector2(x, y);
    }

    private void GameFinished(int totalTime, int open, int pairs)
    {
        totalTimeText.text = totalTime.ToString();
        totalClicksText.text = open.ToString();
        totalPairsText.text = pairs.ToString();

        int totalScore = (pairs*10000) / (totalTime * open);

        finalScore = totalScore;
        totalScoreText.text = finalScore.ToString();

        endPanel.SetActive(true);
    }


    public void SaveScore()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            AlertManager.Instance.ShowAlert(ALERT.ALERT, "ups..", "El campo de nombre esta vacio", new Dictionary<string, UnityEngine.Events.UnityAction>());
        }
        else
        {
            ScoreList scoreList = ScoreService.LoadUserScoreList("UserScores");
            scoreList.scores.Add(new UserScore(inputName.text, finalScore));

            ScoreService.SaveUserScoreList("UserScores", scoreList);

            Dictionary<string, UnityAction> actions = new Dictionary<string, UnityAction>
            {
                { "Continuar", BackToMenu }
            };

            AlertManager.Instance.ShowAlert(ALERT.GOOD, "Bien Hecho!", "El puntaje se ha guardado correctamente", actions);
        }
    }

    private void BackToMenu()
    {
        endPanel.SetActive(false);

        AlertManager.Instance.HideAlert();
        ScreenManager.Instance.ShowScreen(SCREEN.MENU);
    }
}
