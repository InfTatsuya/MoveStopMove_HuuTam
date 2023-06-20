using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessMode_UIManager : MonoBehaviour
{
    public static EndlessMode_UIManager Instance { get; private set; }

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject powerUpPanel;
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] Button playAgainButton;
    [SerializeField] Button mainMenuButton;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        playAgainButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        Invoke(nameof(DeactiveAll), 0.5f);
    }

    private void DeactiveAll()
    {
        pauseMenu.SetActive(false);
        powerUpPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void OpenGameOverPanel()
    {
        DeactiveAll();
        gameOverPanel.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        EndlessGameMode.Instance.PauseGame();
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        EndlessGameMode.Instance.ResumeGame();
    }


    private void RestartGame()
    {
        EndlessGameMode.Instance.RestartGame();
    }

    public void ReturnToMainMenu()
    {
        EndlessGameMode.Instance.ReturnMainMenu();
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}
