using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Pausa : MonoBehaviour
{
    [SerializeField] private GameObject canvas1;
    [SerializeField] private GameObject canvas2;
    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private Animator transitionAnim;
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            canvas1.SetActive(true);
        }
    }

    public void ClickBotonReanudar()
    {
        canvas1.SetActive(false);
        soundPlayer.Play();
        Time.timeScale = 1;
    }

    public void ClickBotonOpciones()
    {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
        soundPlayer.Play();
    }
    public void ClickBotonRegresar()
    {
        canvas2.SetActive(false);
        canvas1.SetActive(true);
        soundPlayer.Play();
    }
    public void ClickBotonMenuPrincipal()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        soundPlayer.Play();
    }
    public void ClickBotonReiniciarNivel()
    {
        Time.timeScale = 1;
        StartCoroutine(ChangeToScene1());
        soundPlayer.Play();
    }
    IEnumerator ChangeToScene0()
    {
        transitionAnim.SetTrigger("exit");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
    IEnumerator ChangeToScene1()
    {
        transitionAnim.SetTrigger("exit");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}