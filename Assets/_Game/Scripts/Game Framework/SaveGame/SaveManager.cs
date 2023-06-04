using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] string saveFileName;
    [SerializeField] bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;

    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (Instance == null)
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
        dataHandler = new FileDataHandler(Application.persistentDataPath, saveFileName, encryptData);

        saveManagers = FindAllSaveManagers();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        Debug.Log("Load game");

        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No saved data found!!!!");

            NewGame();
        }
        else
        {
            foreach (var manager in saveManagers)
            {
                manager.LoadData(gameData);
            }
        }
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);

        Debug.Log("Save Game");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    [ContextMenu("Delete Save File")]
    public void DeleteSaveFile()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, saveFileName, encryptData);
        dataHandler.Delete();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> savaManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(savaManagers);
    }

    public bool HasSaveData()
    {
        if (dataHandler.Load() != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
