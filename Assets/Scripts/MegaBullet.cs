using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBullet : MonoBehaviour
{
    public float bulletSpeed;
    public enum shotDirsEnum { West = 0, South = 1, East = 2, North = 3 };
    public int shotDir;

    private bool isRotated;
    private Vector2 startPoint;
    private Transform tf;

    void Awake()
    {
        tf = GetComponent<Transform>();
        bulletSpeed = 0.25f;
        isRotated = false;
        startPoint = tf.position;
    }

    void horizUpdate()
    {
        if (isRotated)
        {
            transform.Rotate(Vector3.forward * -90);
            isRotated = false;
        }
        transform.Translate(bulletSpeed * (shotDir - 1), 0, 0);
    }

    void vertUpdate()
    {
        if (!isRotated)
        {
            transform.Rotate(Vector3.forward * 90);
            isRotated = true;
        }
        transform.Translate(bulletSpeed * (shotDir - 2), 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(shotDir);
        Vector2 vectorCheck = Vector2.right;
        if (shotDir == 0)
            vectorCheck = Vector2.left;
        else if (shotDir == 2)
            vectorCheck = Vector2.right;
        else if (shotDir == 1)
            vectorCheck = Vector2.down;
        else
            vectorCheck = Vector2.up;

        if (shotDir % 2 == 0)
            horizUpdate();
        else
            vertUpdate();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, vectorCheck, bulletSpeed);
        Debug.DrawRay(transform.position, vectorCheck, Color.red);
        if (hit)
        {
            Destroy(gameObject);
            //Debug.Log(hit.collider.name);
        }
        if(Vector2.Distance(tf.position, startPoint) > 25.0f)
        {
            Destroy(gameObject);
        }
    }
}
