using System.Collections.Generic;

public interface IScoreDAO
{
  public void SaveHiScore(HiScore hiScore);
  public List<HiScore> LoadHiScore();
}
