using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotttonMove : MonoBehaviour
{
    [SerializeField] private Button prefabButton;

    private IEnumerator MoveButtonCoroutine(RectTransform buttonTransform)
    {
        Vector3 center = buttonTransform.localPosition;
        float radius = 100f;
        float angle = 0f;

        while (angle < 360f)
        {
            float x = center.x + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = center.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            buttonTransform.localPosition = new Vector3(x, y, center.z);
            angle += Time.deltaTime * 50f; // Controls the speed of rotation
            yield return null;
        }

        buttonTransform.localPosition = center;
    }

    private void OnButtonClicked()
    {
        // Find existing button with prefabButton name
        Button button = GameObject.Find("Boton_Item").GetComponent<Button>();

        if (button != null)
        {
            StartCoroutine(MoveButtonCoroutine(button.GetComponent<RectTransform>()));
        }
    }
}



