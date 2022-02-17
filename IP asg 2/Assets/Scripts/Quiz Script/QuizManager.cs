using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;

public class QuizManager : MonoBehaviour
{
    public AuthManager authMgr;
    public FirebaseManager firebaseMgr;
    DatabaseReference dbReference;
    FirebaseAuth auth;

    // variable settings
    public List<QuestionsAndAnswers> QnA;
    public GameObject[] options;
    public int currentQuestions;

    public TextMeshProUGUI QuestionTxt;
    public TextMeshProUGUI ScoreTxt;
    int totalQuestion = 0;
    public int score;
    public int questionsAnswered;

    public GameObject quizPanel;
    public GameObject GameOverPanel;
    public GameObject StartPanel;


    public bool isPlayerStatUpdated;

    public string username;
    public int correct;
    public int acc;

    public void Awake(){
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

    // counting question and generating steps
    private void Start()
    {
        totalQuestion = QnA.Count;
        StartPanel.SetActive(true);
        quizPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        isPlayerStatUpdated = false;
        generateQuestions();
    }

    // restart the game function
    public void restart()
    {
        //SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        StartPanel.SetActive(false);
        quizPanel.SetActive(true);
    }

    // when game ends, lead players to game over scene
    void GameOver()
    {
        quizPanel.SetActive(false);
        GameOverPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestion;

        //if (!isPlayerStatUpdated)
        //{
        //    UpdatePlayerStats(this.score, this.accuracy);
        //}
        isPlayerStatUpdated = true;
    }

    public void UpdatePlayerStats(int correct, int accuracy)
    {
        firebaseMgr.UpdatePlayerStats(authMgr.GetCurrentUser().UserId, correct, accuracy, authMgr.GetCurrentUserDisplayName());
    }

    // when players answer the question correctly
    public void Correct()
    {
        score += 1;
        questionsAnswered += 1;
        QnA.RemoveAt(currentQuestions);
        generateQuestions();

    }

    // when players answers the question wrongly
    public void wrong()
    {
        questionsAnswered += 1;
        QnA.RemoveAt(currentQuestions);
        generateQuestions();
    }

    // setting of answers for questions 
    void SetAnswers()
    {
        for ( int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestions].Answers[i];

            if (QnA[currentQuestions].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    // loading to mainmenu with the back to main menu button 
    public void MainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
    }

    // generating the questions 
    void generateQuestions()
    {
        if (QnA.Count > 0)
        {
        currentQuestions = Random.Range(0, QnA.Count);

        QuestionTxt.text = QnA[currentQuestions].Questions;

        SetAnswers();

        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
        
    }

    public string QuizManagerToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void CreateAccuracy()
    {
        acc = (score / questionsAnswered) * 100;
        
    }
    public async Task CreateNewPlayerStats(string uuid, string username, int correct, int accuracy)
    {
        //QuizManager quizManager = authMgr.GetCurrentUser().UserId;
        CreateAccuracy();
        QuizManager newQuizManager = new QuizManager(username, correct, accuracy);
        Debug.LogFormat("Player details : {0}", newQuizManager.PrintQuizManager());

        //root/player/$uuid
        await dbReference.Child("players/" + uuid).SetRawJsonValueAsync(newQuizManager.QuizManagerToJson());
        return;
    }

    public QuizManager( string username, int correct, int acc)
    {
        this.username = username;
        this.correct = correct;
        this.acc = acc;
    }

    public QuizManager()
    {

    }

    public string PrintQuizManager()
    {
        return string.Format("Username: {0} \n Score: {1} \n Acc: {2}",
             this.username, this.correct, this.acc);
    }
}
