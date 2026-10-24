using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// Controla o estado da partida: tempo da fase, pontuação, boss, vitória e derrota.
public class ControlaJogo : MonoBehaviour {

  public static ControlaJogo instancia;

  // Duração da fase (segundos) antes do boss aparecer
  public float duracaoFase = 45f;
  // Pontos por inimigo destruído com a arma
  public int pontosPorInimigo = 10;
  // Pontos por derrotar o boss
  public int pontosBoss = 100;

  int pontos;
  float tempoRestante;
  bool comecou;
  bool faseBoss;
  bool acabou;
  bool venceu;
  string mensagem = "";

  void Awake () {
    instancia = this;
    Time.timeScale = 1f;
    tempoRestante = duracaoFase;
  }

  void Update () {
    // Depois do fim de jogo, espaço para recomeçar
    if (acabou) {
      if (PuloPressionado ()) {
        Time.timeScale = 1f;
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
      }
      return;
    }

    // Conta o tempo da fase; quando acaba, entra o boss
    if (comecou && !faseBoss) {
      tempoRestante -= Time.deltaTime;
      if (tempoRestante <= 0f) {
        tempoRestante = 0f;
        faseBoss = true;
      }
    }
  }

  // Botão de pular: barra de espaço
  public static bool PuloPressionado () {
    return Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
  }

  // Botão da arma: clique esquerdo do mouse
  public static bool TiroPressionado () {
    return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
  }

  public bool Acabou () { return acabou; }
  public bool Comecou () { return comecou; }
  public bool FaseBoss () { return faseBoss; }

  // Chamado pelo jogador no primeiro pulo
  public void Comecar () {
    comecou = true;
  }

  // Chamado pelo projétil quando destrói um inimigo
  public void InimigoDestruido () {
    if (acabou) return;
    pontos += pontosPorInimigo;
  }

  // Chamado pelo Boss quando a vida dele chega a zero
  public void BossDerrotado () {
    if (acabou) return;
    pontos += pontosBoss;
    acabou = true;
    venceu = true;
    mensagem = "Você venceu!\nO Uruca foi derrotado\nPontuação: " + pontos;
    Time.timeScale = 0f;
  }

  public void Derrota (string motivo) {
    if (acabou) return;

    acabou = true;
    venceu = false;
    mensagem = "Fim de jogo!\n" + motivo + "\nPontuação: " + pontos;
    Time.timeScale = 0f;
  }

  void OnGUI () {
    float escala = Screen.height / 960f;

    GUIStyle estilo = new GUIStyle (GUI.skin.label);
    estilo.fontSize = Mathf.RoundToInt (36 * escala);
    estilo.fontStyle = FontStyle.Bold;
    estilo.normal.textColor = Color.white;

    GUI.Label (new Rect (20 * escala, 20 * escala, Screen.width, 60 * escala), "Pontos: " + pontos, estilo);

    GUIStyle direita = new GUIStyle (estilo);
    direita.alignment = TextAnchor.UpperRight;
    string textoTempo = faseBoss ? "BOSS!" : "Tempo: " + Mathf.CeilToInt (tempoRestante);
    GUI.Label (new Rect (0, 20 * escala, Screen.width - 20 * escala, 60 * escala), textoTempo, direita);

    // Barra de vida do boss
    if (faseBoss && Boss.instancia != null && Boss.instancia.gameObject.activeSelf) {
      float largura = Screen.width - 40 * escala;
      float fracao = (float) Boss.instancia.Vida () / Boss.instancia.vidaMaxima;
      GUI.color = new Color (0f, 0f, 0f, 0.6f);
      GUI.DrawTexture (new Rect (20 * escala, 80 * escala, largura, 22 * escala), Texture2D.whiteTexture);
      GUI.color = new Color (0.85f, 0.15f, 0.25f);
      GUI.DrawTexture (new Rect (20 * escala, 80 * escala, largura * fracao, 22 * escala), Texture2D.whiteTexture);
      GUI.color = Color.white;
    }

    if (!comecou) {
      GUIStyle aviso = new GUIStyle (estilo);
      aviso.alignment = TextAnchor.MiddleCenter;
      GUI.Label (new Rect (0, Screen.height * 0.2f, Screen.width, 160 * escala),
        "Espaço para começar\n\nEspaço: pular\nClique: atirar", aviso);
    }

    if (acabou) {
      GUIStyle grande = new GUIStyle (estilo);
      grande.fontSize = Mathf.RoundToInt (50 * escala);
      grande.alignment = TextAnchor.MiddleCenter;
      grande.normal.textColor = venceu ? Color.yellow : new Color (1f, 0.35f, 0.35f);
      GUI.Label (new Rect (0, 0, Screen.width, Screen.height), mensagem + "\n\nEspaço para jogar de novo", grande);
    }
  }
}
