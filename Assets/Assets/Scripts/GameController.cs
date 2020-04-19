using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { Idle, Playing, Ended, Ready };

public class GameController : MonoBehaviour {

    [Range (0f, 02f)]
    public float parallaxSpeed = 0.01f;
    public RawImage backGround;
    public RawImage platform;

    
    public GameObject gameIdle;
    public GameObject uiScore;
    public Text pointsText;
    public Text recordText;
    public GameState gameState = GameState.Idle;

    public GameObject player;
    public GameObject enemyGenerator;    

    public float scaleTime = 6f;
    public float scaleInc = .25f;
    public int timesInc = 0;

    private int points = 0;
    private AudioSource musicPlayer;

    // Use this for initialization
    void Start () {
        musicPlayer = GetComponent<AudioSource>();
        recordText.text = "Record: " + GetMaxScore().ToString();
	}
	
	// Update is called once per frame
	void Update () {
        // Game starts
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);
        if (gameState == GameState.Idle && userAction)
        {
            gameState = GameState.Playing;
            gameIdle.SetActive(false);
            uiScore.SetActive(true);
            player.SendMessage("UpdateState", "playerRun");
            player.SendMessage("DustPlay");

            enemyGenerator.SendMessage("StartGenerator");
            musicPlayer.Play();
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
        } else if(gameState == GameState.Playing)
        {
            parallax();
        } else if (gameState == GameState.Ready)
        {
            if(userAction)
            {
                RestartGame(); 
            }
        }
    }

    private void parallax()
    {
        //float finalSpeed = parallaxSpeed * Time.deltaTime;
        backGround.uvRect = new Rect(backGround.uvRect.x + parallaxSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + parallaxSpeed, 0f, 1f, 1f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Scena 1");
    }

    void GameTimeScale()
    {
        Time.timeScale += scaleInc;
        Debug.Log("Ritmo incrementado: " + Time.timeScale.ToString());
    }

    public void ResetTimeScale()
    {
        CancelInvoke("GameTimeScale");
        Time.timeScale = 1f;
        Debug.Log("Ritmo restablecido:" + Time.timeScale.ToString());
    }

    public void IncrementsPoints()
    {
        points++;
        pointsText.text = points.ToString();
        if(points > GetMaxScore()){
            recordText.text = "Record: " + points.ToString();
            SaveScore(points);
        }
    }

    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt("Max Points", 0);
    }

    public void SaveScore(int currentPoints)
    {
        PlayerPrefs.SetInt("Max Points", currentPoints);
    }
}
