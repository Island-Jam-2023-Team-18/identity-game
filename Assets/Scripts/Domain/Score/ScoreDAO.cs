using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ScoreDAO : IScoreDAO
{
  public class ScoreDAOBuilder {
    private string apiUrl;

    public ScoreDAOBuilder ApiUrl(string apiUrl)
    {
      this.apiUrl = apiUrl;
      return this;
    }

    public ScoreDAO Build()
    {
      return new ScoreDAO(apiUrl);
    }
  }

  private readonly string apiUrl;

  private ScoreDAO(string apiUrl)
  {
    this.apiUrl = apiUrl;
  }
  public List<HiScore> LoadHiScore()
  {
    using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
    {
      request.SendWebRequest();
      while (!request.isDone) ;

      if (request.result == UnityWebRequest.Result.Success)
      {
        string json = request.downloadHandler.text;
        Debug.Log("Raw high scores API response: " + json);
        HiScoreList scoreList = JsonUtility.FromJson<HiScoreList>("{\"scores\":" + json + "}");
        List<HiScore> scores = new List<HiScore>();
        scores.AddRange(scoreList.scores);
        return scores;
      }
      else
      {
        Debug.LogError("Error getting high scores from API: " + request.error);
        return new List<HiScore>();
      }
    }
  }

  public void SaveHiScore(HiScore hiScore)
  {
    string json = JsonUtility.ToJson(hiScore);
    using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, "POST"))
    {
      byte[] rawData = Encoding.UTF8.GetBytes(json);
      request.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawData);
      request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
      request.SetRequestHeader("Content-Type", "application/json");
      request.SendWebRequest();
      while (!request.isDone) ;

      if (request.result == UnityWebRequest.Result.Success)
      {
        Debug.Log("Successfully sent new high score");
      }
      else
      {
        Debug.LogError("Error sending high score: " + request.error);
      }
    }
  }
}
