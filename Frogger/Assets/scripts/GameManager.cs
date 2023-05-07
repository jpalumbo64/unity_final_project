using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score;
    private Frogger frogger;
    private int lives;
    private Home[] homes;
    private int time;
    private int total_time;

    public GameObject gameOverMenu;
    public Text lives_text;
    public Text time_score;
    public Text current_text;
    public GameObject WinMenu;

    private void Awake(){
        homes = FindObjectsOfType<Home>();
        frogger = FindObjectOfType<Frogger>();
    }
    private void Start(){
        NewGame();
    }
    private void NewGame(){
        gameOverMenu.SetActive(false);
        //SetScore(0);
        SetLives(3);
        NewLevel();
        total_time = 0;
    }
    private void End_Game(){
        frogger.gameObject.SetActive(false);            //turn of the frog
        gameOverMenu.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(PlayAgain());

    }
    private IEnumerator PlayAgain(){
        bool play = false;
        while(!play){
            if(Input.GetKeyDown(KeyCode.Return)){
                play = true;
            }
            yield return null;
        }
        NewGame();
    }

    private void NewLevel(){
        for(int i = 0; i < homes.Length; i++){
            homes[i].enabled = false;
        }
        Respawn();
    }

   
    private void Respawn(){
        frogger.Respawn();
        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }
    private IEnumerator Timer(int dur){
        time = dur;
        current_text.text = time.ToString();
        while(time > 0){
            yield return new WaitForSeconds(1);
            time--;
            current_text.text = time.ToString();
        }
        //Display time 
        //add to leaderboard if time is good
        frogger.Death();
    }

    private void SetTime(int tii){
        //this.score = score;
        time_score.text = tii.ToString();
    }
    
    private void SetLives(int lives){
        this.lives = lives;
        lives_text.text = lives.ToString();
    }
   
    public void HomeReached(){
        frogger.gameObject.SetActive(false);
        total_time += (30 - time);
        SetTime(total_time);
        if(Cleared()){
            Invoke(nameof(NewLevel), 1f);
        }
        else{
            Invoke(nameof(Respawn), 1f);
        }
    }

    public void You_Died(){
        SetLives(lives - 1);
        if(lives > 0){
            Invoke(nameof(Respawn), 1f);
        }
        else{
            Invoke(nameof(End_Game), 1f);
        }
    }

    private bool Cleared(){
        for(int i = 0; i < homes.Length; i++){
            if(!homes[i].enabled){
                return false;
            }
        }
        return true;
    }
   
}
