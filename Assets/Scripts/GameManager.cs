using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

  // End Day UI
  public TextMeshProUGUI candidatesText;
  //public TextMeshProUGUI accuracyRateText;
  public TextMeshProUGUI trustText;

  // Visible variables
  public float timePerDay = 10f;

  //TO DO
  // Variables per run
  private int daysPassed = 0;
  private float dayTimeLeft = 10.0f;
  private float currentTrust = 50.0f;

  private int totalSuccesses = 0;
  private int totalFails = 0;

  // Variables per round
  private int roundSuccesses = 0;
  private int roundFails = 0;
  //private float candidateTimeLeft = 20.0f;

  // Debug
  public TextMeshProUGUI timeLeftText;
  public TextMeshProUGUI daysPassedText;
  public TextMeshProUGUI currentSuccessesText;
  public TextMeshProUGUI currentFailsText;

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
    Debug.Log(gameState);
  }

  void Update()
  {

    if (gameState == GameState.Game)
    {

      //candidateTimeLeft -= Time.deltaTime;

      dayTimeLeft -= Time.deltaTime;
      dayTimeLeft = Mathf.Round(dayTimeLeft * 100f) * 0.01f;
      //dayTimeLeft = Mathf.round(dayTimeLeft);
      timeLeftText.text = "Day time left: " + dayTimeLeft;
      if (dayTimeLeft <= 0.1f)
      {
        EndDay();
      }
      if (Input.GetKey("escape"))
      {
        PauseGame();
      }
    }
  }

  public void StartNewGame()
  {
    gameState = GameState.Game;
    Debug.Log(gameState);

    mainMenuUI.SetActive(false);
    gameUI.SetActive(true);

    dayTimeLeft = timePerDay;
  }

  public void PauseGame()
  {
    gameState = GameState.Paused;
    Debug.Log(gameState);

    gameUI.SetActive(false);
    pauseMenuUI.SetActive(true);
  }

  public void ResumeGame()
  {
    gameState = GameState.Game;
    Debug.Log(gameState);

    pauseMenuUI.SetActive(false);
    gameUI.SetActive(true);
  }

  public void EndDay()
  {
    gameState = GameState.DayEnd;
    Debug.Log(gameState);

    // TO DO CALCULATE TRUST!!!!!!
    currentTrust += (roundSuccesses - roundFails);
    int candidatesReviewed = Mathf.Abs(roundSuccesses) + Mathf.Abs(roundFails);
    candidatesText.text = "Candidates reviewed: " + candidatesReviewed;
    //successesText.text = "Successes: " + roundSuccesses;
    //failsText.text = "Fails: " + roundFails;
    trustText. text = "Boss' confidence: " + currentTrust;

    gameUI.SetActive(false);
    dayEndUI.SetActive(true);
  }

  public void NextDay()
  {
    daysPassed += 1;
    daysPassedText.text = "Days passed: " + daysPassed;

    totalSuccesses += roundSuccesses;
    roundSuccesses = 0;
    currentSuccessesText.text = "Round successes: " + roundSuccesses;

    totalFails += roundFails;
    roundFails = 0;
    currentFailsText.text = "Round fails: " + roundFails;

    dayTimeLeft = timePerDay;

    gameState = GameState.Game;
    Debug.Log(gameState);

    dayEndUI.SetActive(false);
    gameUI.SetActive(true);
  }

  public void GameOver()
  {
    gameState = GameState.GameOver;
    Debug.Log(gameState);

    gameUI.SetActive(false);
    gameOverUI.SetActive(true);
  }

  public void ExitToMainMenu()
  {

    gameState = GameState.MainMenu;
    Debug.Log(gameState);

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
    trustText.text = "Boss' confidence: " + currentTrust;
  }



  public void ValidateCandidate(bool accepted)
  {
    // TO DO LOGIC HERE
    if (accepted)
    {
      roundSuccesses++;
      currentSuccessesText.text = "Successes: " + roundSuccesses;
    }
    else
    {
      roundFails++;
      currentFailsText.text = "Fails: " + roundFails;
    }
  }
  
}
