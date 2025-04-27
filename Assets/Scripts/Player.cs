using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float ratioDisparo;
    [SerializeField] private Disparo disparoPrefab;
    private float temporizador = 0.5f;
    private float vidas = 100;
    [SerializeField] private Transform[] posiciones;
    private ObjectPool<Disparo> dispPlayerPool;
    [SerializeField] private AudioSource soundDisparo;
    [SerializeField] private TextMeshProUGUI textoVida;
    [SerializeField] private GameObject canvas1;
    private bool isSpecialActive;
    private bool isOnCooldown;
    private float specialRatioDisparoMult = 0.5f;
    [SerializeField] private float specialDuration;
    [SerializeField] private float cooldownDuration;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private float readyAlpha = 1f;
    [SerializeField] private float cooldownAlpha = 0.5f;
    // Start is called before the first frame update
    private void Awake()
    {
        dispPlayerPool = new ObjectPool<Disparo>(CreateDisparo, null, OnReleaseDisparo, OnDestroyDisparo);
    }


    private Disparo CreateDisparo()
    {
        Disparo disparoCopy = Instantiate(disparoPrefab, transform.position, Quaternion.identity);
        disparoCopy.MiPool3 = dispPlayerPool;
        return disparoCopy;
    }

    private void OnReleaseDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(false);
    }

    private void OnDestroyDisparo(Disparo disparo)
    {
        Destroy(disparo.gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        DeliminarMovimiento();
        Disparar();
        if (Input.GetKeyDown(KeyCode.E) && !isSpecialActive && !isOnCooldown)
        {
            StartCoroutine(ActivateSpecial());
        }
    }

    void Movimiento()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector2(inputH, inputV).normalized * velocidad * Time.deltaTime);
    }

    void DeliminarMovimiento()
    {
        float xClamped = Mathf.Clamp(transform.position.x, -8.4f, 8.4f);
        float yClamped = Mathf.Clamp(transform.position.y, -4.5f, 4.5f);
        transform.position = new Vector3(xClamped, yClamped, 0);
    }

    void Disparar()
    {
        temporizador += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && temporizador > ratioDisparo)
        {
            for (int i = 0; i < 2; i++)
            {
                Disparo disparoCopy = dispPlayerPool.Get();
                disparoCopy.transform.position = posiciones[i].position;
                disparoCopy.gameObject.SetActive(true);
            }
            temporizador = 0;
            soundDisparo.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoEnemigo") || elOtro.gameObject.CompareTag("Enemigo"))
        {
            vidas -= 10;
            textoVida.text = "" + vidas;
        }
        if (vidas <= 0)
        {
            Destroy(this.gameObject);
            Time.timeScale = 0;
            canvas1.SetActive(true);
        }
    }

    private IEnumerator ActivateSpecial()
    {
        isSpecialActive = true;
        if (abilityIcon != null)
        {
            SetIconAlpha(cooldownAlpha);
        }
        float originalFireRate = ratioDisparo;
        ratioDisparo *= specialRatioDisparoMult;
        yield return new WaitForSeconds(specialDuration);
        ratioDisparo = originalFireRate;
        isSpecialActive = false;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
        if (abilityIcon != null)
        {
            SetIconAlpha(readyAlpha);
        }    
    }

    private void SetIconAlpha(float alpha)
    {
        Color color = abilityIcon.color;
        color.a = alpha;
        abilityIcon.color = color;
    }
}
