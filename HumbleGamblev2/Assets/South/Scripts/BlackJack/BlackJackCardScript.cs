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

            if(type == CardType.Picas)
            {
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
            }
            else if (type == CardType.Diamantes)
            {
                if (id >= 1 && id <= 10)
                {
                    texture = blackJack.diamondsTextures[id - 1];
                }
                else if (id == 11) // J
                {
                    texture = blackJack.diamondsTextures[10];
                }
                else if (id == 12) // Q
                {
                    texture = blackJack.diamondsTextures[11];
                }
                else if (id == 13) // K
                {
                    texture = blackJack.diamondsTextures[12];
                }
            }
            else if (type == CardType.Treboles)
            {
                if (id >= 1 && id <= 10)
                {
                    texture = blackJack.clubsTextures[id - 1];
                }
                else if (id == 11) // J
                {
                    texture = blackJack.clubsTextures[10];
                }
                else if (id == 12) // Q
                {
                    texture = blackJack.clubsTextures[11];
                }
                else if (id == 13) // K
                {
                    texture = blackJack.clubsTextures[12];
                }
            }
            else if (type == CardType.Picas)
            {
                if (id >= 1 && id <= 10)
                {
                    texture = blackJack.heartsTextures[id - 1];
                }
                else if (id == 11) // J
                {
                    texture = blackJack.heartsTextures[10];
                }
                else if (id == 12) // Q
                {
                    texture = blackJack.heartsTextures[11];
                }
                else if (id == 13) // K
                {
                    texture = blackJack.heartsTextures[12];
                }
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
