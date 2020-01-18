using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Transform target;
    public GameObject greenBar;

    private void Start()
    {
        if (greenBar == null)
        {
            Debug.LogError("HealthBar is missing the greenBar GameObject");
            Destroy(gameObject);
        }
        if (target == null)
        {
            Debug.LogError("HealthBar is missing the target GameObject");
            Destroy(gameObject);
        }
    }
    private void LateUpdate()
    {
        transform.position = target.transform.position + Vector3.up * target.localScale.x / 2f;
        transform.rotation = Quaternion.identity;
    }

    public void ChangeValue(float value, float maxValue)
    {
        value /= maxValue; // becomes a scale
        greenBar.transform.localScale = new Vector3(value, greenBar.transform.localScale.y, 1f);
        greenBar.transform.localPosition = new Vector3((value / 2f) - 0.5f, greenBar.transform.localPosition.y, 0f);
    }
}
