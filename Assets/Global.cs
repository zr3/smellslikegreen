using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    private static Global _instance;
    public static Global Config => _instance;

    public Color ClearColor = Color.green;

    void Awake() {
        if (_instance != null) {
            Debug.LogError("Cannot instantiate multiple instances of Global");
            Destroy(this);
        }
        else {
            _instance = this;
        }
    }
}
