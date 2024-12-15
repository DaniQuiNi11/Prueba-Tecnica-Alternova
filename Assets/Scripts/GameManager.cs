using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string dataPath;

    [SerializeField] private Sprite[] cardImages;

    private int? firstSelectedCardValue = null;
    private CardItem firstSelectedCell = null;

    private int? secondSelectedCardValue = null;
    private CardItem secondSelectedCell = null;

    private bool isComparing = false;

    private int totalPairs;
    private int found;
    private int open;

    public event Action<float> OnTimeUpdated;
    private float totalTime = 0;
    private bool isTimerRunning;

    public event Action<int, int, int> OnGameFinished;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ShuffleList(cardImages);
    }

    public void SetTotalPairs(int val)
    {
        totalPairs = val;
    }

    public Sprite GetCardImage(int val)
    {
        return cardImages[val];
    }

    public void OnCardSelected(int cardValue, CardItem card)
    {
        if (isComparing)
            return;

        open++;

        if (!firstSelectedCardValue.HasValue)
        {
            firstSelectedCardValue = cardValue;
            firstSelectedCell = card;
            card.Reveal();
        }else if (!secondSelectedCardValue.HasValue)
        {
            secondSelectedCardValue = cardValue;
            secondSelectedCell = card;
            card.Reveal();

            StartCoroutine(CompareCards());
        }
    }

    private IEnumerator<WaitForSeconds> CompareCards()
    {
        isComparing = true;

        yield return new WaitForSeconds(1f);

        if (firstSelectedCardValue == secondSelectedCardValue)
        {
            found++;

            firstSelectedCell.End();
            secondSelectedCell.End();
        }else
        {
            firstSelectedCell.Hide();
            secondSelectedCell.Hide();
        }

        firstSelectedCardValue = null;
        firstSelectedCell = null;
        secondSelectedCardValue = null;
        secondSelectedCell = null;

        isComparing = false;

        if(found == totalPairs)
        {
            GameFinished();
        }
    }

    void ShuffleList(Sprite[] list)
    {
        System.Random random = new System.Random();
        for (int i = list.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            Sprite temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            totalTime += Time.deltaTime;
            OnTimeUpdated?.Invoke(totalTime);
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void GameFinished()
    {
        StopTimer();

        OnGameFinished?.Invoke(Mathf.FloorToInt(totalTime), open, found);

        totalTime = 0;
        open = 0;
        found = 0;
        totalPairs = 0;
    }
}

[System.Serializable]
public class BlockData
{
    public int R;
    public int C;
    public int number;
}

[System.Serializable]
public class BlockDataList
{
    public List<BlockData> blocks;
}