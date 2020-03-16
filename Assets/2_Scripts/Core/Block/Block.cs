using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;

public class Block : MonoBehaviour
{
    public Vector2Int coords;
    public BlockType type;


    private void Awake() {
        var mover=gameObject.AddComponent<Mover>();
        mover.stopDistance = 0.01f;
        mover.speed = Global.BLOCK_SWAP_SPEED;
    }

    public void ChangeCoordsAndMove(Vector2Int targetCoords, float speed) {
        if (coords.x == targetCoords.x) {
            //직선이동
            var pos = BoardUtil.GetPosition(targetCoords);
            ToGoal(pos, speed);
        } else {
            //곡선이동
            var mover = GetComponent<Mover>();
            Vector3 via = GetVia(coords, targetCoords);
            var des = new List<Vector3>();
            if (mover.IsMoving()) {
                des.Add(mover.destination);
            }
            des.Add(via);
            des.Add(BoardUtil.GetPosition(targetCoords));
            mover.speed = speed;
            mover.ToGoal(des);
        }
        coords = targetCoords;
    }

    public virtual void TryDestroy() {
        BlockManager.instance.blocks.Remove(this);
        Destroy(gameObject);
    }

    public void ToGoal(Vector3 position, float speed) {
        var mover = GetComponent<Mover>();
        mover.speed = speed;
        mover.ToGoal(position);
    }
    public bool IsMoving() {
        var mover = GetComponent<Mover>();
        return mover.IsMoving();
    }


    private Vector3 GetVia(Vector2Int startCoords, Vector2Int targetCoords) {
        var coords = startCoords;
        int count = Mathf.Abs(targetCoords.x - startCoords.x);
        Direction dir = Direction.LeftDown;
        if (startCoords.x < targetCoords.x) {
            dir = Direction.RightDown;
        }
        for (int i = 0; i < count; i++) {
            coords = BoardUtil.GetNeighbor(coords, dir);
        }
        return BoardUtil.GetPosition(coords);
    }
}
