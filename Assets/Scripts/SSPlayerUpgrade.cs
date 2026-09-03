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
    // 비활성 상태(참조 누락)에서는 upgradeData 접근 전에 short-circuit으로 막는다.
    public bool IsAllUpgradesMaxed =>
        enabled &&
        speedUpgradeCount >= upgradeData.MaxUpgradeCount &&
        capacityUpgradeCount >= upgradeData.MaxUpgradeCount;

    // 세이브에서 불러온 업그레이드 횟수를 복원하고, 그 횟수에 맞춰 능력치를 다시 적용한다.
    // 0 ~ MaxUpgradeCount 범위로 Clamp.
    public void RestoreUpgradeCounts(int speedCount, int capacityCount)
    {
        // 비활성 상태면 예외 대신 조용히 무시한다. (호출자 SSSaveSystem.Load()가
        // 이후 GameCleared 복원을 계속 진행할 수 있도록)
        if (!enabled)
            return;

        speedUpgradeCount = Mathf.Clamp(speedCount, 0, upgradeData.MaxUpgradeCount);
        capacityUpgradeCount = Mathf.Clamp(capacityCount, 0, upgradeData.MaxUpgradeCount);

        ApplyUpgradeCounts();
    }

    private void Awake()
    {
        if (upgradeData == null || playerController == null ||
            playerCollector == null || playerWallet == null)
        {
            Debug.LogError($"{name}: 참조가 설정되지 않아 업그레이드를 비활성화합니다.");
            enabled = false;
        }
    }

    private void Start()
    {
        ApplyUpgradeCounts();
    }

    // moveSpeed/capacity는 업그레이드 횟수로부터 계산되는 파생 값이다.
    // 여기가 유일한 적용 지점이고, 세이브/로드도 횟수만 저장한다.
    private void ApplyUpgradeCounts()
    {
        float moveSpeed = upgradeData.BaseMoveSpeed + upgradeData.SpeedUpgradeAmount * speedUpgradeCount;
        int capacity = upgradeData.BaseCapacity + upgradeData.CapacityUpgradeAmount * capacityUpgradeCount;

        playerController.SetMoveSpeed(moveSpeed);
        playerCollector.SetMaxCapacity(capacity);
    }

    public void UpgradeSpeed()
    {
        // Awake에서 참조 누락으로 비활성화된 경우 Button/UnityEvent 호출을 무시한다.
        if (!enabled)
            return;

        if (speedUpgradeCount >= upgradeData.MaxUpgradeCount)
            return;

        if (!playerWallet.TrySpendGold(upgradeData.SpeedUpgradeCost))
            return;

        speedUpgradeCount++;

        ApplyUpgradeCounts();

        PlayUpgradeSfx();
    }

    public void UpgradeCapacity()
    {
        // Awake에서 참조 누락으로 비활성화된 경우 Button/UnityEvent 호출을 무시한다.
        if (!enabled)
            return;

        if (capacityUpgradeCount >= upgradeData.MaxUpgradeCount)
            return;

        if (!playerWallet.TrySpendGold(upgradeData.CapacityUpgradeCost))
            return;

        capacityUpgradeCount++;

        ApplyUpgradeCounts();

        PlayUpgradeSfx();
    }

    private void PlayUpgradeSfx()
    {
        if (audioSource == null || upgradeSfx == null)
            return;

        audioSource.PlayOneShot(upgradeSfx, upgradeVolume);
    }
}
