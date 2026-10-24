using UnityEngine;

// Tiro criado por código (não precisa de prefab).
// Do jogador: destrói inimigos e tira vida do boss.
// Do boss: derrota o jogador.
public class Projetil : MonoBehaviour {

  bool doJogador;

  static Sprite spriteBola;

  public static Projetil Criar (Vector2 posicao, Vector2 velocidade, bool doJogador) {
    GameObject obj = new GameObject (doJogador ? "TiroJogador" : "TiroBoss");
    obj.transform.position = posicao;
    obj.transform.localScale = Vector3.one * (doJogador ? 0.5f : 0.8f);

    SpriteRenderer grafico = obj.AddComponent<SpriteRenderer> ();
    grafico.sprite = SpriteBola ();
    grafico.color = doJogador ? new Color (1f, 0.9f, 0.2f) : new Color (0.75f, 0.2f, 1f);
    grafico.sortingOrder = 5;

    CircleCollider2D colisor = obj.AddComponent<CircleCollider2D> ();
    colisor.isTrigger = true;

    Rigidbody2D corpo = obj.AddComponent<Rigidbody2D> ();
    corpo.bodyType = RigidbodyType2D.Kinematic;
    corpo.useFullKinematicContacts = true;
    corpo.linearVelocity = velocidade;

    Projetil projetil = obj.AddComponent<Projetil> ();
    projetil.doJogador = doJogador;
    return projetil;
  }

  // Círculo branco gerado uma vez e reaproveitado
  static Sprite SpriteBola () {
    if (spriteBola != null) return spriteBola;

    int tamanho = 32;
    Texture2D textura = new Texture2D (tamanho, tamanho);
    float raio = tamanho / 2f;
    for (int y = 0; y < tamanho; y++) {
      for (int x = 0; x < tamanho; x++) {
        float distancia = Vector2.Distance (new Vector2 (x + 0.5f, y + 0.5f), new Vector2 (raio, raio));
        float alfa = Mathf.Clamp01 (raio - distancia);
        textura.SetPixel (x, y, new Color (1f, 1f, 1f, alfa));
      }
    }
    textura.Apply ();
    spriteBola = Sprite.Create (textura, new Rect (0, 0, tamanho, tamanho), new Vector2 (0.5f, 0.5f), 100f);
    return spriteBola;
  }

  void Update () {
    // Saiu da tela: some
    Vector3 p = transform.position;
    if (p.x > 6f || p.x < -6f || p.y > 7f || p.y < -7f) {
      Destroy (gameObject);
    }
  }

  void OnTriggerEnter2D (Collider2D outro) {
    if (doJogador) {
      if (outro.GetComponent<Inimigo> () != null) {
        Destroy (outro.gameObject);
        Destroy (gameObject);
        if (ControlaJogo.instancia != null) ControlaJogo.instancia.InimigoDestruido ();
        return;
      }

      Boss boss = outro.GetComponent<Boss> ();
      if (boss != null && boss.Ativo ()) {
        boss.LevarDano ();
        Destroy (gameObject);
      }
    } else {
      if (outro.GetComponent<ColisaoJogador> () != null) {
        ControlaJogo.instancia.Derrota ("O Uruca acertou o Felpudo");
        Destroy (gameObject);
      }
    }
  }
}
