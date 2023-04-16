using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  private enum GameState { MainMenu, Game, Paused, DayEnd, GameOver }
  private GameState gameState;

  // Main UI Panels
  public GameObject mainMenuUI;
  public GameObject gameUI;
  public GameObject pauseMenuUI;
  public GameObject dayEndUI;
  public GameObject gameOverUI;

  // Main Game UI
  private bool rulesRead = false;
  public TextMeshProUGUI rulesDate;
  public TextMeshProUGUI rule1;
  public TextMeshProUGUI rule2;
  public TextMeshProUGUI rule3;
  public TextMeshProUGUI rulesButtonText;

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

  // Visible variables
  public float timePerDay = 10f;

  // Variables per run
  private int daysPassed = 0;
  private float dayTimeLeft = 10.0f;
  private float currentTrust = 5.0f;

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

  }

  void Start()
  {
    currentDate = DateTime.Today;
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
        if (dayTimeLeft <= 0.1f)
        {
          EndDay();
        }
      }

      if (Input.GetKeyDown("escape")) { PauseGame(); }

      if(Input.GetKeyDown(KeyCode.P)) { EndDay(); }

      if(Input.GetKeyDown(KeyCode.O)) { GameOver(); }

      if(Input.GetKeyDown(KeyCode.L)) { ToogleShowDebugInfo(); }

      if (Input.GetKeyDown(KeyCode.K)) { GetNewRules(); }
    }
  }

  public void StartNewGame()
  {
    gameState = GameState.Game;

    mainMenuUI.SetActive(false);
    gameUI.SetActive(true);

    dayTimeLeft = timePerDay;

    GetNewRules();
    GetNewCandidate();
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
    gameState = GameState.DayEnd;
    
    // TO DO CALCULATE TRUST!!!!!!
    currentTrust += ((roundSuccesses - roundFails) - daysPassed);
    int candidatesReviewed = Mathf.Abs(roundSuccesses) + Mathf.Abs(roundFails);
    candidatesText.text = "Candidates reviewed: " + candidatesReviewed;

    // calculate the accuracy rate as a percentage
    float accuracyRate = ((float)roundSuccesses / candidatesReviewed) * 100f;
    accuracyText.text = "Accuracy rate: " + accuracyRate.ToString("F2") + "%";

    gameUI.SetActive(false);
    dayEndUI.SetActive(true);
  }

  public void NextDay()
  {
    daysPassed += 1;
    daysPassedText.text = "Days passed: " + daysPassed;
    currentDate.AddDays(1);

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
    gameState = GameState.GameOver;

    gameUI.SetActive(false);
    gameOverUI.SetActive(true);
  }

  public void ExitToMainMenu()
  {

    gameState = GameState.MainMenu;

    gameUI.SetActive(false);
    pauseMenuUI.SetActive(false);
    dayEndUI.SetActive(false);
    gameOverUI.SetActive(false);

    mainMenuUI.SetActive(true);

    daysPassed = 0;
    daysPassedText.text = "Days passed: " + daysPassed;
    dayTimeLeft = 10.0f;
    currentTrust = 50.0f;

    totalSuccesses = 0;
    totalFails = 0;
    
    roundSuccesses = 0;
    currentSuccessesText.text = "Round successes: " + roundSuccesses;

    roundFails = 0;
    currentFailsText.text = "Round fails: " + roundFails;
    int candidatesReviewed = Mathf.Abs(roundSuccesses) + Mathf.Abs(roundFails);

    candidatesText.text = "Candidates reviewed: " + candidatesReviewed;
    accuracyText.text = "Boss' confidence: " + currentTrust;
  }



  public void ValidateCandidate(bool accepted)
  {

    if (!rulesRead) return;

    ValidationResult validationresult = currentRules.Validate(currentCandidate, currentDate, (daysPassed));
    // TO DO LOGIC HERE


    if (validationresult == ValidationResult.VALID)
    {
      roundSuccesses++;
      currentSuccessesText.text = "Successes: " + roundSuccesses;
    }
    else
    {
      // TO DO
      switch(validationresult)
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
    if (rulesButtonText.text == "GO!") rulesButtonText.text = "RULES";
    else rulesButtonText.text = "GO!";
    
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

  public void QuiGame()
  {
    Application.Quit();
    Debug.Log("PA' FUERA");
  }
  
}
