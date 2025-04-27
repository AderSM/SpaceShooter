using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Disparo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;

    private ObjectPool<Disparo> miPool2;

    public ObjectPool<Disparo> MiPool2 { get => miPool2; set => miPool2 = value; }

    private ObjectPool<Disparo> miPool3;
    public ObjectPool<Disparo> MiPool3 { get => miPool3; set => miPool3 = value; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (CompareTag("DisparoEnemigo") && (elOtro.gameObject.CompareTag("Player") || elOtro.gameObject.CompareTag("Limite")))
        {
            miPool2.Release(this);
        }
        if (CompareTag("DisparoPlayer") && elOtro.gameObject.CompareTag("Enemigo"))
        {
            miPool3.Release(this);
        }
        if (CompareTag("DisparoPlayer") && elOtro.gameObject.CompareTag("Limite"))
        {
            miPool3.Release(this);
        }
    }
}
