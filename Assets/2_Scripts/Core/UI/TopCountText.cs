using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TopCountText : MonoBehaviour
{
    private Text mText;
    // Start is called before the first frame update
    void Start()
    {
        mText=GetComponent<Text>();
    }

    void Update()
    {
        int topCount = BlockManager.instance.totalTopCount;
        mText.text = topCount.ToString();
    }
}
