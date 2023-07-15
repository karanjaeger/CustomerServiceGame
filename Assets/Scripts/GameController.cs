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
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip loseAudio;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        buttons = GameObject.FindGameObjectsWithTag("Button");
        customerText = GameObject.FindGameObjectWithTag("Customer").GetComponent<TMP_Text>();
        customerText.text = textScriptableObject.customerTexts[customerTextIndex];
        playerText = GameObject.FindGameObjectWithTag("Player").GetComponent<TMP_Text>();
        playerText.text = textScriptableObject.playerTexts[playerTextIndex];
    }


    public void CheckAnswer(int optionsIndex)
    {

        if (playerTextIndex < 8)
        {
            if (playerTextIndex == optionsIndex)
            {
                totalScore++;
                StartCoroutine(IncrementQuestion(winAudio));
                Debug.Log(totalScore);
            }
            else
            {
                StartCoroutine(IncrementQuestion(loseAudio));
            }
        }
        else
        {

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

    private IEnumerator EndGameCall()
    {
        ButtonControl(false);
        yield return new WaitForSeconds(2);        

    }

}
