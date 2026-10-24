using System.Collections;
using UnityEngine;

// Boss do fim da fase (Uruca). Fica fora da tela até o tempo acabar,
// entra pela direita, sobe e desce e atira no Felpudo.
public class Boss : MonoBehaviour {

  public static Boss instancia;

  public int vidaMaxima = 15;
  // Posição X onde ele para dentro da tela
  public float xAlvo = 1.5f;
  // Posição X onde ele espera fora da tela
  public float xFora = 6f;
  public float velocidadeEntrada = 2f;
  // Movimento de sobe e desce
  public float amplitude = 2.5f;
  public float velocidadeVertical = 1.2f;
  // Ataque
  public float intervaloTiro = 1.4f;
  public float velocidadeTiro = 4f;

  int vida;
  bool ativo;
  float tempoAtivo;
  float proximoTiro;
  SpriteRenderer grafico;
  Transform alvo;

  void Awake () {
    instancia = this;
    vida = vidaMaxima;
    grafico = GetComponent<SpriteRenderer> ();

    Rigidbody2D corpo = GetComponent<Rigidbody2D> ();
    if (corpo != null) {
      corpo.bodyType = RigidbodyType2D.Kinematic;
      corpo.useFullKinematicContacts = true;
    }

    transform.position = new Vector3 (xFora, 0f, transform.position.z);
  }

  void Start () {
    ColisaoJogador jogador = FindFirstObjectByType<ColisaoJogador> ();
    if (jogador != null) alvo = jogador.transform;
  }

  public int Vida () { return vida; }
  public bool Ativo () { return ativo; }

  void Update () {
    ControlaJogo jogo = ControlaJogo.instancia;
    if (jogo == null || !jogo.FaseBoss () || jogo.Acabou ()) return;

    Vector3 p = transform.position;

    if (!ativo) {
      // Entrando pela direita
      p.x = Mathf.MoveTowards (p.x, xAlvo, velocidadeEntrada * Time.deltaTime);
      transform.position = p;
      if (Mathf.Approximately (p.x, xAlvo)) {
        ativo = true;
        proximoTiro = Time.time + intervaloTiro;
      }
      return;
    }

    // Sobe e desce
    tempoAtivo += Time.deltaTime;
    p.y = Mathf.Sin (tempoAtivo * velocidadeVertical) * amplitude;
    transform.position = p;

    // Atira em direção ao Felpudo
    if (Time.time >= proximoTiro && alvo != null) {
      proximoTiro = Time.time + intervaloTiro;
      Vector2 origem = (Vector2) transform.position + new Vector2 (-0.8f, 0f);
      Vector2 direcao = ((Vector2) alvo.position - origem).normalized;
      Projetil.Criar (origem, direcao * velocidadeTiro, false);
    }
  }

  public void LevarDano () {
    if (!ativo || vida <= 0) return;

    vida--;
    StartCoroutine (Piscar ());

    if (vida <= 0) {
      ControlaJogo.instancia.BossDerrotado ();
      gameObject.SetActive (false);
    }
  }

  IEnumerator Piscar () {
    grafico.color = new Color (1f, 0.4f, 0.4f);
    yield return new WaitForSeconds (0.08f);
    grafico.color = Color.white;
  }
}
