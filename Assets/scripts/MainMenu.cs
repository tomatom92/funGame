using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class mainMenu : MonoBehaviour
{
    public Sprite playerImage;
    public PlayableDirector playableDirector;
    public Image currentImage;
    public GameObject ui;
    // Start is called before the first frame update

    public void PlayGame()
    {
        currentImage.sprite = playerImage;

        //play intro cutscene
        StartCoroutine(Wait());
        
       
    }
    public void StartGame(PlayableDirector director)
    {
        playableDirector.stopped -= StartGame;
        //change scene to actual game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator Wait()
    {
        ui.SetActive(false);
        playableDirector.Play();
        playableDirector.stopped += StartGame;
        yield return new WaitForSeconds(2);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
