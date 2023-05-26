using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public int currCharIndex = 0;
    public GameObject[] characters;

    private void Start()
    {
        SetCurrentCharacter(currCharIndex);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currCharIndex = 0;
            SetCurrentCharacter(currCharIndex);
        }        

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currCharIndex = 1;
            SetCurrentCharacter(currCharIndex);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currCharIndex = 2;
            SetCurrentCharacter(currCharIndex);
        }
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
