using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System.Collections.Generic;
public class PlayGameScript : MonoBehaviour {
    public bool isAuthorized;
    public bool initializeOnStart;
	// Use this for initialization
	void Start () {
        if (initializeOnStart)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            SignIn();
        }
	}
	
	void SignIn()
    {
        Social.localUser.Authenticate(success => { isAuthorized = success; Debug.Log("LOGGED IN "+success); });
       
    }

    #region Achievments
    public static void UnlockAchievement(string id)
    {
        if (!Social.localUser.authenticated) return;
        List<string> ids = new List<string>();
        Social.LoadAchievements(achiv => 
        {
            foreach(Achievement a in achiv)
            {
                ids.Add(a.Id);
            }
        });
        if(!ids.Contains(id))
            Social.ReportProgress(id, 100, callback => { });
    }
    public static void IncrementAchievement(string id, int steps)
    {
        if (!Social.localUser.authenticated) return;
        PlayGamesPlatform.Instance.IncrementAchievement(id, steps, callback => { });

    }
    public static void ShowAchievementsUI()
    {
        if(Social.localUser.authenticated)
            Social.ShowAchievementsUI();
        
    }
    #endregion
    #region Leaderboards
    public static void AddToLeaderboard(string boardId, int score)
    {
        if (!Social.localUser.authenticated) return;
        Social.ReportScore(score, boardId, callback => { });
    }
    public static void ShowLeaderBoardsUI()
    {
        if (Social.localUser.authenticated)
            Social.ShowLeaderboardUI();
    }
    
    #endregion
}
