using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Pulo do Felpudo (agora na barra de espaço).
public class ControlaJogadorMouseEsquerdo : MonoBehaviour {

  // Velocidade para cima a cada pulo
  public float forcaPulo = 6f;
  // Gravidade do Felpudo
  public float gravidade = 1.5f;

  bool comecou;
  Rigidbody2D corpoJogador;

  void Start () {
    corpoJogador = GetComponent<Rigidbody2D> ();
    corpoJogador.gravityScale = gravidade;
    corpoJogador.freezeRotation = true;
  }

  void Update () {

    if (ControlaJogo.instancia != null && ControlaJogo.instancia.Acabou ()) {
      return;
    }

    if (ControlaJogo.PuloPressionado ()) {

      if (!comecou) {
        comecou = true;
        corpoJogador.bodyType = RigidbodyType2D.Dynamic;
        if (ControlaJogo.instancia != null) {
          ControlaJogo.instancia.Comecar ();
        }
      }

      // Define a velocidade direto: cada pulo é sempre igual
      corpoJogador.linearVelocity = new Vector2 (0, forcaPulo);
    }

  }
}
