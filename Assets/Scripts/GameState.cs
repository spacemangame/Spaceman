using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
public class GameState
{
    public static GameState current;
    public int gameScore;
    public int gamePoints;

    public GameState(int score, int point) {
        this.gameScore = score;
        this.gamePoints = point;
    }
}

