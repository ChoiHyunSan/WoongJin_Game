using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatPurchasePopUp :PopUpUI, IStatusCheckPopUP
{
    [SerializeField] private Text noticeText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Button okayButton;


    bool clickedPurchase = false;
    

    void Start()
    {
        GameStartView gameStartView = ViewManager.GetView<GameStartView>();
        gameStartView.ClickPurcahseAction +=(()=> clickedPurchase);
        purchaseButton?.onClick.AddListener(() => OnPurchaseButton());
        okayButton?.onClick.AddListener(() => ViewManager.ShowLast());
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

    public void OnOkayButton(bool flag)
    {
        okayButton?.gameObject.SetActive(flag);
        purchaseButton?.gameObject.SetActive(!flag);
        closeButton?.gameObject.SetActive(!flag);
    }

}
