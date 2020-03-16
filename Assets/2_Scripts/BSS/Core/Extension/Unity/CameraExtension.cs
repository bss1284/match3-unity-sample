using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtension  {
    public static Vector2 GetWorldSize(this Camera camera) {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        return new Vector2(width, height);
    }
}
