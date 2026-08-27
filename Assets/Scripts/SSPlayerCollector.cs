using System.Collections.Generic;
using UnityEngine;

public class SSPlayerCollector : MonoBehaviour
{
    [SerializeField]
    private Transform stackRoot;

    [SerializeField]
    private float stackSpacing = 0.5f;

    [SerializeField]
    private int maxCapacity = 5;

    private readonly List<SSItem> collectedItems = new();

    public bool IsFull()
    {
        return collectedItems.Count >= maxCapacity;
    }

    public void Collect(SSItem item)
    {
        collectedItems.Add(item);

        item.transform.SetParent(stackRoot);

        int index = collectedItems.Count - 1;

        item.transform.localPosition = new Vector3(0f, index * stackSpacing, 0f);

        item.transform.localRotation = Quaternion.identity;
    }
}
