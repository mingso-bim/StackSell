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

    public int SpeedUpgradeCount => speedUpgradeCount;
    public int CapacityUpgradeCount => capacityUpgradeCount;

    // 클리어 판정의 source of truth. UI는 이 값만 읽고 계산식을 중복하지 않는다.
    public bool IsAllUpgradesMaxed =>
        speedUpgradeCount >= upgradeData.MaxUpgradeCount &&
        capacityUpgradeCount >= upgradeData.MaxUpgradeCount;

    // 세이브에서 불러온 업그레이드 횟수를 복원한다. 0 ~ MaxUpgradeCount 범위로 Clamp.
    public void RestoreUpgradeCounts(int speedCount, int capacityCount)
    {
        speedUpgradeCount = Mathf.Clamp(speedCount, 0, upgradeData.MaxUpgradeCount);
        capacityUpgradeCount = Mathf.Clamp(capacityCount, 0, upgradeData.MaxUpgradeCount);
    }

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
