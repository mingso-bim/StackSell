using System.Collections;
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

    [SerializeField]
    private float moveDuration = 0.15f;

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

        Vector3 targetLocalPosition = new Vector3(0f, index * stackSpacing, 0f);

        item.transform.localRotation = Quaternion.identity;

        StartCoroutine(MoveItemToStack(item, targetLocalPosition));
    }

    private IEnumerator MoveItemToStack(SSItem item, Vector3 targetLocalPosition)
    {
        Vector3 startLocalPosition = item.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            if (item == null)
                yield break;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);

            item.transform.localPosition = Vector3.Lerp(startLocalPosition, targetLocalPosition, t);

            yield return null;
        }

        if (item == null)
            yield break;

        item.transform.localPosition = targetLocalPosition;
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
