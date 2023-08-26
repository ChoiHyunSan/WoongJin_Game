using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private static ViewManager instance;
    public static ViewManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ViewManager();
            return instance;
        }
    }

    [Header("���� ȭ��")]
    [SerializeField]
    private View startingView; // ���� ȭ��
    [Header("â ���")]
    [SerializeField]
    private List<View> views = new List<View>();

    private View _currentView; // ���� â

    private readonly Stack<View> _viewHistory = new Stack<View>(); // UI Stack

    private void Awake()
    {
        instance = this; // �̱��� �ν��Ͻ� 
    }

    private void Start()
    {
        foreach (var view in Instance.views) // ��ȸ�ϸ鼭 �ʱ�ȭ �� �����
        {
            view.Initialize();

            view.Hide();
        }

        if (startingView != null) // ������ �� ������ â
        {
            Show(startingView); // ó�� �����ϴ� ȭ���� ����ϱ�
        }
    }

    public static T GetView<T>() where T : View // View�� ��ӹްų� �ϴ� ��ü��� ����
    {
        foreach (var view in Instance.views)
        {
            if (view is T tView)
            {
                return tView;
            }
        }
#if UNITY_EDITOR
        Debug.LogWarning("UI ã�� �� ����");
#endif
        return null;
    }

    public static void Show<T>(bool remember = true, bool popUpUI = false) where T : View
    {
        foreach (var view in Instance.views)
        {
            if (view is T)
            {
                if (Instance._currentView != null) // ���� â�� null�� �ƴϸ�
                {
                    if (remember) // ����ؾ��ϴ� �Ŷ�� 
                    {
                        Instance._viewHistory.Push(Instance._currentView); // stack�� ����
                    }

                    if (!popUpUI) // popup�� �ƴϸ�
                    {
                        Instance._currentView.Hide(); // ���� â�� �����
                    }
                }

                view.Show(); // ���� ������ â ���̱�
                Instance._currentView = view; // ���� â���� ����
                return;
            }
        }
    }

    public static void Show(View view, bool remember = true)
    {
        if (Instance._currentView != null) // ���� �������� â�� null�̸� return
        {
            if (remember) // ����ؾ��ϴ� â�̶��
            {
                Instance._viewHistory.Push(Instance._currentView);
            }

            Instance._currentView.Hide(); // ���� â�� �����
        }

        view.Show(); // ���� �� â�� �����ش�

        Instance._currentView = view; // ���� â�� ���� â���� ����
    }

    public static void ShowLast() // ������ â(���� â) �����ֱ�
    {
        if (Instance._viewHistory.Count > 0)
        {
            Show(Instance._viewHistory.Pop(), false);
        }
    }
}
