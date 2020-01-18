using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpawn : MonoBehaviour
{
    [SerializeField] private GameObject parentPos;
    [SerializeField] private Vector3 offset;
    private void Start()
    {
        if (parentPos == null)
        {
            Debug.LogError("parentPos missing on : " + gameObject.name);
            Destroy(gameObject);
        }
    }
    void LateUpdate()
    {
        transform.position = parentPos.transform.position + offset;
        transform.rotation = Quaternion.identity;
    }
}
