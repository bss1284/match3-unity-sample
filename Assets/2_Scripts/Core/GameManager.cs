using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using BSS.Extension;
using System.Linq;
using Sirenix.OdinInspector;

/// <summary>
/// -Todo. 해야될것-
/// 1.Gravity 적용
/// 2.최초 Map 생성시 Match없는 맵 생성 
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [ReadOnly]
    [SerializeField]
    private Block selectBlock;
    [ReadOnly]
    [SerializeField]
    private List<MatchInfo> curMatchInfos = new List<MatchInfo>();
    [ReadOnly]
    [SerializeField]
    private bool isReady = true;


    private void Update() {
        if (!isReady) return;
        if (selectBlock != null) return;

        if (Input.GetMouseButtonDown(0)) {
            var mousePos = InputEx.GetMouseWorld();
            RaycastHit2D hitBlock = Physics2D.Raycast(mousePos, Vector2.zero);


            if (hitBlock.collider!=null && hitBlock.collider.gameObject.GetComponent<Block>() != null) {
                selectBlock = hitBlock.collider.gameObject.GetComponent<Block>();
                StartCoroutine(CoWaitDrag(selectBlock.transform.position));
            }
        }
    }

    IEnumerator CoWaitDrag(Vector3 initPos) {
        isReady = false;
        while (true) {
            if (Input.GetMouseButtonUp(0)) break;
            var mousePos = InputEx.GetMouseWorld();
            float distance = Vector3.Distance(initPos, mousePos);
            if (distance > Global.DRAG_DISTANCE) {
                //드래그 완료
                Direction dir=BoardUtil.GetDirection(initPos, mousePos);
                Block changeBlock = BlockManager.instance.GetNeighbor(selectBlock, dir);
                if (changeBlock == null) break;
                //블록 스왑
                yield return StartCoroutine(BlockManager.instance.CoSwapBlock(selectBlock, changeBlock));
                var selectInfos=MatchManager.instance.Check(selectBlock);
                var targetInfos=MatchManager.instance.Check(changeBlock);
                curMatchInfos = MatchUtil.Distinct(selectInfos.Union(targetInfos).ToList());
                //스왑 실패 (Undo)
                if (curMatchInfos.Count == 0) {
                    Debug.Log("Swap Fail");
                    yield return StartCoroutine(BlockManager.instance.CoUndoSwap());
                    break;
                }
                //중력 적용 후 맵 생성 (추가 Match가 없을때까지 반복)
                while (true) {
                    if (curMatchInfos.Count == 0) break;
                    BlockManager.instance.DestoryBlocks(MatchUtil.GetCoordsAll(curMatchInfos));
                    yield return StartCoroutine(BlockManager.instance.CoApplyGravityAndGenerateMap());
                    curMatchInfos = MatchManager.instance.CheckAll();
                }
                break;
            }
            yield return null;
        }
        if (BlockManager.instance.totalTopCount == 0) {
            Debug.Log("Win!");
        }

        //초기화
        selectBlock = null;
        isReady = true;
    }
}
