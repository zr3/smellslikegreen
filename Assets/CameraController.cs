using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera _camera;
    void Awake() {
        _camera = GetComponent<Camera>();
        _camera.backgroundColor = Global.Config.ClearColor;
    }
}
