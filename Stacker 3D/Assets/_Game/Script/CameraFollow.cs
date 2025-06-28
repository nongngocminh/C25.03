using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; 
    [SerializeField] public Vector3 offset;
    [SerializeField] public float cameraMoveSpeed;

    void Start()
    {
        transform.position = player.position + offset;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.position + offset, cameraMoveSpeed * Time.deltaTime);
        }
    }
}
