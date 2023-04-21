using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ScoreTests
{
  // Hardcore production test, not asserting, just getting and displaying data
  [Test]
  public void ReadHighScores()
  {
    string url = "https://api.jazbelt.net/high_scores";

    IScoreDAO dao = new ScoreDAO.ScoreDAOBuilder()
      .ApiUrl(url)
      .Build();

    List<HiScore> scores = dao.LoadHiScore();

    foreach (HiScore score in scores)
    {
      Debug.Log(string.Format("High Score -> Name: {0}, Score: {1}", score.name, score.score));
    }
  }

  // Another hardcore production test, this should be disabled from pipelines
  [Test]
  public void StoreHighScore()
  {
    string url = "https://api.jazbelt.net/high_scores";

    IScoreDAO dao = new ScoreDAO.ScoreDAOBuilder()
      .ApiUrl(url)
      .Build();

    HiScore score = new HiScore
    {
      name = "Bob",
      score = 0
    };

    // dao.SaveHiScore(score);
  }
}
