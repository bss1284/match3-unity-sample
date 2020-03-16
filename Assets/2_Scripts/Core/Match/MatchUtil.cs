using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class MatchUtil 
{
    public static Direction LookAtStart(MatchDirection matchDir) {
        Direction dir = Direction.None;
        if (matchDir == MatchDirection.Vertical) {
            dir = Direction.Down;
        } else if (matchDir == MatchDirection.ForwardSlash) {
            dir = Direction.LeftDown;
        } else if (matchDir == MatchDirection.BackSlash) {
            dir = Direction.LeftUp;
        }
        return dir;
    }

    public static Direction LookAtEnd(MatchDirection matchDir) {
        Direction dir = Direction.None;
        if (matchDir == MatchDirection.Vertical) {
            dir = Direction.Up;
        } else if (matchDir == MatchDirection.ForwardSlash) {
            dir = Direction.RightUp;
        } else if (matchDir == MatchDirection.BackSlash) {
            dir = Direction.RightDown;
        }
        return dir;
    }
    /// <summary>
    /// MatchInfo List에서 좌표가 전부 같은 중복을 제거합니다.
    /// </summary>
    public static List<MatchInfo> Distinct(List<MatchInfo> result) {
        return result.Distinct(new ListOfMatchInfoComparer()).ToList();
    }

    /// <summary>
    /// MatchInfo List에서 중복을 제거하여 모든 좌표를 반환합니다.
    /// </summary>
    public static List<Vector2Int> GetCoordsAll(List<MatchInfo> results) {
        var coordsAll = new List<Vector2Int>();
        foreach (var result in results) {
            foreach (var coord in result.coords) {
                if (!coordsAll.Exists(x => x.Equals(coord))) {
                    coordsAll.Add(coord);
                }
            }
        }
        return coordsAll;
    }
}
//MatchInfo 비교를 위한 Comparer
public class ListOfMatchInfoComparer : IEqualityComparer<MatchInfo> {
    public bool Equals(MatchInfo a, MatchInfo b) {
        return
            a.coords.SequenceEqual(b.coords);
    }

    public int GetHashCode(MatchInfo l) {
        unchecked {
            int hash = 0;
            foreach (var it in l.coords) {
                hash += (it.x * it.x) + (it.y * it.y) * 10;
            }
            return hash;
        }
    }
}
