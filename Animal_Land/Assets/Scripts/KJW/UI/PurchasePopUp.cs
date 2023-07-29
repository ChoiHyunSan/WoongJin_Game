using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePopUp : PopUpUI
{
    [SerializeField]
    private List<Button> selectBtns;

    public override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < selectBtns.Count; i++)
        {
            int index = i; // ������ ĸ���ϱ� ������ i�� �ȳְ� ���÷� ���� ������ �Ҵ��ؼ� ���
            selectBtns[i].onClick.AddListener(() => OnSelectPurchase(index));
        }
    }

    void OnSelectPurchase(int index)
    {
        switch (index)
        {
            case 0: // ����
                // TODO : ��� Ȯ���ϱ� 

                PurchaseCheckPopUp popUpUI = ViewManager.GetView<PurchaseCheckPopUp>();
                if (popUpUI != null && popUpUI.name == "Selection_Result_PopUp") // �˾� â �̸��� ��������
                {
                    ViewManager.Show<PurchaseCheckPopUp>(true, true); // ���� Ȯ�� â
                }
                break;
            case 1: // ���
                ViewManager.ShowLast(); // ���� ȭ������ 
                blocker.gameObject?.SetActive(false); // ���Ŀ ��Ȱ��ȭ
                break;
        }
    }


}
