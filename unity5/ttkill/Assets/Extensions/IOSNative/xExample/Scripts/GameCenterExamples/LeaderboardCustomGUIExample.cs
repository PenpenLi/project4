////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardCustomGUIExample : MonoBehaviour {

	private string leaderboardId =  "your.leaderboard2.id.here";
	public int hiScore = 100;


	public GUIStyle headerStyle;
	public GUIStyle boardStyle;


	private GK_Leaderboard loadedLeaderboard = null;

	private GK_CollectionType displayCollection = GK_CollectionType.FRIENDS;

	void Awake() {

		GameCenterManager.OnAuthFinished += OnAuthFinished;



		GameCenterManager.OnScoresListLoaded += OnScoresListLoaded;

		//Initializing Game Center class. This action will trigger authentication flow
		GameCenterManager.init();
	}
	

	void OnGUI() {

		GUI.Label(new Rect(10, 20, 400, 40), "Custom Leaderboard GUI Example", headerStyle);

		if(GUI.Button(new Rect(400, 10, 150, 50), "Load Friends Scores")) {
			GameCenterManager.LoadScore(leaderboardId, 1, 10, GK_TimeSpan.ALL_TIME, GK_CollectionType.FRIENDS);
		}

		if(GUI.Button(new Rect(600, 10, 150, 50), "Load Global Scores")) {
			GameCenterManager.LoadScore(leaderboardId, 50, 150, GK_TimeSpan.ALL_TIME, GK_CollectionType.GLOBAL);
		}

		Color defaultColor = GUI.color;

		if(displayCollection == GK_CollectionType.GLOBAL) {
			GUI.color = Color.green;
		}
		if(GUI.Button(new Rect(800, 10, 170, 50), "Displaying Global Scores")) {
			displayCollection = GK_CollectionType.GLOBAL;
		}
		GUI.color = defaultColor;



		if(displayCollection == GK_CollectionType.FRIENDS) {
			GUI.color = Color.green;
		}
		if(GUI.Button(new Rect(800, 70, 170, 50), "Displaying Friends Scores")) {
			displayCollection = GK_CollectionType.FRIENDS;
		}
		GUI.color = defaultColor;

		GUI.Label(new Rect(10,  90, 100, 40), "rank", boardStyle);
		GUI.Label(new Rect(100, 90, 100, 40), "score", boardStyle);
		GUI.Label(new Rect(200, 90, 100, 40), "playerId", boardStyle);
		GUI.Label(new Rect(400, 90, 100, 40), "name ", boardStyle);
		GUI.Label(new Rect(550, 90, 100, 40), "avatar ", boardStyle);

		if(loadedLeaderboard != null) {
			for(int i = 1; i < 10; i++) {
				GK_Score score = loadedLeaderboard.GetScore(i, GK_TimeSpan.ALL_TIME, displayCollection);
				if(score != null) {
					GUI.Label(new Rect(10,  90 + 70 * i, 100, 40), i.ToString(), boardStyle);
					GUI.Label(new Rect(100, 90 + 70 * i, 100, 40), score.GetLongScore().ToString() , boardStyle);
					GUI.Label(new Rect(200, 90 + 70 * i, 100, 40), score.playerId, boardStyle);


					GK_Player player = GameCenterManager.GetPlayerById(score.playerId);
					if(player != null) {
						GUI.Label(new Rect(400, 90 + 70 * i , 100, 40), player.Alias, boardStyle);
						if(player.SmallPhoto != null) {
							GUI.DrawTexture(new Rect(550, 75 + 70 * i, 50, 50), player.SmallPhoto);
						} else  {
							GUI.Label(new Rect(550, 90 + 70 * i, 100, 40), "no photo ", boardStyle);
						}
					}

					if(GUI.Button(new Rect(650, 90 + 70 * i, 100, 30), "Challenge")) {
						GameCenterManager.IssueLeaderboardChallenge(leaderboardId, "Your message here", score.playerId);
					}

				}

			}
		}



	}

	private void OnScoresListLoaded (ISN_Result res) {
		if(res.IsSucceeded) {
			Debug.Log("Scores loaded");
			loadedLeaderboard = GameCenterManager.GetLeaderboard(leaderboardId);
		} else  {
			Debug.Log("Failed to load scores");
		}
	}





	private void OnAuthFinished (ISN_Result res) {
		if (res.IsSucceeded) {
			IOSNativePopUpManager.showMessage("Player Authed ", "ID: " + GameCenterManager.Player.Id + "\n" + "Name: " + GameCenterManager.Player.DisplayName);
		} else {
			IOSNativePopUpManager.showMessage("Game Center ", "Player authentication failed");
		}
	}



}
