using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class TransformExtension {
    public static RectTransform ToRect(this Transform transform) {
        return transform.GetComponent<RectTransform>();
    }

    public static T FindChild<T>(this Transform transform, string name) where T : Component {
        T[] comps = transform.GetComponentsInChildren<T>();
        return comps.ToList().Find(x => x.name == name);
    }
    public static void AddChildren(this Transform transform, GameObject[] children) {
        Array.ForEach(children, child => child.transform.parent = transform);
    }
    public static void AddChildren(this Transform transform, Component[] children) {
        Array.ForEach(children, child => child.transform.parent = transform);
    }
    public static void ResetChildPositions(this Transform transform, bool recursive = false) {
        foreach (Transform child in transform) {
            child.position = Vector3.zero;
            if (recursive) {
                child.ResetChildPositions(recursive);
            }
        }
    }
    public static int GetParentCount(this Transform transform) {
        Transform parent = transform;
        int sum = 0;
        while (true) {
            parent = parent.parent;
            if (parent == null) {
                return sum;
            }
            sum++;
        }
    }


    #region Posion값 함수
    public static void SetPosition(this Transform transform, Vector3 pos) {
        transform.position = pos;
    }
    public static void SetPosX(this Transform transform,float x) {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    public static void SetPosY(this Transform transform, float y) {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    public static void SetPosZ(this Transform transform, float z) {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
    public static void AddPosition(this Transform transform, Vector3 pos) {
        transform.position += pos;
    }
    public static void AddPosX(this Transform transform, float x) {
        transform.position += new Vector3(x, 0f, 0f);
    }
    public static void AddPosY(this Transform transform, float y) {
        transform.position += new Vector3(0f, y, 0f);
    }
    public static void AddPosZ(this Transform transform, float z) {
        transform.position += new Vector3(0f, 0f, z);
    }
    #endregion
}
