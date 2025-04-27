using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlVolumen : MonoBehaviour
{
    [SerializeField] private Slider controlVolumen;
    [SerializeField] private Slider controlSonido;
    [SerializeField] private AudioSource musica;
    [SerializeField] private AudioSource soundPlayer;
    [SerializeField] private AudioSource soundDisparo;
    [SerializeField] private AudioSource soundBum;
    // Start is called before the first frame update
    void Start()
    {
        controlVolumen.value = PlayerPrefs.GetFloat("volumenSave", 0.5f);
        controlSonido.value = PlayerPrefs.GetFloat("sonidoSave", 0.5f);
        controlSonido.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }

    private void OnSliderValueChanged()
    {
        soundPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        musica.volume = controlVolumen.value;
        soundPlayer.volume = controlSonido.value;
        soundDisparo.volume = controlSonido.value;
        soundBum.volume = controlSonido.value;
    }

    public void GuardarVolumen()
    {
        PlayerPrefs.SetFloat("volumenSave", controlVolumen.value);
    }

    public void GuardarSonido()
    {
        PlayerPrefs.SetFloat("sonidoSave", controlSonido.value);
    }
}
