using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndlessMode_SwitchButton : MonoBehaviour
{
    [SerializeField] Button thisButton;

    private void Start()
    {
        thisButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        GameManager.Instance.ReturnAllEnemy(); 
        DataTransfer.Instance.SetPlayerData();
        SceneManager.LoadScene(2);
    }
}
