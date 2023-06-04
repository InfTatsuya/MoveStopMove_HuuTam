using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public static IndicatorManager Instance { get; private set; }

    [SerializeField] Indicator indicatorPrefab;

    [SerializeField] Dictionary<Character, Indicator> indicatorDict = new Dictionary<Character, Indicator>();

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

    public void CreateNewIndicator(Character character)
    {
        if (indicatorDict.ContainsKey(character)) return;
        
        Indicator instance = Instantiate(indicatorPrefab, transform);
        instance.SetupIndicator(character);
        indicatorDict.Add(character, instance);
        
    }

    public void RemoveIndicator(Character character)
    {
        if(indicatorDict.TryGetValue(character, out var indicator))
        {
            Destroy(indicator.gameObject); 
            indicatorDict.Remove(character);
        }
    }
}
