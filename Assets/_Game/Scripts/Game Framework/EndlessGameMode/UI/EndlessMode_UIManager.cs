using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMode_UIManager : MonoBehaviour
{
    public static EndlessMode_UIManager Instance { get; private set; }

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject powerUpPanel;

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
        pauseMenu.SetActive(false);
        powerUpPanel.SetActive(false);


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

    public void ReturnToMainMenu()
    {
        //TODO: load MainScene.
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}
