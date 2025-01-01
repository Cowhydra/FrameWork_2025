using System;
using System.Collections.Generic;
using UnityEngine;
using D_F_Enum;
using UnityEngine.XR;

public interface IDlgBoxOwner
{
    public void OnDestoryed();
}


public partial class UIScene : MonoBehaviour,IDlgBoxOwner
{
    private class DlgBoxNode
    {
        public E_DLG_ID DlgBoxID;
        public UIDlgBox DlgBoxInst;
        public object DlgBoxVal;
        public Action<UIDlgBox, int> OnClose;

        public void Open(IDlgBoxOwner owner)
        {
            if (DlgBoxInst != null || DlgBoxInst.IsOpened == false)
            {
                DlgBoxInst.Open(owner, DlgBoxID, DlgBoxVal, OnClose);
            }
        }


        public void ReOpen()
        {
            if (DlgBoxInst != null)
            {
                DlgBoxInst.Reopen();
            }
        }

        //루트노드에서  해당 노드 뺴고 전부 숨기는 용도
        public void Hide()
        {
            if (DlgBoxInst != null)
            {
                DlgBoxInst.Hide();
            }
        }

        // 자식 노드와 부모를 모두 정리하는 Clear 함수
        public void Clear()
        {
            DlgBoxInst?.Close(0);
            DlgBoxInst = null;
        }

        public override bool Equals(object obj)
        {
            // obj가 DlgBoxNode로 캐스팅 가능한지 확인
            if (obj is DlgBoxNode otherNode)
            {
                return this.DlgBoxID == otherNode.DlgBoxID; // DlgBoxID 값이 같으면 동일 객체로 취급
            }

            return false;
        }

        public override int GetHashCode()
        {
            return DlgBoxID.GetHashCode(); // DlgBoxID의 해시 코드 값 사용
        }
    }

    private DlgBoxNode _CurrentNode;   // 현재 활성화된 노드

    private Stack<DlgBoxNode> _NodeStack= new Stack<DlgBoxNode>();

    // 현재 대화상자 얻기
    public UIDlgBox CurrentDlgBox
    {
        get { return (_CurrentNode != null ? _CurrentNode.DlgBoxInst : null); }
    }


    // 대화상자 열기
    public UIDlgBox OpenDlgBox(E_DLG_ID dlgBoxID, object dlgBoxVal, E_DLG_OPT opt = E_DLG_OPT.ShowCurrent, Action<UIDlgBox, int> onClose = null)
    {
        // 현재 Dlg과 같은 ID를 가진 Dlg를 연속해서 못 열게 함 (실수 방지)
        if (_CurrentNode != null && _CurrentNode.DlgBoxInst != null && _CurrentNode.DlgBoxID == dlgBoxID)
        { 
            return _CurrentNode.DlgBoxInst;
        }

        //로드
        UIDlgBox dlgBoxInst = AssetServer.Load<UIDlgBox>(dlgBoxID.ToString());
        if (dlgBoxInst == null)
        {
            Debug.LogError($"TSUIScene.OpenDlg> Failed to Load prefab ({dlgBoxID})!!");
            return null;
        }

        //노드생성
        DlgBoxNode curNode = CreateNewNode(dlgBoxID, dlgBoxInst, dlgBoxVal, onClose);

        switch (opt)
        {
            case E_DLG_OPT.DestoryAll:
                DestoryAllStack();
                break;
            case E_DLG_OPT.DestroyCurrent:
                DestoryStack();
                break;
            case E_DLG_OPT.ShowCurrent:
                break;
            case E_DLG_OPT.HideCurrent:
                HideNode();
                break;
        }

        AddNewNode(curNode);

        return dlgBoxInst;
    }


    private DlgBoxNode CreateNewNode(E_DLG_ID dlgID, UIDlgBox dlgBoxInst, object dlgVal, Action<UIDlgBox, int> onClose)
    {
        DlgBoxNode newNode = new DlgBoxNode
        {
            DlgBoxID = dlgID,
            DlgBoxInst = dlgBoxInst,
            DlgBoxVal = dlgVal,
            OnClose = onClose,
        };

        return newNode;
    }


    private void DestoryAllStack()
    {
        while (_NodeStack.Count > 0)
        {
            DestoryStack();
        }
    }


    private void DestoryStack()
    {
        if (_NodeStack.Count == 0)
        {
            return;
        }

        DlgBoxNode node = _NodeStack.Pop();

        if (node != null)
        {
            node.Clear();
        }
    }


    private void AddNewNode(DlgBoxNode node)
    {
        if (node != null)
        {
            _CurrentNode = node;
            _NodeStack.Push(node);
            _CurrentNode.Open(this);
        }
    }


    private void PopNode()
    {
        if (_NodeStack.Count == 0)
        {
            _CurrentNode.Clear();
            _CurrentNode = null;
        }
        else
        {
            _CurrentNode.Clear();
            _CurrentNode = _NodeStack.Pop();
            _CurrentNode.ReOpen();
        }
    }


    private void HideNode()
    {
        if (_CurrentNode != null)
        {
            _CurrentNode.Hide();
        }
    }


    public void OnDestoryed()
    {
        PopNode();
    }
}
