using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum Seed { Empty, X, O }
public enum GameState
{
    Playing,
    GameOver
}


public class TicTacToeCell : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image cellImage;

    private int _index;
    private TicTacToeController _controller;

    public void Initialize(int index, TicTacToeController controller)
    {
        _index = index;
        _controller = controller;

        cellImage.sprite = null;
        SetImageAlpha(0);

        button.interactable = true;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnCellClicked);
    }

    private void OnCellClicked()
    {
        _controller.HandleCellClick(_index, this);
    }
    private void SetImageAlpha(float alpha)
    {
        Color color = cellImage.color;
        color.a = alpha;
        cellImage.color = color;
    }

    public void UpdateVisual(Sprite playerSprite)
    {
        SetImageAlpha(1);
        cellImage.sprite = playerSprite;
        cellImage.gameObject.SetActive(true);
        button.interactable = false;
    }
}