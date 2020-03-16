using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Vector2Int coords;

    public bool isEmpty() {
        return !BlockManager.instance.ExistsBlock(coords);
    }


    public Block FindFillBlock() {
        //위쪽방향 우선 찾기
        var nextCoords = coords;
        Vector2Int maxUpCoords=coords;
        while (true) {
            nextCoords=BoardUtil.GetNeighbor(nextCoords, Direction.Up);
            if (!BoardManager.instance.IsEnable(nextCoords)) {
                maxUpCoords = BoardUtil.GetNeighbor(nextCoords, Direction.Down);
                break;
            }
            var nextBlock = BlockManager.instance.GetBlock(nextCoords);
            if (nextBlock != null) {
                return nextBlock;
            }
        }
        int centerX = Global.MAX_SIZE.x / 2;
        if (coords.x == centerX) {
            return null;
        }
        if (coords.x < centerX) {
            nextCoords = maxUpCoords;
            for (int i = 0; i < centerX - coords.x; i++) {
                nextCoords = BoardUtil.GetNeighbor(nextCoords, Direction.RightUp);
                var nextBlock = BlockManager.instance.GetBlock(nextCoords);
                if (nextBlock != null) {
                    return nextBlock;
                }
            }
            return null;
        } else {
            nextCoords = maxUpCoords;
            for (int i = 0; i < coords.x - centerX; i++) {
                nextCoords = BoardUtil.GetNeighbor(nextCoords, Direction.LeftUp);
                var nextBlock = BlockManager.instance.GetBlock(nextCoords);
                if (nextBlock != null) {
                    return nextBlock;
                }
            }
            return null;
        }
    }
}
