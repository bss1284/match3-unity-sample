using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;

public class TopBlock : Block, IObstacle {
    public bool isSpin { get; private set; } = false;
    public TweenElement spinElement;


    public override void TryDestroy() {
        if (!isSpin) {
            isSpin = true;
            GetComponent<TweenPlayer>().Play(spinElement);
            return;
        }
        BlockManager.instance.blocks.Remove(this);
        Destroy(gameObject);
    }

}
