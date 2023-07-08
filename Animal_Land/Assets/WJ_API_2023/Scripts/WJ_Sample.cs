using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WjChallenge;

public enum CurrentStatus { WAITING, DIAGNOSIS, LEARNING }
public class WJ_Sample : MonoBehaviour
{
    [SerializeField] WJ_Connector wj_conn;
    [SerializeField] CurrentStatus currentStatus;
    public CurrentStatus CurrentStatus => currentStatus; // ������Ƽ

    [Header("Panels")]
    [SerializeField] GameObject panel_diag_chooseDiff;  //���̵� ���� �г�
    [SerializeField] GameObject panel_question;         //���� �г�(����,�н�)

    [SerializeField] Text textDescription;        //���� ���� �ؽ�Ʈ
    [SerializeField] TEXDraw textEquation;           //���� �ؽ�Ʈ(TextDraw�� ���� ����)
    [SerializeField] Button[] btAnsr = new Button[4]; //���� ��ư��
    TEXDraw[] textAnsr;                  //���� ��ư�� �ؽ�Ʈ(TextDraw�� ���� ����)

    [Header("Status")]
    int currentQuestionIndex;
    bool isSolvingQuestion;
    float questionSolveTime;

    [Header("For Debug")]
    [SerializeField] WJ_DisplayText wj_displayText;         //�ؽ�Ʈ ǥ�ÿ�(�ʼ�X)
    [SerializeField] Button getLearningButton;      //���� �޾ƿ��� ��ư

    [Header("Test")]
    [SerializeField] Button StartButton;
    [SerializeField] Text QuestionTimer;
    [SerializeField] Text CorrectAnswerCount;
    [SerializeField] Slider TimerSlider;
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

