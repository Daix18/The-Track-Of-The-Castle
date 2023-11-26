using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    // Start is called before the first frame update
    //Instancia el codigo 
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(Item item)//añade a la lista Item 
    {
        Items.Add(item);
    }

    public void Remove(Item item)//remueve de la lista Item 
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        //Limpia el contenido antes de abrirse
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            //instancia el objeto en el inventario
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            //añade el texto e imagen del item del objeto 
            //var ItemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemName = obj.transform.Find("TextItemName").GetComponent<TextMeshProUGUI>();
            var ItemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();


            itemName.text = item.Name;
            ItemIcon.sprite = item.icon;
            /*if (obj != null)
            {
                //Buscamos el componente "InventoryManager" en el objeto "targetObject"
                 Icon = obj.transform.Find("Icon").GetComponent<Text>();
                if (itemName != null)
                {
                    //Llamada a la función "ListItems" en el objeto "inventory"
                    //InventoryManager.ListItems();
                    Debug.LogError("no nulo ");
                }
                else
                {
                    Debug.LogError("El componente itemName no se encuentra en el objeto " + item.name);
                }
            }
            else
            {
                Debug.LogError("El objeto itemName es nulo");
            }*/
        }
    }

}


