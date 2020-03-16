using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using System;
using System.Linq;
using BSS.Extension;

public class BlockManager : Singleton<BlockManager>
{
    public int totalTopCount=> remainTopCount + blocks.FindAll(x => x.type == BlockType.Top).Count;
    public int remainTopCount=10;
    public List<Block> prefabs = new List<Block>();
    public List<Block> blocks = new List<Block>();

    private Block[] backUpBlock = new Block[2];

    private void Start() {
        while (true) {
            CreateRandomMap();
            var matchInfo=MatchManager.instance.CheckAll();
            if (matchInfo.Count == 0) break;
            ClearMap();
        }
    }

    public void CreateRandomMap() {
        Debug.Log("맵생성");
        remainTopCount = 10;
        foreach (var it in BoardManager.instance.boards) {
            CreateRandomBlock(it.coords);
        }
    }
    public void ClearMap() {
        Debug.Log("맵삭제");
        blocks.ConvertAll(x => x.gameObject).DestroyAll();
        blocks.Clear();
    }

    public bool ExistsBlock(Vector2Int coords) {
        return blocks.Exists(x => x.coords == coords);
    }
    public Block GetBlock(Vector2Int coords) {
        return blocks.Find(x => x.coords == coords);
    }
    public Block GetBlock(int x, int y) {
        return blocks.Find(xx => xx.coords == new Vector2Int(x, y));
    }
    public Block GetNeighbor(Vector2Int coords, Direction dir) {
        return blocks.Find(x => x.coords == BoardUtil.GetNeighbor(coords, dir));
    }
    public Block GetNeighbor(Block origin, Direction dir) {
        return blocks.Find(x=>x.coords==BoardUtil.GetNeighbor(origin.coords, dir));
    }
    public IEnumerable<Block> GetNeighborAll(Block origin) {
        return BoardUtil.GetNeighborAll(origin.coords).Select(x => GetBlock(x)).Where(x => x != null);
    }
    public void DestoryBlocks(List<Vector2Int> targetCoords) {
        Debug.Log($"Pang!");
        TryDestroyObstacle(targetCoords);

        for (int i = 0; i < targetCoords.Count; i++) {
            Vector2Int coords = targetCoords[i];
            var targetBlock = GetBlock(coords);
            if (targetBlock == null) continue;
            targetBlock.TryDestroy();
        }
    }
    



    public IEnumerator CoSwapBlock(Block blockA,Block blockB) {
        if (blockA.coords == blockB.coords) throw new Exception("같은 블록 지정 불가");
        Debug.Log($"Swap ({blockA.coords} , {blockB.coords})");
        backUpBlock[0] = blockA;
        backUpBlock[1] = blockB;
        //이동
        blockA.ToGoal(BoardUtil.GetPosition(blockB.coords), Global.BLOCK_SWAP_SPEED);
        blockB.ToGoal(BoardUtil.GetPosition(blockA.coords), Global.BLOCK_SWAP_SPEED);
        yield return new WaitUntil(() => !blockA.IsMoving() && !blockB.IsMoving());
        //실제 좌표변경
        Vector2Int temp = blockA.coords;
        blockA.coords = blockB.coords;
        blockB.coords = temp;
    }
    public IEnumerator CoUndoSwap() {
        if (backUpBlock[0] == null || backUpBlock[1] == null) yield break;
        Debug.Log("Swap Undo");
        yield return StartCoroutine(CoSwapBlock(backUpBlock[0], backUpBlock[1]));
        backUpBlock[0] = null;
        backUpBlock[1] = null;
    }

