using UnityEngine;


public class Arma : MonoBehaviour {

  
  public float intervaloTiro = 0.25f;
  public float velocidadeTiro = 9f;
 
  public Vector2 saidaTiro = new Vector2 (0.9f, 0f);

  float proximoTiro;

  void Update () {
    ControlaJogo jogo = ControlaJogo.instancia;
    if (jogo == null || !jogo.Comecou () || jogo.Acabou ()) return;

    if (ControlaJogo.TiroPressionado () && Time.time >= proximoTiro) {
      proximoTiro = Time.time + intervaloTiro;
      Vector2 origem = (Vector2) transform.position + saidaTiro;
      Projetil.Criar (origem, new Vector2 (velocidadeTiro, 0f), true);
    }
  }
}
