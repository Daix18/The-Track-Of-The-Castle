using UnityEngine;

public class ItemShowcaseRotation : MonoBehaviour
{
    public float _rotationSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
