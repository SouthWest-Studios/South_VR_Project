using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlackJackCardScript : MonoBehaviour
{
    Transform childTransform;
    BlackJackScript blackJack; 

    public enum CardType
    {
        Treboles = 0,
        Picas = 1,
        Corazones = 2,
        Diamantes = 3
    }

    public int id;
    public int value;
    public CardType type;

    void Start()
    {
        blackJack = FindAnyObjectByType<BlackJackScript>();
        childTransform = transform.Find("Face");

        MeshRenderer meshRenderer = childTransform.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            Texture2D texture = null;

            if (id >= 1 && id <= 10)
            {
                texture = blackJack.spadesTextures[id - 1];
            }
            else if (id == 11) // J
            {
                texture = blackJack.spadesTextures[10];
            }
            else if (id == 12) // Q
            {
                texture = blackJack.spadesTextures[11];
            }
            else if (id == 13) // K
            {
                texture = blackJack.spadesTextures[12];
            }

            if (texture != null)
            {

                Shader urpShader = Shader.Find("Universal Render Pipeline/Lit");


                if (urpShader != null)
                {

                    Material nuevoMaterial = new Material(urpShader);


                    nuevoMaterial.SetTexture("_BaseMap", texture);


                    meshRenderer.material = nuevoMaterial;
                }

            }

        }
    }

}
