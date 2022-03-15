using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float moveSpeed;
    bool isMoving;
    Vector2 movPos;
    private Animator anim;
    public LayerMask solidObjLayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // move player
        if(!isMoving)
        {
            movPos.x = Input.GetAxisRaw("Horizontal");
            movPos.y = Input.GetAxisRaw("Vertical");

            // Remove Diagonal movement
            if (movPos.x != 0) movPos.y = 0;

            if (movPos != Vector2.zero)
            {
                // sets animator parameters for animations
                anim.SetFloat("moveX", movPos.x);
                anim.SetFloat("moveY", movPos.y);

                Vector3 targetPos = transform.position;
                targetPos.x += movPos.x;
                targetPos.y += movPos.y;

                // checks if tile is walkable before moving
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(MovePlayer(targetPos));
                }
                
            }

        }
        anim.SetBool("isMoving", isMoving);

    }

    // player movement 
    IEnumerator MovePlayer(Vector3 targetPos)
    {
        isMoving = true;
        
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjLayer) != null)
        {
            Debug.Log("Collision");
            return false;
        }


        return true;
    }
}
