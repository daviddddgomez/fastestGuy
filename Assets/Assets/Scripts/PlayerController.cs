using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject game;
    public GameObject enemyGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    public ParticleSystem dust;

    private Animator animator;
    private AudioSource audioPlayer;
    private float startY;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;

    }
	
	// Update is called once per frame
	void Update () {
        bool isGrounded = transform.position.y == startY;
        bool isPlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);

        if (isGrounded && isPlaying && userAction)
        {
            UpdateState("playerJump");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }
	}

    public void UpdateState(string state = null)
    {
        if(state != null)
        {
            animator.Play(state);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("playerDie");
            game.GetComponent<GameController>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("StopGenerator", true);
            game.SendMessage("ResetTimeScale");

            DustStop();

            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();
        } else if (other.gameObject.tag == "Point") {
            game.SendMessage("IncrementsPoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }
    }

    void GameReady()
    {
        game.GetComponent<GameController>().gameState = GameState.Ready;
    }

    void DustPlay()
    {
        dust.Play();
    }

    void DustStop()
    {
        dust.Stop();
    }
}
