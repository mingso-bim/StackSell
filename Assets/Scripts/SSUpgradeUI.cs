using TMPro;
using UnityEngine;

public class SSUpgradeUI : MonoBehaviour
{
    [SerializeField]
    private SSPlayerController playerController;

    [SerializeField]
    private SSPlayerCollector playerCollector;

    [SerializeField]
    private SSPlayerUpgradeData upgradeData;

    [SerializeField]
    private TMP_Text speedInfoText;

    [SerializeField]
    private TMP_Text capacityInfoText;

    private float displayedSpeed = -1f;
    private int displayedCapacity = -1;

    private void Update()
    {
        UpdateSpeedInfo();
        UpdateCapacityInfo();
    }

    private void UpdateSpeedInfo()
    {
        if (playerController == null)
        {
            Debug.LogWarning($"{name}: playerController가 설정되지 않아 Speed 정보를 표시할 수 없습니다.");
            return;
        }

        if (upgradeData == null)
        {
            Debug.LogWarning($"{name}: upgradeData가 설정되지 않아 Speed 정보를 표시할 수 없습니다.");
            return;
        }

        if (speedInfoText == null)
        {
            Debug.LogWarning($"{name}: speedInfoText가 설정되지 않아 Speed 정보를 표시할 수 없습니다.");
            return;
        }

        if (playerController.MoveSpeed == displayedSpeed)
            return;

        displayedSpeed = playerController.MoveSpeed;

        speedInfoText.text = $"Speed: {displayedSpeed} (Cost: {upgradeData.SpeedUpgradeCost})";
    }

    private void UpdateCapacityInfo()
    {
        if (playerCollector == null)
        {
            Debug.LogWarning($"{name}: playerCollector가 설정되지 않아 Capacity 정보를 표시할 수 없습니다.");
            return;
        }

        if (upgradeData == null)
        {
            Debug.LogWarning($"{name}: upgradeData가 설정되지 않아 Capacity 정보를 표시할 수 없습니다.");
            return;
        }

        if (capacityInfoText == null)
        {
            Debug.LogWarning($"{name}: capacityInfoText가 설정되지 않아 Capacity 정보를 표시할 수 없습니다.");
            return;
        }

        if (playerCollector.MaxCapacity == displayedCapacity)
            return;

        displayedCapacity = playerCollector.MaxCapacity;

        capacityInfoText.text = $"Capacity: {displayedCapacity} (Cost: {upgradeData.CapacityUpgradeCost})";
    }
}
