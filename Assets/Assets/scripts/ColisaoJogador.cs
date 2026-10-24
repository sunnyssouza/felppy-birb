using UnityEngine;

// Vai no FelpudoIde. Detecta o que causa a derrota do jogador.
public class ColisaoJogador : MonoBehaviour {

  // Colisão física: bateu em um inimigo
  void OnCollisionEnter2D (Collision2D colisao) {
    if (colisao.gameObject.GetComponent<Inimigo> () != null) {
      ControlaJogo.instancia.Derrota ("O Felpudo bateu no Lesmo");
    } else if (colisao.gameObject.GetComponent<Boss> () != null) {
      ControlaJogo.instancia.Derrota ("O Felpudo bateu no Uruca");
    }
  }

  // Trigger: entrou em uma zona mortal (chão ou teto)
  void OnTriggerEnter2D (Collider2D outro) {
    if (outro.GetComponent<ZonaMortal> () != null) {
      ControlaJogo.instancia.Derrota ("O Felpudo saiu da tela");
    }
  }
}
