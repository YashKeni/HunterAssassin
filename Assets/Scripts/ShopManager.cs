using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private Dictionary<string, bool> itemPurchaseStatus = new Dictionary<string, bool>();

    public bool isItemPurchased(string itemKey)
    {
        if (itemPurchaseStatus.ContainsKey(itemKey))
        {
            return itemPurchaseStatus[itemKey];
        }
        return false;
    }

    public void MarkItemAsPurchased(string itemKey)
    {
        itemPurchaseStatus[itemKey] = true;
    }
}
