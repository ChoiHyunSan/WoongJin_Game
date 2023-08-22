using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LearningAPI : MonoBehaviour
{
    [SerializeField] WJ_Connector wj_conn;
    [SerializeField] CurrentStatus currentStatus;
    public CurrentStatus CurrentStatus => currentStatus; // ������Ƽ

    [Header("Panels")]
    [SerializeField] GameObject panel_question;         //���� �г�(����,�н�)

    [Header("Status")]
    int currentQuestionIndex;
    bool isSolvingQuestion;
    float questionSolveTime;

    [SerializeField] Text textDescription;        //���� ���� �ؽ�Ʈ
    [SerializeField] TEXDraw textEquation;           //���� �ؽ�Ʈ(TextDraw�� ���� ����)
    [SerializeField] Button[] btAnsr = new Button[4]; //���� ��ư��
    TEXDraw[] textAnsr;                  //���� ��ư�� �ؽ�Ʈ(TextDraw�� ���� ����)

    [SerializeField] SGameUIManager uIManager;

    private const int SOLVE_TIME = 15; // ���� Ǯ�� �ð�
    private int _correctAnswerRemind; // ���� �ε��� ����
    private int _diagnosisIndex; // ���� �ε���
    private int _correctAnswers; // ���� ���� �� 
    private IEnumerator _timerCoroutine;

    private void Awake()
    {
        textAnsr = new TEXDraw[btAnsr.Length]; // ��ư ���ڸ�ŭ �Ҵ� TextDraw�� ����
        for (int i = 0; i < btAnsr.Length; ++i) // textAnsr�� text�Ҵ� 

            textAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();

     
        _correctAnswerRemind = 0;
        _diagnosisIndex = 0;
        _correctAnswers = 0;
        _timerCoroutine = null;
    }

    private void Update()
    {
        if (isSolvingQuestion) // ���� Ǯ�� ���϶� �ð� ���
        {
            questionSolveTime += Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        Setup();
    }

    void Setup() // ���� �ʱ�ȭ
    {

        if (wj_conn != null && !wj_conn._needDiagnosis)
        {
            currentStatus = CurrentStatus.LEARNING;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("Cannot find Connector");
#endif
        }
        switch (currentStatus)
        {
           
            case CurrentStatus.LEARNING:
                {
                    if (wj_conn != null)
                    {
                        SetLearning();
                        wj_conn.onGetLearning.AddListener(() => GetLearning(0));
                    }
                }
                break;
        }
    }

    void SetLearning()
    {
        currentStatus = CurrentStatus.LEARNING;
     //   panel_question.SetActive(false); // ���ο� ������ �ޱ� ���ؼ� ��Ȱ��ȭ
        //getLearningButton.gameObject.SetActive(true); // ������ ������ ���� Ǯ�� ��ư Ȱ��ȭ
        //getLearningButton.interactable = true;
        _correctAnswers = 0;
    }


    /// <summary>
    ///  n ��° �н� ���� �޾ƿ���
    /// </summary>
    private void GetLearning(int _index)
    {
        if (_index == 0)
        {
            currentQuestionIndex = 0;
        }

        MakeQuestion(wj_conn.cLearnSet.data.qsts[_index].textCn,
                wj_conn.cLearnSet.data.qsts[_index].qstCn,
                wj_conn.cLearnSet.data.qsts[_index].qstCransr,
                wj_conn.cLearnSet.data.qsts[_index].qstWransr);


    }

    /// <summary>
    /// �޾ƿ� �����͸� ������ ������ ǥ��
    /// </summary>
    private void MakeQuestion(string textCn, string qstCn, string qstCransr, string qstWransr)
    {
        panel_question.SetActive(true);

        // ù��° ���� �����̰ų� ù��° �н������϶���
        bool isFirstQuestion = (_diagnosisIndex == 0 && currentStatus == CurrentStatus.DIAGNOSIS) ||
            (currentQuestionIndex == 0 && currentStatus == CurrentStatus.LEARNING);

        if (isFirstQuestion)
        {
            SetupQuestion(textCn, qstCn, qstCransr, qstWransr);
            _diagnosisIndex++;
        }
        else
        {
            StartCoroutine(ColoringCorrectAnswer(textCn, qstCn, qstCransr, qstWransr, 0.5f));
            _diagnosisIndex++;
        }

    }


    /// <summary>
    /// ���� ���� �¾Ҵ� �� üũ
    /// </summary>
    public void SelectAnswer(int _idx = -1)
    {
        if (_idx == -1) // �ð��ʰ� ��
        {
            switch (currentStatus)
            {
                case CurrentStatus.LEARNING:

                    isSolvingQuestion = false;
                    currentQuestionIndex++;

                    wj_conn.Learning_SelectAnswer(currentQuestionIndex, "-1", "N", (int)(questionSolveTime * 1000));

                    if (currentQuestionIndex >= 2) // ���� ����
                    {
                        //  panel_question.SetActive(false);

                        uIManager.CloseSolveScreen();
                    }
                    else
                    {
                        GetLearning(currentQuestionIndex);
                    }
                    questionSolveTime = 0;
                    break;
            }
            return;
        }

        bool isCorrect = false;
        string ansrCwYn = "N";
        // StopAllCoroutines();

        switch (currentStatus)
        {
            case CurrentStatus.LEARNING:
                isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;
                currentQuestionIndex++;

                wj_conn.Learning_SelectAnswer(currentQuestionIndex, textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                if (currentQuestionIndex >= 2) // ���� ����
                {
                   // panel_question.SetActive(false);
                    uIManager.CloseSolveScreen();

                    if (_timerCoroutine != null)
                    {
                        StopCoroutine(_timerCoroutine);
                        _timerCoroutine = null;
                    }

                    //getLearningButton.gameObject.SetActive(true); // ������ ������ ���� Ǯ�� ��ư Ȱ��ȭ
                    //getLearningButton.interactable = true;
                }
                else
                {
                    GetLearning(currentQuestionIndex);
                }
                questionSolveTime = 0;
                break;
        }

        if (isCorrect)
        {
            _correctAnswers += 1;
        }
    }


    void SetupQuestion(string textCn, string qstCn, string qstCransr, string qstWransr) // ���� ���� �Լ�
    {
        string correctAnswer;
        string[] wrongAnswers;

      //  textDescription.text = textCn;
        textEquation.text = qstCn;

        correctAnswer = qstCransr;
        wrongAnswers = qstWransr.Split(',');

        int ansrCount = Mathf.Clamp(wrongAnswers.Length, 0, 3) + 1;

        for (int i = 0; i < btAnsr.Length; i++)
        {
            if (i < ansrCount)
                btAnsr[i].gameObject.SetActive(true);
            else
                btAnsr[i].gameObject.SetActive(false);
        }

        int ansrIndex = Random.Range(0, ansrCount);
        _correctAnswerRemind = ansrIndex; // ���� �ε��� �����صα�

        for (int i = 0, q = 0; i < ansrCount; ++i, ++q)
        {
            if (i == ansrIndex)
            {
                textAnsr[i].text = correctAnswer;
                --q;
            }
            else
                textAnsr[i].text = wrongAnswers[q];
        }
        isSolvingQuestion = true;
    }

    IEnumerator ColoringCorrectAnswer(string textCn, string qstCn, string qstCransr, string qstWransr, float delay) // ���� ���� �� ���� ǥ��
    {
        int prevIndex = _correctAnswerRemind; // ���� �ε��� ����
        textAnsr[_correctAnswerRemind].color = new Color(1.0f, 0.0f, 0.0f); // ���� �ε��� ���� ����

        yield return new WaitForSeconds(delay); // ������

        textAnsr[prevIndex].color = new Color(0.0f, 0.0f, 0.0f); // ���� ���� �ε��� �ٽ� ���� �ǵ�����
        SetupQuestion(textCn, qstCn, qstCransr, qstWransr);
    }


    public void ButtonEvent_GetLearning()
    {
        SetLearning();

        wj_conn.Learning_GetQuestion();
        _correctAnswers = 0;

    }


}