    public IEnumerator CoApplyGravityAndGenerateMap() {
        while (true) {//전체 중력적용
            var emptyBoards = BoardManager.instance.boards.FindAll(x => x.isEmpty());
            if (emptyBoards.Count == 0) break;
            bool isFillExists = false;

            foreach (var emptyBoard in emptyBoards) {
                var fillBlock = emptyBoard.FindFillBlock();
                if (fillBlock != null) {
                    isFillExists = true;
                    fillBlock.ChangeCoordsAndMove(emptyBoard.coords, Global.BLOCK_DROP_SPEED);
                }
            }
            if (!isFillExists) {
                break;
            }
        }
        while (true) {//블록 생성후 배치
            var spawnBlock=SpawnRandomBlock();
            if (blocks.Count == Global.TOTAL_COUNT) break;
            var emptyCoords=FindEmpty();
            spawnBlock.ChangeCoordsAndMove(emptyCoords, Global.BLOCK_DROP_SPEED);
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 스폰 위치에서 블록을 생성합니다.
    /// </summary>
    /// <param name="blockPrefab">복제할 프리팹</param>
    /// <param name="coords">좌표</param>
    private Block SpawnRandomBlock() {
        var randPrefab = prefabs[UnityEngine.Random.Range(0, prefabs.Count)];
        if (randPrefab.type == BlockType.Top) {
            if (remainTopCount > 0) {
                remainTopCount--;
            } else {
                return SpawnRandomBlock();
            }
        }

        Vector2Int spawnCoords = new Vector2Int(Global.MAX_SIZE.x / 2, Global.MAX_SIZE.y - 1);
        if (blocks.Exists(x => x.coords == spawnCoords)) throw new Exception("같은 위치에 블록이 있음");

        var block = Instantiate(randPrefab);
        block.coords = spawnCoords;
        block.transform.SetParent(transform, false);
        block.transform.position = BoardUtil.GetPosition(spawnCoords) + new Vector3(0f, 3f, 0f);
        block.ToGoal(BoardUtil.GetPosition(spawnCoords), Global.BLOCK_DROP_SPEED);
        blocks.Add(block);
        return block;
    }

    /// <summary>
    /// 해당 좌표에 블록을 즉시 생성합니다.
    /// </summary>
    /// <param name="blockPrefab">복제할 프리팹</param>
    /// <param name="coords">좌표</param>
    private Block CreateBlock(Block blockPrefab,Vector2Int coords) {
        if (!BoardManager.instance.IsEnable(coords)) throw new Exception("유효범위 초과");
        if (blocks.Exists(x=>x.coords == coords)) throw new Exception("같은 위치에 블록이 있음");

        var block = Instantiate(blockPrefab);
        block.coords = coords;
        block.transform.SetParent(transform, false);
        block.transform.position = BoardUtil.GetPosition(block.coords);
        blocks.Add(block);
        return block;
    }
    /// <summary>
    /// 해당 좌표에 블록을 랜덤으로 생성합니다.
    /// </summary>
    /// <param name="coords">좌표</param>
    private Block CreateRandomBlock(Vector2Int coords) {
        var randPrefab = prefabs[UnityEngine.Random.Range(0, prefabs.Count)];
        if (randPrefab.type == BlockType.Top) {
            if (remainTopCount > 0) {
                remainTopCount--;
            } else {
                return CreateRandomBlock(coords);
            }
        }
        return CreateBlock(randPrefab, coords);
    }

    /// <summary>
    /// 장애물을 제거합니다.
    /// </summary>
    private void TryDestroyObstacle(List<Vector2Int> targetCoords) {
        var obstacles = new List<IObstacle>();
        foreach (var targetCoord in targetCoords) {
            foreach (var coords in BoardUtil.GetNeighborAll(targetCoord)) {
                var block = GetBlock(coords);
                if (block != null && block is IObstacle) {
                    var obstacle = block as IObstacle;
                    if (!obstacles.Exists(x => x == obstacle)) {
                        obstacles.Add(obstacle);
                    }
                }
            }
        }
        foreach (var obstacle in obstacles) {
            (obstacle as Block).TryDestroy();
        }
    }

    /// <summary>
    /// 비어있는 블럭 좌표 찾기 (Y가 작은값이 우선순위,Y가 같을경우 X가 Center와 가까울수록 우선순위)
    /// </summary>
    /// <returns></returns>
    private Vector2Int FindEmpty() {
        var coordList = BoardManager.instance.boards.Where(x => GetBlock(x.coords) == null).Select(x => x.coords).ToList();
        coordList.Sort((c1, c2) => (10 * c1.y + (Mathf.Abs(Global.MAX_SIZE.x / 2) - c1.x)) - (10 * c2.y + (Mathf.Abs(Global.MAX_SIZE.x / 2) - c2.x)));
        return coordList[0];
    }
}
