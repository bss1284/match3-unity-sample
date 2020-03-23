using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using BSS.Extension;
using System;

public class BoardManager : MonoBehaviour{
    private static BoardManager _instance;
    public static BoardManager instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<BoardManager>();
                if (_instance == null) {
                    var obj = new GameObject(typeof(BoardManager).Name);
                    _instance = obj.AddComponent<BoardManager>();
                }
            }
            return _instance;
        }
    }
    
    public Board boardPrefab;
    public List<Board> boards = new List<Board>();

    private bool[,] boardTable = new bool[Global.MAX_SIZE.x, Global.MAX_SIZE.y];

    void Awake() {
        InitializeBoardTable();
        InitializeTiles();
        if (Global.TOTAL_COUNT != boards.Count) throw new Exception("TotalCount 상수와 실제 보드 개수가 다릅니다.");
    }

    public bool IsEnable(Vector2Int coords) {
        return GetBoard(coords)!=null;
    }

    public Board NextBoard(Board board, Direction dir) {
        return boards.Find(x => x.coords == BoardUtil.GetNeighbor(board.coords, dir));
    }
    public Board GetBoard(Vector2Int coords) {
        return boards.Find(x => x.coords == coords);
    }
    public Board GetBoard(int x, int y) {
        return boards.Find(xx => xx.coords == new Vector2Int(x, y));
    }


    private void OnValidate() {
        ////사이즈 변경될시 새로운 배열 적용
        //if (Global.MAX_SIZE!=new Vector2Int(boardTable.GetLength(0),boardTable.GetLength(1)) ) {
        //    boardTable = new bool[Global.MAX_SIZE.x, Global.MAX_SIZE.y];
        //}
        ////Offset 적용
        //for (int i = 0; i < boardTable.GetLength(0); i++) {
        //    for (int j = 0; j < boardTable.GetLength(1); j++) {
        //        if (i % 2 != j % 2) continue;
        //        boardTable[i, j] = false;
        //    }
        //}
    }

    private void InitializeBoardTable() {
        for(int x = 0; x < Global.MAX_SIZE.x; x++) {
            for(int y = 0; y < Global.MAX_SIZE.y; y++) {
                if((x + y) % 2 == 0) {
                    continue;
                }
                if((x == 0 && y == 1) || (x == 0 && y == Global.MAX_SIZE.y-2) || (x == 1 && y == 0) || (x == 1 && y == Global.MAX_SIZE.y - 1))
                    continue;
                if((x == Global.MAX_SIZE.x-1 && y == 1) || (x == Global.MAX_SIZE.x - 1 && y == Global.MAX_SIZE.y - 2) || (x == Global.MAX_SIZE.x - 2 && y == 0) || (x == Global.MAX_SIZE.x - 2 && y == Global.MAX_SIZE.y - 1))
                    continue;
                boardTable[x, y] = true;
            }
        }
    }

    private void InitializeTiles() {
        for (int i = 0; i < Global.MAX_SIZE.x; i++) {
            for (int j = 0; j < Global.MAX_SIZE.y; j++) {
                if (!boardTable[i, j]) continue;
                var coord = new Vector2Int(i, j);
                var board = CreateBoard(coord);
                boards.Add(board);
            }
        }
    }
    private Board CreateBoard(Vector2Int coords) {
        var board = Instantiate(boardPrefab);
        board.coords = coords;
        board.transform.SetParent(transform, false);
        board.transform.position = BoardUtil.GetPosition(board.coords);
        return board;
    }


}
