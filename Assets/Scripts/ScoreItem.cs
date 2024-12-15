using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreItem : MonoBehaviour
{
    public TMP_Text userText;
    public TMP_Text scoreText;

    public void Initialize(UserScore score)
    {
        userText.text = score.user;
        scoreText.text = score.points.ToString();
    }
}
