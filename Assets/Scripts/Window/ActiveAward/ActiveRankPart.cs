﻿using UnityEngine;
using System.Collections;

public class ActiveRankPart: MonoBehaviour 
{
    [SerializeField]
    private GameObject MyGrid;
    [SerializeField]
    private GameObject ActiveAwardItemObj;
    [SerializeField]
    private UILabel myFightLabel;
    [SerializeField]
    private UILabel myFightRankLabel;
	// Use this for initialization
	void Start () 
    {
	
	}

    public void SetActiveAwardPart(int tabIndex, BetterList<ActiveAwardItemData> mList)
    {
        if (MyGrid.transform.childCount > 0)
        {
            return;
        }
        myFightLabel.text = CharacterRecorder.instance.Fight.ToString();
        //myFightRankLabel.text = CharacterRecorder.instance.FightRank.ToString();
        Debug.LogError(CharacterRecorder.instance.FightRank);
        string _RankNumberStr = "";
        switch (CharacterRecorder.instance.RankNumber)
        {
            case 0: _RankNumberStr = "未上榜"; break;
            default: _RankNumberStr = "排名:" + CharacterRecorder.instance.RankNumber.ToString(); break;
        }
        myFightRankLabel.text = _RankNumberStr;
        for (int i = 0; i < mList.size; i++)
        {
            GameObject go = NGUITools.AddChild(MyGrid, ActiveAwardItemObj);
            //go.name = mList[i].userId.ToString();
            go.GetComponent<ActiveAwardItem>().SetInfo(tabIndex, mList[i]);
        }
        MyGrid.GetComponent<UIGrid>().Reposition();
    }
    void ClearUIGrid()
    {
        for (int i = MyGrid.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(MyGrid.transform.GetChild(i));
        }
    }
}