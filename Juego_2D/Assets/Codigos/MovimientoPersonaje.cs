using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 7f;

    public Vector3 posicionInicio { get; set; }
    public Vector3 escalaInicio { get; set; }

    private bool enElsuelo = false;
    private Rigidbody2D cuerpoRigido;
    private Animator animaciones;

    void Awake()
    {
        escalaInicio = transform.localScale;
        posicionInicio = transform.position;
        cuerpoRigido = GetComponent<Rigidbody2D>();   
        animaciones = GetComponent<Animator>();
    }

    void Update()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        cuerpoRigido.velocity = new Vector2(movimientoHorizontal * velocidadMovimiento, cuerpoRigido.velocity.y);

        if (Input.GetButtonDown("Jump") && enElsuelo)
        {
            cuerpoRigido.velocity = new Vector2(movimientoHorizontal * velocidadMovimiento,0);
            cuerpoRigido.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
            enElsuelo = false;
        }

        if (movimientoHorizontal > 0)
            transform.localScale = new Vector3(escalaInicio.x, escalaInicio.y, escalaInicio.z);
        else if (movimientoHorizontal < 0)
            transform.localScale = new Vector3(-escalaInicio.x, escalaInicio.y, escalaInicio.z);

        animaciones.SetInteger("Salto", (int) cuerpoRigido.velocity.y);
        animaciones.SetBool("Piso", enElsuelo);

        if(enElsuelo)
            animaciones.SetFloat("MovimientoHorizontal", Mathf.Abs(movimientoHorizontal));
    }

    void OnCollisionStay2D (Collision2D collision)
    {
        enElsuelo = collision.gameObject.CompareTag("Suelo");

        if (collision.gameObject.CompareTag("Morir"))
            Reinicio();
    }

    void Reinicio()
    {
        cuerpoRigido.velocity = Vector2.zero;
        cuerpoRigido.angularVelocity = 0;
        cuerpoRigido.bodyType = RigidbodyType2D.Static;
        transform.position = posicionInicio;
        cuerpoRigido.bodyType = RigidbodyType2D.Dynamic;
    }
}

