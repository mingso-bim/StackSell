using System;
using System.IO;
using UnityEngine;

public class SSSaveSystem : MonoBehaviour
{
    [SerializeField]
    private SSPlayerWallet playerWallet;

    [SerializeField]
    private SSPlayerUpgrade playerUpgrade;

    public bool GameCleared { get; private set; }

    private string SavePath => Path.Combine(Application.persistentDataPath, "savedata.json");

    private void Awake()
    {
        if (playerWallet == null || playerUpgrade == null)
        {
            Debug.LogError($"{name}: 참조가 설정되지 않아 저장/불러오기를 비활성화합니다.");
            enabled = false;
        }
    }

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        try
        {
            SSSaveData data = new SSSaveData
            {
                gold = playerWallet.Gold,
                speedUpgradeCount = playerUpgrade.SpeedUpgradeCount,
                capacityUpgradeCount = playerUpgrade.CapacityUpgradeCount,
                gameCleared = GameCleared,
            };

            string json = JsonUtility.ToJson(data);

            File.WriteAllText(SavePath, json);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"{name}: 저장에 실패했습니다. {e.Message}");
        }
    }

    public void Load()
    {
        if (!File.Exists(SavePath))
            return;

        try
        {
            string json = File.ReadAllText(SavePath);

            SSSaveData data = JsonUtility.FromJson<SSSaveData>(json);

            if (data == null)
            {
                Debug.LogWarning($"{name}: 저장 데이터를 해석할 수 없어 불러오기를 건너뜁니다.");
                return;
            }

            playerWallet.SetGold(data.gold);
            playerUpgrade.RestoreUpgradeCounts(data.speedUpgradeCount, data.capacityUpgradeCount);
            GameCleared = data.gameCleared;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"{name}: 불러오기에 실패했습니다. {e.Message}");
        }
    }

    // 두 업그레이드가 모두 MAX가 되어 게임 클리어 처리될 때 호출한다.
    public void MarkGameCleared()
    {
        GameCleared = true;

        Save();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
