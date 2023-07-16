using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameController : MonoBehaviour
{
    public TextScriptableObject textScriptableObject;

    private TMP_Text playerText;
    private TMP_Text customerText;
    private int playerTextIndex = 0;
    private int customerTextIndex = 0;
    private int totalScore = 0;
    private GameObject[] buttons;
    private AudioSource audioSource;
    private ObjectRandomizer objectRandomizer;
    private int answerIndex = 0;
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip finalWin;
    [SerializeField] private AudioClip loseAudio;
    [SerializeField] private Image winImage;
    [SerializeField] private Image loseImage;
    [SerializeField] private Bridge bridge;
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        scoreText.text = totalScore.ToString();
        objectRandomizer = GetComponent<ObjectRandomizer>();
        winImage.enabled = false;
        loseImage.enabled = false;
        audioSource = GetComponent<AudioSource>();
        buttons = GameObject.FindGameObjectsWithTag("Button");
        customerText = GameObject.FindGameObjectWithTag("Customer").GetComponent<TMP_Text>();
        customerText.text = textScriptableObject.customerTexts[customerTextIndex];
        customerText.enabled = false;
        playerText = GameObject.FindGameObjectWithTag("Player").GetComponent<TMP_Text>();
        playerText.text = textScriptableObject.playerTexts[playerTextIndex];
    }


    public void CheckAnswer(int optionsIndex)
    {
        objectRandomizer.ShuffleObjects();
        customerText.enabled = true; 
        playerText.text = textScriptableObject.answers[answerIndex++];
        if (playerTextIndex < 8)
        {
            if (playerTextIndex == optionsIndex)
            {
                totalScore++;
                scoreText.text = totalScore.ToString();
                StartCoroutine(IncrementQuestion(winAudio));
                Debug.Log(totalScore);
            }
            else
            {
                scoreText.text = totalScore.ToString();
                StartCoroutine(IncrementQuestion(loseAudio));
            }
        }
        else
        {
            StartCoroutine(LastText());
            if(totalScore > 5)
            {
                if(playerTextIndex == optionsIndex)
                {
                    audioSource.clip = winAudio;                    
                    audioSource.Play();
                    totalScore++;
                    scoreText.text = totalScore.ToString(); 
                    Debug.Log("meh");
                }
                else
                {
                    audioSource.clip = loseAudio;
                    audioSource.Play();
                }
                StartCoroutine(EndGameCall(true));
            }
            else
            {
                if (playerTextIndex == optionsIndex)
                {
                    totalScore++;
                    scoreText.text = totalScore.ToString();
                    audioSource.clip = winAudio;                    
                    audioSource.Play();
                }
                else
                {
                    audioSource.clip = loseAudio;
                    audioSource.Play();
                }
                StartCoroutine(EndGameCall(false));
            }
        }
    }

    private IEnumerator IncrementQuestion(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        ButtonControl(false);        
        yield return new WaitForSeconds(2f);
        IncrementIndex();
        playerText.text = textScriptableObject.playerTexts[playerTextIndex];
        customerText.text = textScriptableObject.customerTexts[customerTextIndex];
        ButtonControl(true);
    }

    private void IncrementIndex()
    {
        if(playerTextIndex < 8)
        {
            playerTextIndex++;
            customerTextIndex++;
        }
    }
    private void ButtonControl(bool interactable)
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = interactable;
        }
    }

    private IEnumerator EndGameCall(bool won)
    {
        ButtonControl(false);
        yield return new WaitForSeconds(2);
        if(won)
        {
            audioSource.clip = finalWin;
            audioSource.Play();
            winImage.enabled = true;
            yield return new WaitForSeconds(2);
            bridge.TriggerWebCall("winScenario");
        }
        else
        {
            audioSource.clip = loseAudio;
            audioSource.Play();
            loseImage.enabled = true;
            yield return new WaitForSeconds(2);
            bridge.TriggerWebCall("failScenario");
        }
    }

    private IEnumerator LastText()
    {
        customerText.text = textScriptableObject.customerTexts[9];
        yield return new WaitForSeconds(1);
    }

}
