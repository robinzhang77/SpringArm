using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTree : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // float fVal = speed * Time.deltaTime;
        // if (Input.GetKey(KeyCode.A))
        // {
        //     transform.position += new Vector3(-fVal, 0, 0);
        // }

        // else if (Input.GetKey(KeyCode.D))
        // {
        //     transform.position += new Vector3(fVal, 0, 0);
        // }

        // else if (Input.GetKey(KeyCode.W))
        // {
        //     transform.position += new Vector3(0, 0, fVal);
        // }

        // else if (Input.GetKey(KeyCode.S))
        // {
        //     transform.position += new Vector3(0, 0, -fVal);
        // }

        float vertical = Input.GetAxis("Vertical");

        float horizontal = Input.GetAxis("Horizontal");

        transform.Translate(new Vector3(horizontal, 0, vertical) * Time.deltaTime * speed);//注意参数，只传入一个vector是不行的

    }
}
