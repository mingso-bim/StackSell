using System.Collections;
using UnityEngine;

public class SSItemZone : MonoBehaviour
{
    [SerializeField]
    private SSItem itemPrefab;

    [SerializeField]
    private bool isSellZone;

    [SerializeField]
    private float actionInterval = 0.2f;

    [SerializeField]
    private float sellMoveDuration = 0.15f;

    private Coroutine productionCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (productionCoroutine != null)
            return;

        SSPlayerCollector collector = other.GetComponent<SSPlayerCollector>();

        if (collector == null)
        {
            Debug.LogWarning($"{name}: {other.name}에 SSPlayerCollector가 없어 무시합니다.");
            return;
        }

        if (isSellZone)
        {
            SSPlayerWallet wallet = other.GetComponent<SSPlayerWallet>();

            if (wallet == null)
            {
                Debug.LogWarning($"{name}: SSPlayerWallet이 없어 판매를 처리할 수 없습니다.");
                return;
            }

            productionCoroutine = StartCoroutine(SellItems(collector, wallet));
            return;
        }

        if (itemPrefab == null)
        {
            Debug.LogWarning($"{name}: itemPrefab이 설정되지 않아 아이템을 생성할 수 없습니다.");
            return;
        }

        productionCoroutine = StartCoroutine(ProduceItems(collector));
    }

    private void OnTriggerExit(Collider other)
    {
        SSPlayerCollector collector = other.GetComponent<SSPlayerCollector>();

        if (collector == null)
        {
            Debug.LogWarning($"{name}: {other.name}에 SSPlayerCollector가 없어 무시합니다.");
            return;
        }

        if (productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
            productionCoroutine = null;
        }
    }

    private IEnumerator ProduceItems(SSPlayerCollector collector)
    {
        while (true)
        {
            if (!collector.IsFull())
            {
                SSItem item = Instantiate(itemPrefab, transform.position, transform.rotation);

                collector.Collect(item);
            }

            yield return new WaitForSeconds(actionInterval);
        }
    }

    private IEnumerator SellItems(SSPlayerCollector collector, SSPlayerWallet wallet)
    {
        while (true)
        {
            SSItem soldItem = collector.RemoveOneItem();

            if (soldItem != null)
            {
                wallet.AddGold(soldItem.SellPrice);

                soldItem.transform.SetParent(null);

                StartCoroutine(MoveItemToZoneAndDestroy(soldItem));
            }

            yield return new WaitForSeconds(actionInterval);
        }
    }

    private IEnumerator MoveItemToZoneAndDestroy(SSItem item)
    {
        Vector3 startPosition = item.transform.position;
        Vector3 targetPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < sellMoveDuration)
        {
            if (item == null)
                yield break;

            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / sellMoveDuration);

            item.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        if (item == null)
            yield break;

        Destroy(item.gameObject);
    }
}
