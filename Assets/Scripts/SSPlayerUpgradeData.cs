using UnityEngine;

[CreateAssetMenu(fileName = "SSPlayerUpgradeData", menuName = "StackSell/SSPlayerUpgradeData")]
public class SSPlayerUpgradeData : ScriptableObject
{
    [SerializeField]
    private float baseMoveSpeed = 5f;

    [SerializeField]
    private int baseCapacity = 5;

    [SerializeField]
    private int speedUpgradeCost = 10;

    [SerializeField]
    private float speedUpgradeAmount = 1f;

    [SerializeField]
    private int capacityUpgradeCost = 10;

    [SerializeField]
    private int capacityUpgradeAmount = 1;

    [SerializeField]
    private int maxUpgradeCount = 5;

    public float BaseMoveSpeed => baseMoveSpeed;
    public int BaseCapacity => baseCapacity;
    public int SpeedUpgradeCost => speedUpgradeCost;
    public float SpeedUpgradeAmount => speedUpgradeAmount;
    public int CapacityUpgradeCost => capacityUpgradeCost;
    public int CapacityUpgradeAmount => capacityUpgradeAmount;
    public int MaxUpgradeCount => maxUpgradeCount;
}
