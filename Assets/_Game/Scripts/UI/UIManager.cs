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

    [SerializeField] GameObject weaponShopView;
    [SerializeField] GameObject clothesShopView;

    [SerializeField] Button tryAgainButton;
    [SerializeField] Button nextLevelButton;

    [SerializeField] Sprite oneStar;
    [SerializeField] Sprite twoStar;
    [SerializeField] Sprite threeStar;
    [SerializeField] Image starImage;

    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] TextMeshProUGUI starAmtText;
    [SerializeField] TMP_InputField playerNameInputField;
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
        playerNameInputField.onEndEdit.AddListener(SetPlayerName);
        weaponShopButton.onClick.AddListener(OpenWeaponShopUI);
        clothesShopButton.onClick.AddListener(OpenClothesShopUI);

        nextLevelButton.onClick.AddListener(ResetGame);
        tryAgainButton.onClick.AddListener(ResetGame);

        SwitchToMainMenuUI();
        ingameUI.SetActive(true);

        DeactivateModelView();
    }

    private void SetPlayerName(string playerName)
    {
        GameManager.Instance.SetPlayerName(playerName);
    }

    private void ResetGame()
    {
        AdsManager.Instance.LoadInterstitialAds();

        GameManager.Instance.ReturnAllEnemy();
        GameManager.Instance.CalculateStarByKillAmount();
        
        SwitchToMainMenuUI();
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
        UpdateInfoOnScreen();
    }

    public void UpdateInfoOnScreen()
    {
        currencyText.text = ShopSystem.Instance.Money.ToString();
        starAmtText.text = GameManager.Instance.StarCount.ToString();
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
        SetUpLosePanel();
    }

    private void SetUpLosePanel()
    {
        int killCount = GameManager.Instance.PlayerKillCount;
        Sprite starSprite = oneStar;
        if(killCount > 30)
        {
            starSprite = twoStar;
        }
        if(killCount > 60)
        {
            starSprite = threeStar;
        }

        starImage.sprite = starSprite;
    }

    private void OpenWeaponShopUI()
    {
        weaponShopUI.SetActive(true);
        weaponShopView.SetActive(true);
    }

    private void OpenClothesShopUI()
    {
        clothesShopUI.SetActive(true);
        clothesShopView.SetActive(true);
    }

    public void DeactivateModelView()
    {
        weaponShopView.SetActive(false);
        clothesShopView.SetActive(false);
    }
}
