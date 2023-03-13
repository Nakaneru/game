using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public int charIndex = 0;
    private int currCharIndex;
    public GameObject[] characters;

    private void Start()
    {
        SetCurrentCharacter(GetActiveCharacter());
    }

    private void Update()
    {
        if(charIndex > 2)
        {
            charIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            charIndex++;
            SetCurrentCharacter(GetActiveCharacter());
        }
    }

    private int GetActiveCharacter()
    {
        currCharIndex = charIndex;
        return currCharIndex;
    }

    private void SetCurrentCharacter(int currentCharIndex)
    {
        characters[currentCharIndex].gameObject.SetActive(true);
        CheckCharacterActivation(currentCharIndex);
    }

    private void CheckCharacterActivation(int charIndex)
    {
        switch (charIndex)
        {
            case 1:
                characters[0].gameObject.SetActive(false);
                characters[2].gameObject.SetActive(false);
                break;
            case 2:
                characters[0].gameObject.SetActive(false);
                characters[1].gameObject.SetActive(false);
                break;
            default:
                characters[1].gameObject.SetActive(false);
                characters[2].gameObject.SetActive(false);
                break;
        }
    }
}