        wj_displayText.SetState("�����", "", "", "");
        _correctAnswerRemind = 0;
        _diagnosisIndex = 0;
        _correctAnswers = 0;
        _timerCoroutine = null;
    }


    private void OnEnable()
    {
        //Setup();
    }

    private void Setup()
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
            case CurrentStatus.WAITING:
                panel_diag_chooseDiff.SetActive(true);
                if (wj_conn != null)
                {
                    wj_conn.onGetDiagnosis.AddListener(() => GetDiagnosis());
                    wj_conn.onGetLearning.AddListener(() => GetLearning(0));
                }
                break;
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

        StartButton.gameObject.SetActive(false); // ���ӽ��� ��ư ��Ȱ��ȭ
    }

    private void Update()
    {
        if (isSolvingQuestion) // ���� Ǯ�� ���϶� �ð� ���
        {
            questionSolveTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// ������ ���� �޾ƿ���
    /// </summary>
    private void GetDiagnosis()
    {
        switch (wj_conn.cDiagnotics.data.prgsCd)
        {
            case "W":
                MakeQuestion(wj_conn.cDiagnotics.data.textCn,
                       wj_conn.cDiagnotics.data.qstCn,
                       wj_conn.cDiagnotics.data.qstCransr,
                       wj_conn.cDiagnotics.data.qstWransr);
                _diagnosisIndex++;
                if (_timerCoroutine == null) // Ÿ�̸� �ڷ�ƾ�� null�̰� ���� ���� �ƴ� �� 
                {
                    _timerCoroutine = SolveQuestionTimer();
#if UNITY_EDITOR
                    Debug.LogError("Ÿ�̸� �ڷ�ƾ ����");
#endif
                  //  if(!_timerCoroutine.MoveNext())
                    //{
                    //}
                       StartCoroutine(_timerCoroutine);
                }
                break;
            case "E":
                Debug.Log("������ ��! �н� �ܰ�� �Ѿ�ϴ�.");
                wj_displayText.SetState("������ �Ϸ�", "", "", "");
                currentStatus = CurrentStatus.LEARNING;
                panel_question.SetActive(false); // ���ο� ������ �ޱ� ���ؼ� ��Ȱ��ȭ
                getLearningButton.gameObject.SetActive(true); // ������ ������ ���� Ǯ�� ��ư Ȱ��ȭ
                getLearningButton.interactable = true;
                _correctAnswers = 0;
                break;
        }
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

        if (panel_diag_chooseDiff.activeSelf)
        {
            panel_diag_chooseDiff.SetActive(false);
        }

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
                case CurrentStatus.DIAGNOSIS:

                    isSolvingQuestion = false;

                    wj_conn.Diagnosis_SelectAnswer("-1", "N", (int)(questionSolveTime * 1000));

                    wj_displayText.SetState("������ ��", "-1", "N", questionSolveTime + " ��");

                    questionSolveTime = 0;
                    break;

                case CurrentStatus.LEARNING:

                    isSolvingQuestion = false;
                    currentQuestionIndex++;

                    wj_conn.Learning_SelectAnswer(currentQuestionIndex, "-1", "N", (int)(questionSolveTime * 1000));

                    wj_displayText.SetState("����Ǯ�� ��", "-1", "N", questionSolveTime + " ��");

                    if (currentQuestionIndex >= 8)
                    {
                        panel_question.SetActive(false);
                        wj_displayText.SetState("����Ǯ�� �Ϸ�", "", "", "");
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
            case CurrentStatus.DIAGNOSIS:
                isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cDiagnotics.data.qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;

                wj_conn.Diagnosis_SelectAnswer(textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("������ ��", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " ��");

                //panel_question.SetActive(false);  // ���� ������ �����Ÿ�

                questionSolveTime = 0;
                break;

            case CurrentStatus.LEARNING:
                isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;
                currentQuestionIndex++;

                wj_conn.Learning_SelectAnswer(currentQuestionIndex, textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("����Ǯ�� ��", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " ��");

                if (currentQuestionIndex >= 8)
                {
                    panel_question.SetActive(false);
                    wj_displayText.SetState("����Ǯ�� �Ϸ�", "", "", "");

                    if(_timerCoroutine != null)
                    {
                        StopCoroutine(_timerCoroutine);
                        _timerCoroutine = null;
                    }

                    getLearningButton.gameObject.SetActive(true); // ������ ������ ���� Ǯ�� ��ư Ȱ��ȭ
                    getLearningButton.interactable = true;
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
        CorrectAnswerCount.text = $"���� ���� �� : {_correctAnswers}";

    }

    public IEnumerator SolveQuestionTimer() // ����Ǯ�� Ÿ�̸�
    {

        float elapsedTime = SOLVE_TIME;
        while ((int)elapsedTime >= 0)
        {
            if (elapsedTime <= 0)
            {
                elapsedTime = 0;
            }
            QuestionTimer.text = $"���� �ð� : {(int)elapsedTime} ��";
            TimerSlider.value = elapsedTime;//�����̴� �� ����
            elapsedTime -= Time.deltaTime;
#if UNITY_EDITOR
            Debug.LogError(elapsedTime);
#endif
            yield return null;// new WaitForSeconds(1f);
        }
        //SelectAnswer();
    }

    public void UpdateTimerSlider(float elapsedTime)
    {
        TimerSlider.value = elapsedTime;
    }

    void SetupQuestion(string textCn, string qstCn, string qstCransr, string qstWransr) // ���� ���� �Լ�
    {
        string correctAnswer;
        string[] wrongAnswers;

        textDescription.text = textCn;
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

    void SetLearning()
    {
        currentStatus = CurrentStatus.LEARNING;
        panel_question.SetActive(false); // ���ο� ������ �ޱ� ���ؼ� ��Ȱ��ȭ
        getLearningButton.gameObject.SetActive(true); // ������ ������ ���� Ǯ�� ��ư Ȱ��ȭ
        getLearningButton.interactable = true;
        _correctAnswers = 0;
    }

    public void DisplayCurrentState(string state, string myAnswer, string isCorrect, string svTime)
    {
        if (wj_displayText == null) return;

        wj_displayText.SetState(state, myAnswer, isCorrect, svTime);
    }

    #region Unity ButtonEvent
    public void ButtonEvent_ChooseDifficulty(int a) // ������ �� ���̵� ���� ��ư
    {
        if (wj_conn._needDiagnosis)
        {
            currentStatus = CurrentStatus.DIAGNOSIS; // ���� ���� �������� ����
            wj_conn.FirstRun_Diagnosis(a); // ���̵� ���� 
        }
        else
        {
            SetLearning();
            wj_conn.Learning_GetQuestion();
            wj_displayText.SetState("����Ǯ�� ��", "-", "-", "-");
            _correctAnswers = 0;
            CorrectAnswerCount.text = $"���� ���� �� : {_correctAnswers}";
        }
    }
    public void ButtonEvent_GetLearning() // ���� �� ����
    {
        SetLearning();

        wj_conn.Learning_GetQuestion();
        wj_displayText.SetState("����Ǯ�� ��", "-", "-", "-");
        _correctAnswers = 0;
        CorrectAnswerCount.text = $"���� ���� �� : {_correctAnswers}";

        if (_timerCoroutine == null)
        {
            _timerCoroutine = SolveQuestionTimer();
        }
        StartCoroutine(_timerCoroutine);

         getLearningButton.gameObject.SetActive(false); // ������ ������ ���� Ǯ�� ��ư Ȱ��ȭ
         getLearningButton.interactable = false;
    }
    public void ButtonEvent_GameStart() // �߰�
    {
        Setup(); // ���� ���� �� ����
    }
    #endregion
}
