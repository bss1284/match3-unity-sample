using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indexer2 : MonoBehaviour {
    public Indexer2 Init(int x,int y) {
        _x = x;
        _y = y;
        return this;
    }
    [SerializeField]
    private int _x;
    [SerializeField]
    private int _y;

    [SerializeField]
    private Vector2 xy {
        get { return new Vector2(_x, _y); }
    }

    public bool Equals(int x,int y) {
        return (x == _x && y == _y);
    }
    public bool Equals(Vector2 vec) {
        return ((int)vec.x == _x && (int)vec.y == _y);
    }
    public int GetX() {
        return _x;
    }
    public int GetY() {
        return _y;
    }
    public Vector2 GetVector() {
        return xy;
    }
}
