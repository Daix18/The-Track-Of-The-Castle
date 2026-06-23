using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string _itemName;
    public Sprite _itemIcon;
    public GameObject _itemModel;
    public string _itemDescription;
    public string _itemID;
}
