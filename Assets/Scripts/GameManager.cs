using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  private enum GameState { MainMenu, Tutorial, Game, Paused, DayEnd, GameOver }
  private GameState gameState;

  public SoundManager soundManager;

  // Main UI Panels
  public GameObject mainMenuUI;
  public GameObject gameUI;
  public GameObject pauseMenuUI;
  public GameObject dayEndUI;
  public GameObject gameOverUI;

  //Main Menu
  public TextMeshProUGUI mainMenuHighscoreText;
  public TextMeshProUGUI mainMenuHighScoreNameText;
  public TextMeshProUGUI mainMenuHighScoreScoreText;

  // Main Game UI
  public GameObject tutorial;
  private bool rulesRead = false;
  public TextMeshProUGUI rulesDate;
  public TextMeshProUGUI rule1;
  public TextMeshProUGUI rule2;
  public TextMeshProUGUI rule3;
  public TextMeshProUGUI rulesButtonText;

  public TextMeshProUGUI PCTimeLeft;
  public TextMeshProUGUI PCName;
  public TextMeshProUGUI PCBirthDate;
  public TextMeshProUGUI PCGender;
  public TextMeshProUGUI PCProvenance;
  public TextMeshProUGUI PCExpirationDate;

  public Image candidateSilouette;

  public GameObject ingameDebug;
  private bool debugActive = false;

  // End Day UI
  public TextMeshProUGUI candidatesText;
  public TextMeshProUGUI accuracyText;
  public GameObject clip;
  public TextMeshProUGUI companyConfidence;
  public GameObject endGameNextDayButton;
  public GameObject endGameRevisionButton;
  public GameObject[] clipPlaceholders = new GameObject[10];

  // End Game UI
  public TextMeshProUGUI daysSurvivedEndGame;
  public TextMeshProUGUI candidatesReviewedEndGame;

  // Visible variables
  public float timePerDay = 10f;

  // Variables per run
  private int daysPassed = 0;
  private float dayTimeLeft = 10.0f;
  private int currentTrust = 5;

  private int totalSuccesses = 0;
  private int totalFails = 0;

  private DateTime currentDate;

  private int highscore = 0;

  // Variables per round
  private int roundSuccesses = 0;
  private int roundFails = 0;
  //private float candidateTimeLeft = 20.0f;

  // Debug
  public TextMeshProUGUI timeLeftText;
  public TextMeshProUGUI daysPassedText;
  public TextMeshProUGUI currentSuccessesText;
  public TextMeshProUGUI currentFailsText;

  //candidate factory
  CandidateFactory candidaFactory;
  //current candidate
  Candidate currentCandidate;
  //current rules
  RuleSet currentRules;

  // Score data access
  private IScoreDAO scoreDAO;

  private static readonly string SCORE_API_URL = "https://api.jazbelt.net/high_scores";

  private void Awake()
  {

    gameState = GameState.MainMenu;

    // Reset positions
    (mainMenuUI.transform as RectTransform).anchoredPosition = Vector2.zero;
    (gameUI.transform as RectTransform).anchoredPosition = Vector2.zero;
    (pauseMenuUI.transform as RectTransform).anchoredPosition = Vector2.zero;
    (dayEndUI.transform as RectTransform).anchoredPosition = Vector2.zero;
    (gameOverUI.transform as RectTransform).anchoredPosition = Vector2.zero;

    // Deactivate elements
    gameUI.SetActive(false);
    pauseMenuUI.SetActive(false);
    dayEndUI.SetActive(false);
    gameOverUI.SetActive(false);

    soundManager.PlayGameMusic();

  }

  void Start()
  {
    currentDate = DateTime.Today;
    soundManager.PlayGameMusic();

    scoreDAO = new ScoreDAO.ScoreDAOBuilder()
      .ApiUrl(SCORE_API_URL)
      .Build();

    StartCoroutine(GetHighScore());
  }

  void Update()
  {

    if (gameState == GameState.Game)
    {
      //candidateTimeLeft -= Time.deltaTime;
      if (rulesRead)
      {
        dayTimeLeft -= Time.deltaTime;
        dayTimeLeft = Mathf.Round(dayTimeLeft * 100f) * 0.01f;
        timeLeftText.text = "Day time left: " + dayTimeLeft;
        int intValue = Mathf.RoundToInt(dayTimeLeft); // Round to nearest integer
        string intString = intValue.ToString(); // Convert integer to string
        PCTimeLeft.text = "TIME: " + intString; // Set UI text to integer string
        if (dayTimeLeft <= 0.1f)
        {
          EndDay();
        }
      }

      if (Input.GetKeyDown("escape")) { PauseGame(); }

      if (Input.GetKeyDown(KeyCode.O)) { EndDay(); }

      if (Input.GetKeyDown(KeyCode.P)) { GameOver(); }

      if (Input.GetKeyDown(KeyCode.K)) { ToogleShowDebugInfo(); }

      if (Input.GetKeyDown(KeyCode.L)) { GetNewRules(); }
    }
  }

  public void StartNewGame()
  {
    soundManager.StopMusic();
    if (gameState == GameState.MainMenu && highscore == 0)
    {
      tutorial.SetActive(true);
      gameState = GameState.Tutorial;
    }
    else
    {
      soundManager.StartBackgroundNoise();
      tutorial.SetActive(false);
      gameState = GameState.Game;

      soundManager.StopMusic();
      soundManager.StartBackgroundNoise();

      mainMenuUI.SetActive(false);
      gameUI.SetActive(true);

      dayTimeLeft = timePerDay;

      GetNewRules();
      GetNewCandidate();
    }
  }

  public void PauseGame()
  {
    gameState = GameState.Paused;

    gameUI.SetActive(false);
    pauseMenuUI.SetActive(true);
  }

  public void ResumeGame()
  {
    gameState = GameState.Game;

    pauseMenuUI.SetActive(false);
    gameUI.SetActive(true);
  }

  public void EndDay()
  {
    soundManager.StopBackgroundNoise();
    gameState = GameState.DayEnd;

    // TO DO CALCULATE TRUST!!!!!!
    currentTrust += ((roundSuccesses - roundFails) - (daysPassed * 2));
    if (currentTrust <= 0)
    {
      currentTrust = 0;
      endGameNextDayButton.SetActive(false);
      endGameRevisionButton.SetActive(true);
      soundManager.PlayEndDayNegativeMusic();
    }
    else
    {
      endGameNextDayButton.SetActive(true);
      endGameRevisionButton.SetActive(false);
      soundManager.PlayEndDayPositiveMusic();
    }
    if (currentTrust >= 10) { currentTrust = 9; }
    int candidatesReviewed = Mathf.Abs(roundSuccesses) + Mathf.Abs(roundFails);
    candidatesText.text = "Candidates reviewed: " + candidatesReviewed;

    // calculate the accuracy rate as a percentage
    float accuracyRate = ((float)roundSuccesses / candidatesReviewed) * 100f;
    accuracyText.text = "Accuracy rate: " + accuracyRate.ToString("F2") + "%";

    (clip.transform as RectTransform).anchoredPosition = (clipPlaceholders[currentTrust].gameObject.transform as RectTransform).anchoredPosition;

    companyConfidence.text = "ROLE CONFIDENCE: " + currentTrust;

    gameUI.SetActive(false);
    dayEndUI.SetActive(true);
  }

  public void NextDay()
  {
    soundManager.StopMusic();
    soundManager.StartBackgroundNoise();
    daysPassed += 1;
    daysPassedText.text = "Days passed: " + daysPassed;
    currentDate = currentDate.AddDays(1);

    GetNewRules();

    rulesRead = false;

    totalSuccesses += roundSuccesses;
    roundSuccesses = 0;
    currentSuccessesText.text = "Round successes: " + roundSuccesses;

    totalFails += roundFails;
    roundFails = 0;
    currentFailsText.text = "Round fails: " + roundFails;

    dayTimeLeft = timePerDay;

    gameState = GameState.Game;

    dayEndUI.SetActive(false);
    gameUI.SetActive(true);
  }

  public void GameOver()
  {
    soundManager.StopBackgroundNoise();
    soundManager.PlayEndGameMusic();
    gameState = GameState.GameOver;

    soundManager.PlayEndGameMusic();

    soundManager.StopBackgroundNoise();

    daysSurvivedEndGame.text = "DAYS SURVIVED: " + daysPassed;
    candidatesReviewedEndGame.text = "CANDIDATES REVIEWED: " + (totalSuccesses + totalFails);

    gameUI.SetActive(false);
    dayEndUI.SetActive(false);
    gameOverUI.SetActive(true);
  }

  public void ExitToMainMenu()
  {

    gameState = GameState.MainMenu;
    soundManager.PlayGameMusic();

    gameUI.SetActive(false);
    pauseMenuUI.SetActive(false);
    dayEndUI.SetActive(false);
    gameOverUI.SetActive(false);

    mainMenuUI.SetActive(true);

    daysPassed = 0;
    daysPassedText.text = "Days passed: " + daysPassed;
    dayTimeLeft = 10.0f;
    currentTrust = 5;

    /*if (highscore < daysPassed)
    {
      highscore = daysPassed;
      mainMenuHighscoreText.gameObject.SetActive(true);
      mainMenuHighscoreText.text = "HIGHSCORE: " + highscore;
    }*/


    totalSuccesses = 0;
    totalFails = 0;

    roundSuccesses = 0;
    currentSuccessesText.text = "Round successes: " + roundSuccesses;

    roundFails = 0;
    currentFailsText.text = "Round fails: " + roundFails;
    int candidatesReviewed = Mathf.Abs(roundSuccesses) + Mathf.Abs(roundFails);

    candidatesText.text = "Candidates reviewed: " + candidatesReviewed;
    accuracyText.text = "Boss' confidence: " + currentTrust;

    soundManager.PlayGameMusic();
  }



  public void ValidateCandidate(bool accepted)
  {
    if (!rulesRead) return;

    if (accepted)
    {
      soundManager.Pass();
    }
    else
    {
      soundManager.Deny();
    }

    ValidationResult validationresult = currentRules.Validate(currentCandidate, currentDate, (daysPassed + 1));
    // TO DO LOGIC HERE
    bool success = validationresult == ValidationResult.VALID ? accepted : !accepted;


    if (success)
    {
      roundSuccesses++;
      currentSuccessesText.text = "Successes: " + roundSuccesses;
      StartCoroutine(PlayResultSound(true));
    }
    else
    {
      // TO DO
      switch (validationresult)
      {
        case ValidationResult.ID_EXPIRED:
          Debug.Log("ID EXPIRED");
          break;

        case ValidationResult.AGE_NOT_MATCH:
          Debug.Log("AGE DON'T MATCH");
          break;

        case ValidationResult.GENDER_NOT_MATCH:
          Debug.Log("GENDER DON'T MATCH");
          break;

        case ValidationResult.ORIGIN_NOT_MATCH:
          Debug.Log("ORIGIN DON'T MATCH");
          break;

      }
      roundFails++;
      currentFailsText.text = "Fails: " + roundFails;
      StartCoroutine(PlayResultSound(false));
    }
    GetNewCandidate();
  }

  private void ToogleShowDebugInfo()
  {
    debugActive = !debugActive;
    ingameDebug.SetActive(debugActive);
  }

  private void GetNewCandidate()
  {
    candidaFactory = CandidateFactory.GetInstance();
    currentCandidate = candidaFactory.GetCandidate(DateTime.Now);
    PCName.text = currentCandidate.name.ToString();
    PCBirthDate.text = currentCandidate.dob.Date.ToString("dd/MM/yyyy");
    PCGender.text = currentCandidate.gender.ToString();
    PCProvenance.text = currentCandidate.origin.ToString();
    PCExpirationDate.text = currentCandidate.expiration.Date.ToString("dd/MM/yyyy");

    // create a random color
    Color randomColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

    // tint the image to the random color
    candidateSilouette.color = randomColor;
  }

  public void CheckRulesRead()
  {
    if (rulesRead == false) rulesRead = true;
    if (rulesButtonText.text == "GO!")
    {
      rulesButtonText.text = "RULES";
      soundManager.RulesHide();
    }
    else
    {
      rulesButtonText.text = "GO!";
      soundManager.ShowRules();
    }

  }

  public void GetNewRules()
  {
    rulesDate.text = currentDate.ToString("dd/MM/yyyy");

    currentRules = new RuleSet();
    List<String> rulesList = currentRules.GetDescriptions();
    if (daysPassed == 0)
    {
      rule1.gameObject.SetActive(true);
      rule1.text = "- " + rulesList[0];

      rule2.gameObject.SetActive(false);
      rule3.gameObject.SetActive(false);
    }
    else if (daysPassed == 1)
    {
      rule1.gameObject.SetActive(true);
      rule1.text = "- " + rulesList[0];

      rule2.gameObject.SetActive(true);
      rule2.text = "- " + rulesList[1];

      rule3.gameObject.SetActive(false);
    }
    else
    {
      rule1.gameObject.SetActive(true);
      rule1.text = "- " + rulesList[0];

      rule2.gameObject.SetActive(true);
      rule2.text = "- " + rulesList[1];

      rule3.gameObject.SetActive(true);
      rule3.text = "- " + rulesList[2];
    }
  }


  public void CloseTutorial()
  {
    tutorial.SetActive(false);
  }

  public void QuiGame()
  {
    Application.Quit();
    Debug.Log("PA' FUERA");
  }

  public IEnumerator PlayResultSound(bool success)
  {
    yield return new WaitForSeconds(.2f);
    if (success)
    {
      soundManager.Valid();
    }
    else
    {
      soundManager.NotValid();
    }
  }

  public IEnumerator GetHighScore()
  {
    yield return null;

    List<HiScore> scores = scoreDAO.LoadHiScore();
    if (scores.Count > 0)
    {
      HiScore score = scores[0];
      mainMenuHighScoreNameText.text = "Player: " + score.name;
      mainMenuHighScoreScoreText.text = "Days passed: " + score.score;
      highscore = score.score;
      mainMenuHighscoreText.gameObject.SetActive(true);
    }
  }

}
