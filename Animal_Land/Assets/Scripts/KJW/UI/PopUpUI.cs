using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : View
{
    [SerializeField]
    protected Button closeButton;
    [SerializeField]
    protected Image blocker; // �ڿ� ȭ���� ��ġ�� �ȵǵ��� �̹��� Ȱ��ȭ

    public override void Initialize()
    {
        closeButton.onClick.AddListener(() => ViewManager.ShowLast()); // ������ â  
        closeButton.onClick.AddListener(() => blocker?.gameObject.SetActive(false)); // OnDisable�� ���� �� �׽�Ʈ �غ���
    }

    private void OnEnable()
    {
        if (blocker != null && !blocker.gameObject.activeSelf) // Ȱ��ȭ�� �ȵǾ� ���� ��
        {
            blocker.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (blocker != null) // �������� �Ǿ����� ��츦 ���ؼ� �߰�
        {
            blocker?.gameObject.SetActive(false);
        }
    }
}
