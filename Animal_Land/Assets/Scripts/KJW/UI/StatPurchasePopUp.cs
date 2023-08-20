using UnityEngine;
using UnityEngine.UI;

public class StatPurchasePopUp :PopUpUI, IStatusCheckPopUP
{


    [SerializeField] private Text noticeText;
    [SerializeField] private Button purchaseButton;

    bool clickedPurchase = false;
    

    void Start()
    {
        ViewManager.GetView<GameStartView>().ClickPurcahseAction +=(()=> clickedPurchase);
        purchaseButton.onClick.AddListener(() => OnPurchaseButton());
    }

    public void SetCheckMessage(string message)
    {
        noticeText.text = message;
    }

    public void OnPurchaseButton()
    {
        ViewManager.ShowLast(); // ���� Ȯ�� â ����
        clickedPurchase = true; // ���� ��ư ������
    }

    protected override void OnDisable()
    {
        base.OnDisable(); 
        clickedPurchase = false; // ���� â�� ���� ������ �ٽ� false�� ����
    }
}
