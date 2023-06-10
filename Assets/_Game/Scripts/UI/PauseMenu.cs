using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button toMainMenuButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        toMainMenuButton.onClick.AddListener(ToMainMenu);

        this.gameObject.SetActive(false);
    }

    private void ResumeGame()
    {
        this.gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    private void ToMainMenu()
    {
        this.gameObject.SetActive(false);
        GameManager.Instance.ReturnAllEnemy();
        UIManager.Instance.SwitchToMainMenuUI();
    }
}
