using UnityEngine;

public class SSPlayerUpgrade : MonoBehaviour
{
    [SerializeField]
    private SSPlayerUpgradeData upgradeData;

    [SerializeField]
    private SSPlayerController playerController;

    [SerializeField]
    private SSPlayerCollector playerCollector;

    [SerializeField]
    private SSPlayerWallet playerWallet;

    private int speedUpgradeCount;
    private int capacityUpgradeCount;

    private void Start()
    {
        playerController.SetMoveSpeed(upgradeData.BaseMoveSpeed);
        playerCollector.SetMaxCapacity(upgradeData.BaseCapacity);
    }

    public void UpgradeSpeed()
    {
        if (speedUpgradeCount >= upgradeData.MaxUpgradeCount)
            return;

        if (!playerWallet.TrySpendGold(upgradeData.SpeedUpgradeCost))
            return;

        float newSpeed = playerController.MoveSpeed + upgradeData.SpeedUpgradeAmount;

        playerController.SetMoveSpeed(newSpeed);

        speedUpgradeCount++;
    }

    public void UpgradeCapacity()
    {
        if (capacityUpgradeCount >= upgradeData.MaxUpgradeCount)
            return;

        if (!playerWallet.TrySpendGold(upgradeData.CapacityUpgradeCost))
            return;

        int newCapacity = playerCollector.MaxCapacity + upgradeData.CapacityUpgradeAmount;

        playerCollector.SetMaxCapacity(newCapacity);

        capacityUpgradeCount++;
    }
}
