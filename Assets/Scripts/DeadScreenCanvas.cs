using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadScreenCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text winText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button restartButton;
    private void Awake()
    {
        canvas.enabled = false;
        restartButton.onClick.AddListener(() =>
        {
            canvas.enabled = false;
            SceneManager.LoadScene(0);
        });
    }

    public void ShowWinner(string winner)
    {
        canvas.enabled = true;
        winText.text = winner + " won!";
    }
}
