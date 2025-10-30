using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMoving : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        _camera.transform.LookAt(_target);
        _camera.transform.RotateAround(_target.position, Vector3.up, _speed);
    }
}
