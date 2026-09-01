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

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip upgradeSfx;

    [SerializeField]
    [Range(0f, 1f)]
    private float upgradeVolume = 1f;

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

        PlayUpgradeSfx();

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

        PlayUpgradeSfx();

        capacityUpgradeCount++;
    }

    private void PlayUpgradeSfx()
    {
        if (audioSource == null || upgradeSfx == null)
            return;

        audioSource.PlayOneShot(upgradeSfx, upgradeVolume);
    }
}
