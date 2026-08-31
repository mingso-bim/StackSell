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

    public int MaxCapacity => maxCapacity;

    private readonly List<SSItem> collectedItems = new();

    public void SetMaxCapacity(int newCapacity)
    {
        maxCapacity = newCapacity;
    }

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

    public bool IsEmpty()
    {
        return collectedItems.Count == 0;
    }

    public SSItem RemoveOneItem()
    {
        if (IsEmpty())
            return null;

        int lastIndex = collectedItems.Count - 1;

        SSItem item = collectedItems[lastIndex];

        collectedItems.RemoveAt(lastIndex);

        return item;
    }
}
