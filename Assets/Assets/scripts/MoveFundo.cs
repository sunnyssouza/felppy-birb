using UnityEngine;
using System.Collections;

public class MoveFundo : MonoBehaviour {

  float larguraTela;

  SpriteRenderer grafico;

  void Start(){
  
    grafico = GetComponent<SpriteRenderer>();

  }

  void Update(){
      
    float offset = Time.time * 0.5f;

    grafico = GetComponent<SpriteRenderer>();

    // mainTextureOffset usa a textura principal do shader (_MainTex no Standard, _BaseMap no URP)
    grafico.material.mainTextureOffset = new Vector2(offset, 0);
 
  }


}