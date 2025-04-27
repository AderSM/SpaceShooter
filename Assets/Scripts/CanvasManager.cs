using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas1;
    [SerializeField] private GameObject canvas2;
    [SerializeField] private GameObject canvas3;
    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private Animator transitionAnim;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void ClickBotonIniciar()
    {
        soundPlayer.Play();
        StartCoroutine(ChangeToScene1());
    }

    public void ClickBotonOpciones()
    {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
        soundPlayer.Play();
    }

    public void ClickBotonInterfaz()
    {
        canvas1.SetActive(false);
        canvas3.SetActive(true);
        soundPlayer.Play();
    }

    public void ClickBotonRegresar()
    {
        canvas2.SetActive(false);
        canvas1.SetActive(true);
        soundPlayer.Play();
    }

    public void ClickBotonRegresar2()
    {
        canvas3.SetActive(false);
        canvas1.SetActive(true);
        soundPlayer.Play();
    }

    public void ClickBotonSalir()
    {
        soundPlayer.Play();
        Application.Quit();
    }
    IEnumerator ChangeToScene1()
    {
        transitionAnim.SetTrigger("exit");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
