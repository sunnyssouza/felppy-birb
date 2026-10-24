using UnityEngine;


public class ColisaoJogador : MonoBehaviour {


  void OnCollisionEnter2D (Collision2D colisao) {
    if (colisao.gameObject.GetComponent<Inimigo> () != null) {
      ControlaJogo.instancia.Derrota ("O Felpudo bateu no Lesmo");
    } else if (colisao.gameObject.GetComponent<Boss> () != null) {
      ControlaJogo.instancia.Derrota ("O Felpudo bateu no Uruca");
    }
  }

 
  void OnTriggerEnter2D (Collider2D outro) {
    if (outro.GetComponent<ZonaMortal> () != null) {
      ControlaJogo.instancia.Derrota ("O Felpudo saiu da tela");
    }
  }
}
