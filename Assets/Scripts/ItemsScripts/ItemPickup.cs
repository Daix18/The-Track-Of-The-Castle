using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    public Item Item;

    public void Pickup()
    {
        //Añade al objeto en la lista item del script InventoryManager
        InventoryManager.Instance.Add(Item);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        // activar la funcion pick up al clickear en el objeto 
        Pickup();
    }
}
