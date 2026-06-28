using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CarouselManager : MonoBehaviour
{
    public Inventory _inventory;

    public GameObject _placeholderPrefab;

    public Transform _carouselRing;

    public float _spacing = 1.5f;
    public float _maxRadius = 5f;

    public float _distanceFromCamera = 2f;

    bool _isOpen = false;

    int _currentIndex = 0;

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
            //Navegar por el carousel
            if (Input.GetKeyDown(KeyCode.A))
                NavigateCarousel(-1);
            if (Input.GetKeyDown(KeyCode.D))
                NavigateCarousel(1);
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
            GameObject spawnItem = Instantiate(_placeholderPrefab, _carouselRing);
            spawnItem.AddComponent<ItemShowcaseRotation>();
            spawnItem.transform.localPosition = pos;
            spawnItem.transform.localRotation = Quaternion.identity;
            spawnItem.layer = LayerMask.NameToLayer("Carousel");
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
        StartCoroutine(OpenAnimation(Camera.main.transform.position, transform.position));
        _isOpen = true;
        FirstPersonController.THIS.enabled = false;
        FirstPersonController.THIS.GetComponent<CharacterController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void CloseCarousel()
    {
        StartCoroutine(CloseAnimation(Camera.main.transform.position + Camera.main.transform.forward * _distanceFromCamera * 2f));
        _isOpen = false;
    }

    void NavigateCarousel(int direction)
    {   
        int totalItems = _inventory._inventorySlot.Count;
        _currentIndex = (_currentIndex + direction + totalItems) % totalItems;
        float angle = (360f / totalItems) * _currentIndex;
        StartCoroutine(RotateCarousel(_carouselRing.localRotation, Quaternion.Euler(0, angle, 0), 0.5f));
    }

    IEnumerator OpenAnimation(Vector3 startPos, Vector3 targetPos)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        startPos = Camera.main.transform.position + Camera.main.transform.forward * _distanceFromCamera;

        transform.localScale = Vector3.zero;
        transform.position = startPos;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, progress);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);
            yield return null;
        }

        transform.position = targetPos;
        transform.localScale = Vector3.one;
    }

    IEnumerator CloseAnimation(Vector3 targetPos)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, progress);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, progress);
            yield return null;
        }

        transform.position = targetPos;
        transform.localScale = Vector3.zero;
        ClearInventorySlot();
        FirstPersonController.THIS.enabled = true;
        FirstPersonController.THIS.GetComponent<CharacterController>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator RotateCarousel(Quaternion startRotation, Quaternion targetRotation, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            _carouselRing.localRotation = Quaternion.Slerp(startRotation, targetRotation, progress);
            yield return null;
        }
        _carouselRing.localRotation = targetRotation;
    }
}
