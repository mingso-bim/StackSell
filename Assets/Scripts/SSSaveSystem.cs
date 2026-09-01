using System;
using System.IO;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class SSSaveSystem : MonoBehaviour
{
    [SerializeField]
    private SSPlayerWallet playerWallet;

    [SerializeField]
    private SSPlayerController playerController;

    [SerializeField]
    private SSPlayerCollector playerCollector;

    private string SavePath => Path.Combine(Application.persistentDataPath, "savedata.json");

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
                moveSpeed = playerController.MoveSpeed,
                maxCapacity = playerCollector.MaxCapacity,
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
            playerController.SetMoveSpeed(data.moveSpeed);
            playerCollector.SetMaxCapacity(data.maxCapacity);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"{name}: 불러오기에 실패했습니다. {e.Message}");
        }
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
