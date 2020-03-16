using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using BSS.Extension;
using System;
using System.Linq;
using Sirenix.OdinInspector;

public class MatchManager : Singleton<MatchManager>
{
    public List<Block> blocks => BlockManager.instance.blocks;
    [ShowInInspector]
    public List<Direction[]> bunchDirections = new List<Direction[]>();

    private void Awake() {
        foreach (var it in Global.NEIGHBOR_DIRECTIONS) {
            Direction[] bunchDirArray = new Direction[3];
            for (int i = 0; i < 3; i++) {
                bunchDirArray[i] = (Direction)(((int)it + i) % Global.NEIGHBOR_DIRECTIONS.Length+ 1);
            }
            bunchDirections.Add(bunchDirArray);
        }
    }

    /// <summary>
    /// 해당 맵의 모든 블록의 매치를 검사합니다.
    /// </summary>
    /// <param name="block">기준 블록</param>
    /// <returns>매치결과 </returns>
    public List<MatchInfo> CheckAll() {
        var result=new List<MatchInfo>();
        foreach (var it in BlockManager.instance.blocks) {
            var blockResult=Check(it);
            if (blockResult != null) {
                result=result.Union(blockResult).ToList();
            }
        }
        result=MatchUtil.Distinct(result);
        return result;
    }


    /// <summary>
    /// 해당 블록의 모든 매치를 검사합니다.
    /// </summary>
    /// <param name="block">기준 블록</param>
    /// <returns>매치결과 </returns>
    public List<MatchInfo> Check(Block block) {
        var originType = block.type;
        if (Global.OBSTACLE_BLOCKS.Exists(x => x == originType)) return new List<MatchInfo>();//장애물 블록 검사안함
        var checkInfos=CheckStraightAll(block);
        var bunchInfos = CheckBunchAll(block);
        var matchInfos = checkInfos.Union(bunchInfos).Distinct(new ListOfMatchInfoComparer()).ToList();
        return matchInfos;
    }


    //직선 블록 검사

    /// <summary>
    /// 해당 블록의 모든 직선 검사를 합니다. (없을경우 빈 List)
    /// </summary>
    /// <param name="block">기준 블록</param>
    /// <returns>매치결과 </returns>
    private List<MatchInfo> CheckStraightAll(Block block) {
        var matchInfos = new List<MatchInfo>();
        BlockType originType = block.type;

        foreach (var it in Enum.GetValues(typeof(MatchDirection))) {
            MatchDirection matchDir = (MatchDirection)it;
            if (matchDir == MatchDirection.None) continue;
            var matchInfo=CheckStraight(block, matchDir);
            if (matchInfo!=null) {
                matchInfos.Add(matchInfo);
            }
        }
        return matchInfos;
    }

    /// <summary>
    /// 직선 검사를 해서 결과를 반환합니다. (없을경우 Null)
    /// </summary>
    /// <param name="block">기준 블록</param>
    /// <param name="matchDir">체크 방향</param>
    /// <returns>매치결과 </returns>
    private MatchInfo CheckStraight(Block block, MatchDirection matchDir) {
        var originType = block.type;
        var firstBlock = GetFirstBlock(block, matchDir);
        Direction dir = MatchUtil.LookAtEnd(matchDir);
        var result = CheckDir(firstBlock, dir);
        if (result.Count == 0) return null;
        var matchInfo=new MatchInfo(originType, MatchType.Straight, matchDir,result);
        return matchInfo;
    }


    /// <summary>
    /// 기준 블록부터 해당방향으로 3개 이상인지 확인합니다. (없을경우 빈 List)
    /// </summary>
    /// <param name="block">기준 블록</param>
    /// <param name="matchDir">방향</param>
    /// <returns>블록 List</returns>
    private List<Vector2Int> CheckDir(Block block,Direction dir) {
        if ((int)dir > 10) throw new Exception("파라미터의 유효범위가 아닙니다.");
        var result = new List<Vector2Int>();
        BlockType originType = block.type;
        int match = 1;
        var nextBlock = block;
        result.Add(block.coords);
        while (true) {
            nextBlock = BlockManager.instance.GetNeighbor(nextBlock, dir);
            if (nextBlock == null || nextBlock.type != originType) {
                break;
            }
            match++;
            result.Add(nextBlock.coords);
        }
        bool isMatch = (3<= match) ;
        if (!isMatch) {
            result.Clear();
        }
        return result;
    }

    /// <summary>
    /// 매치 검사를 시작할 첫번째 블록을 가져옵니다.
    /// </summary>
    /// <param name="block">기준 블록</param>
    /// <param name="matchDir">체크 방향</param>
    /// <returns>첫번째 블록</returns>
    private Block GetFirstBlock(Block block,MatchDirection matchDir) {
        Block firstBlock = block;
        Block nextBlock = block;
        Direction dir = MatchUtil.LookAtStart(matchDir);
        while (true) {
            nextBlock = BlockManager.instance.GetNeighbor(nextBlock, dir);
            if (nextBlock == null || nextBlock.type != block.type) {
                break;
            }
            firstBlock = nextBlock;
        }
        return firstBlock;
    }



    //모여있는 블록(Bunch) 검사

    /// <summary>
    /// 해당 블록의 모든 모여있는 블록 검사를 합니다. (없을경우 빈 List)
    /// </summary>
    /// <param name="block">기준 블록</param>
    /// <returns>매치결과 </returns>
    private List<MatchInfo> CheckBunchAll(Block block) {
        List<MatchInfo> matchInfos = new List<MatchInfo>();
        BlockType originType = block.type;

        var neighborBlocks = BlockManager.instance.GetNeighborAll(block);
        foreach (var checkBlock in neighborBlocks) {
            foreach (var it in bunchDirections) {
                var matchBlocks = CheckBunch(originType, checkBlock, it);
                if (matchBlocks.Count != 0) {
                    var matchInfo = new MatchInfo(originType,MatchType.Bunch, MatchDirection.None, matchBlocks);
                    matchInfos.Add(matchInfo);
                }
            }
        }
        return matchInfos;
    }

    private List<Vector2Int> CheckBunch(BlockType originType,Block block, Direction[] dirArray) {
        var matchBlocks = new List<Vector2Int>();
        if (block.type != originType) return matchBlocks;
        matchBlocks.Add(block.coords);
        var nextBlock=block;
        for (int i = 0; i < dirArray.Length; i++) {
            nextBlock = BlockManager.instance.GetNeighbor(block, dirArray[i]);
            if (nextBlock == null || nextBlock.type != originType) {
                matchBlocks.Clear();
                return matchBlocks;
            }
            matchBlocks.Add(nextBlock.coords);
        }
        return matchBlocks;
    }


    
}
