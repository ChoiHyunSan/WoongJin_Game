using UnityEngine;
using UnityEngine.UI;

public class TitlePopUp : PopUpUI, IStatusCheckPopUP
{

    public override void Initialize()
    {
        closeButton.onClick.AddListener(() => this.gameObject.SetActive(false)); // ������ â  
        if (blocker != null)
        {
            closeButton.onClick.AddListener(() => blocker?.gameObject.SetActive(false)); // OnDisable�� ���� �� �׽�Ʈ �غ���
        }
    }
    [SerializeField] private Text popUPText;
    public void SetCheckMessage(string message)
    {
        popUPText.text = message;
    }
}
