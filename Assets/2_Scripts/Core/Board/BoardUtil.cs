using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS.Extension;
using System;
using System.Linq;

public static class BoardUtil 
{
    /// <summary>
    /// 실제 보드판의 월드 좌표
    /// </summary>
    /// <param name="coords">월드 포지션</param>
    public static Vector3 GetPosition(Vector2Int coords) {
        int x = coords.x;
        int y = coords.y;
        return new Vector3((x - (Global.MAX_SIZE.x / 2)) * 1.775f, (y - (Global.MAX_SIZE.y / 2)) * 0.975f, 0f);
    }

    /// <summary>
    /// 기준 좌표에서 이웃한 좌표 반환
    /// </summary>
    /// <param name="origin">기준좌표</param>
    /// <param name="dir">방향</param>
    /// <returns>이웃 좌표</returns>
    public static Vector2Int GetNeighbor(Vector2Int origin, Direction dir) {
        switch (dir) {
            case Direction.LeftUp:
                return new Vector2Int(origin.x - 1, origin.y + 1);
            case Direction.Up:
                return new Vector2Int(origin.x, origin.y + 2);
            case Direction.RightUp:
                return new Vector2Int(origin.x + 1, origin.y + 1);
            case Direction.LeftDown:
                return new Vector2Int(origin.x - 1, origin.y - 1);
            case Direction.Down:
                return new Vector2Int(origin.x, origin.y - 2);
            case Direction.RightDown:
                return new Vector2Int(origin.x + 1, origin.y - 1);
            case Direction.RightUpOffset:
                return new Vector2Int(origin.x + 1, origin.y + 3);
            case Direction.RightOffset:
                return new Vector2Int(origin.x + 2, origin.y);
            case Direction.RightDownOffset:
                return new Vector2Int(origin.x + 1, origin.y - 3);
            case Direction.LeftDownOffset:
                return new Vector2Int(origin.x - 1, origin.y - 3);
            case Direction.LeftOffset:
                return new Vector2Int(origin.x - 2, origin.y);
            case Direction.LeftUpOffset:
                return new Vector2Int(origin.x - 1, origin.y+3);
        }
        return Vector2Int.zero;
    }

    /// <summary>
    /// 기준 좌표에서 이웃한 모든 좌표 반환 
    /// </summary>
    public static IEnumerable<Vector2Int> GetNeighborAll(Vector2Int origin) {
        return Global.NEIGHBOR_DIRECTIONS.Select(x => GetNeighbor(origin, x));
    }

    /// <summary>
    /// 두 좌표가 서로 이웃한 좌표인지 확인
    /// </summary>
    public static bool IsNeighbor(Vector2Int coords1, Vector2Int coords2) {
        int xDiff=Mathf.Abs(coords1.x - coords2.x);
        int yDiff = Mathf.Abs(coords1.y - coords2.y);
        return xDiff <= 1 && yDiff <= 2;
    }

    /// <summary>
    /// 시작위치에서 끝위치까지가 어떤 방향인지 반환
    /// </summary>ㄴ
    /// <param name="startPos">시작 월드 포지션</param>
    /// <param name="endPos">끝 월드 포지션</param>
    /// <returns>방향</returns>
    public static Direction GetDirection(Vector3 startPos,Vector3 endPos) {
        float degree=Vector3Ex.GetDirectionDegree(startPos,endPos);
        if (degree <= 60f) return Direction.RightUp;
        if (degree <= 120f) return Direction.Up;
        if (degree <= 180f) return Direction.LeftUp;
        if (degree <= 240f) return Direction.LeftDown;
        if (degree <= 300f) return Direction.Down;
        return Direction.RightDown;
    }

    public static Direction GetOffsetDirection(Direction dir1, Direction dir2) {
        int intDir1 = (int)dir1;
        int intDir2 = (int)dir2;
        if (intDir1 > 10 || intDir2 > 10) throw new Exception("파라미터의 유효범위가 아닙니다.");
        return (Direction)(((intDir1 * intDir1) + (intDir2 * intDir2)) * 10);
    }
}
