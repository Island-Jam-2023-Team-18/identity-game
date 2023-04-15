using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

  //TO DO
  //private int daysPassed = 0;
  private float timeLeft = 10.0f;


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

  }

  void Update()
  {

    if (gameState == GameState.Game)
    {
      timeLeft -= Time.deltaTime;
      if(timeLeft <= 0.0f)
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

    gameUI.SetActive(false);
    dayEndUI.SetActive(true);
  }

  public void NextDay()
  {
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
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
