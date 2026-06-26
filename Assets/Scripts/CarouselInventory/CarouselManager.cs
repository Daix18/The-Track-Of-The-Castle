using System.Collections.Generic;
using UnityEngine;

public class CarouselManager : MonoBehaviour
{
    public Inventory _inventory;

    public GameObject _placeholderPrefab;

    public float _spacing = 1.5f;
    public float _maxRadius = 5f;

    public float _distanceFromCamera = 2f;

    bool _isOpen = false;

    List<GameObject> _spawnedItems = new List<GameObject>();

    void Update()
    {
        if (!_isOpen)
        {
            //Abrir inventario y activar carousel
            if (Input.GetKeyDown(KeyCode.I))
                OpenCarousel();
        }
        else
        {
            //Cerrar inventario y desactivar carousel
            if (Input.GetKeyDown(KeyCode.I))
              CloseCarousel();
        }

    }

    void GetInventorySlot()
    {
        Transform camTransform = Camera.main.transform;
        Vector3 cameraPosition = camTransform.position;
        float dynamicRadius = CarouselLayout.GetDynamicRadius(_inventory._inventorySlot.Count, _spacing, _maxRadius);

        transform.position = cameraPosition + camTransform.forward * _distanceFromCamera;
        transform.rotation = Quaternion.LookRotation(-camTransform.forward, Vector3.up); 

        for (int i = 0; i < _inventory._inventorySlot.Count; i++)
        {
            var item = _inventory._inventorySlot[i];
            Vector3 pos = CarouselLayout.GetPositionForIndex(i, _inventory._inventorySlot.Count, dynamicRadius);
            GameObject spawnItem = Instantiate(_placeholderPrefab, transform);
            spawnItem.transform.localPosition = pos;
            spawnItem.transform.localRotation = Quaternion.identity;
            _spawnedItems.Add(spawnItem);
        }
    }

    void ClearInventorySlot()
    {
        foreach (var item in _spawnedItems)
            Destroy(item);
        _spawnedItems.Clear();
    }

    void OpenCarousel()
    {
        GetInventorySlot();
        _isOpen = true;
        FirstPersonController.THIS.enabled = false;
        FirstPersonController.THIS.GetComponent<CharacterController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void CloseCarousel()
    {
        ClearInventorySlot();
        _isOpen = false;
        FirstPersonController.THIS.enabled = true;
        FirstPersonController.THIS.GetComponent<CharacterController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
