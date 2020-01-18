using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEffect : MonoBehaviour
{
    public static float effectSize = 10f;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("MusicEffect is missing LineRenderer component : " + gameObject.name);
            Destroy(gameObject);
        }
        
    }

    public void UpdateShape(float angle)
    {
        List<Vector3> newPositions = new List<Vector3>();
        if (angle < 360)
            newPositions.Add(transform.position + Vector3.forward * -0.1f);
        for (float teta = -angle / 2f; teta < angle / 2f; teta += 10)
            newPositions.Add(CustomLib.MoveInDirection(transform.position, (transform.rotation.eulerAngles.z + teta) * Mathf.Deg2Rad, effectSize) + Vector3.forward * -0.1f);
        newPositions.Add(CustomLib.MoveInDirection(transform.position, (transform.rotation.eulerAngles.z + angle / 2f) * Mathf.Deg2Rad, effectSize) + Vector3.forward * -0.1f);
        lineRenderer.positionCount = newPositions.Count;
        lineRenderer.SetPositions(newPositions.ToArray());
    }
}
