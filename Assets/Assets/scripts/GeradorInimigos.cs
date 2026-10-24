using UnityEngine;
using System.Collections;

public class GeradorInimigos : MonoBehaviour
{
    // Prefab do inimigo (arraste no Inspector)
    public GameObject inimigoPrefab;

    // Intervalo entre spawns (segundos)
    public float intervalo = 3f;

    // Limites de spawn no cenário
    public float limiteX = 8f;
    public float limiteY = 4f;

    // Velocidade de movimento dos inimigos
    public float velocidade = 3f;

    // Limite X para destruir o inimigo ao sair da tela
    public float limiteDestruicaoX = -12f;

    void Start()
    {
        // Começa a gerar inimigos repetidamente
        InvokeRepeating("GerarInimigo", 0f, intervalo);
    }

    void GerarInimigo()
    {
        // Só gera inimigos depois do primeiro pulo e antes do boss
        if (ControlaJogo.instancia != null &&
            (!ControlaJogo.instancia.Comecou() || ControlaJogo.instancia.FaseBoss()))
            return;

        // Define posição de spawn (à direita da tela)
        float x = limiteX;
        float y = Random.Range(-limiteY, limiteY);
        Vector2 posicaoAleatoria = new Vector2(x, y);

        // Instancia o inimigo
        GameObject inimigo = Instantiate(inimigoPrefab, posicaoAleatoria, Quaternion.identity);

        // Inicia o movimento automático (corrotina)
        StartCoroutine(MoverInimigo(inimigo));
    }

    IEnumerator MoverInimigo(GameObject inimigo)
    {
        while (inimigo != null)
        {
            // Move o inimigo da direita para a esquerda
            inimigo.transform.Translate(Vector2.left * velocidade * Time.deltaTime);

            // Se o inimigo sair do limite visível, destrói o objeto
            if (inimigo.transform.position.x < limiteDestruicaoX)
            {
                Destroy(inimigo);
                yield break; // Sai da corrotina
            }

            yield return null; // Espera o próximo frame
        }
    }
}
