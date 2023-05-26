using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScaler : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D objectCollider;
    [SerializeField] private ChangeCharacter changeChara;

    private void Start()
    {
        objectCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        ScaleCollider(objectCollider);
    }

    public void ScaleCollider(CapsuleCollider2D collider)
    {
        if(changeChara.currCharIndex == 1)
        {
            collider.size = new Vector2(collider.size.x,3.17f);
        }
        else
        {
            collider.size = new Vector2(collider.size.x, 2.6f);
        }
    }
}
