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
                SSItem item = Instantiate(itemPrefab);

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

                Destroy(soldItem.gameObject);
            }

            yield return new WaitForSeconds(actionInterval);
        }
    }
}
