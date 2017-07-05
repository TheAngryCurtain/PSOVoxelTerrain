using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 offset = new Vector3(-5f, 6f, -3f);

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        transform.position = target.position + offset;
    }
}
