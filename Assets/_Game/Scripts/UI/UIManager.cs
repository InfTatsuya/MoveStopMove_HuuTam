using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }

    [SerializeField] GameObject ingameUI;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject weaponShopUI;
    [SerializeField] GameObject clothesShopUI;
    [SerializeField] GameObject revivePanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;

    [SerializeField] Button tryAgainButton;
    [SerializeField] Button nextLevelButton;

    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] TextMeshProUGUI starAmtText;
    [SerializeField] Button playButton;
    [SerializeField] Button weaponShopButton;
    [SerializeField] Button clothesShopButton;


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
        playButton.onClick.AddListener(NewGame);
        weaponShopButton.onClick.AddListener(OpenWeaponShopUI);
        clothesShopButton.onClick.AddListener(OpenClothesShopUI);

        nextLevelButton.onClick.AddListener(ResetGame);
        tryAgainButton.onClick.AddListener(ResetGame);

        SwitchToMainMenuUI();
        ingameUI.SetActive(true);
    }

    private void ResetGame()
    {
        SwitchToMainMenuUI();
        GameManager.Instance.ReturnAllEnemy();
    }

    private void NewGame()
    {
        SwitchToIngameUI();
        GameManager.Instance.StartNewGame();
    }

    private void DeactiveAll()
    {
        ingameUI.SetActive(false);
        mainMenuUI.SetActive(false);
        revivePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        weaponShopUI.SetActive(false);
        clothesShopUI.SetActive(false);
    }

    private void SwitchTo(GameObject ui)
    {
        DeactiveAll();
        ui.gameObject.SetActive(true);
    }

    public void SwitchToMainMenuUI()
    {
        SwitchTo(mainMenuUI);
        currencyText.text = ShopSystem.Instance.Money.ToString();
        starAmtText.text = "N/A";
    }

    public void SwitchToIngameUI()
    {
        SwitchTo(ingameUI);
    }

    public void SwitchToRevivePanel()
    {
        SwitchTo(revivePanel);
    }

    public void SwitchToWinPanel()
    {
        SwitchTo(winPanel);
    }

    public void SwitchToLosePanel()
    {
        SwitchTo(losePanel);
    }

    private void OpenWeaponShopUI()
    {
        weaponShopUI.SetActive(true);
    }

    private void OpenClothesShopUI()
    {
        clothesShopUI.SetActive(true);
    }
}
