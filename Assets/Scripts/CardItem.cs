using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CardItem : MonoBehaviour
{
    private Animator animator;
    private int value;
    private Button button;

    [SerializeField] private Image cardImage;

    bool showing;

    void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();

        button.onClick.AddListener(NotifyCardSelected);
    }

    public void Initialize(int v)
    {
        value = v;

        cardImage.sprite = GameManager.Instance.GetCardImage(value-1);
    }

    private void NotifyCardSelected()
    {
        if(!showing)
            GameManager.Instance.OnCardSelected(value, this);
    }

    public void Reveal()
    {
        showing = true;
        animator.Play("Card_Show");
    }

    public void Hide()
    {
        showing = false;
        animator.Play("Card_Hide");
    }

    public void End()
    {
        showing = true;
        animator.Play("Card_Win");
    }
}
