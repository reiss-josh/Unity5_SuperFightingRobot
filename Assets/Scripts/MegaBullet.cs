using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBullet : MonoBehaviour
{
    public float bulletSpeed;
    public enum shotDirsEnum {West = 0, South = 1, East = 2, North = 3};
    public int shotDir;

    private bool isRotated;

    void Awake()
    {
        bulletSpeed = 0.5f;
        shotDir = (int)shotDirsEnum.East;
        isRotated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(shotDir == 0 || shotDir == 2)
        {
            transform.Translate(bulletSpeed * (shotDir - 1), 0, 0);
            if(isRotated)
            {
                transform.Rotate(Vector3.forward * -90);
                isRotated = false;
            }
        }
        if (shotDir == 1 || shotDir == 3)
        {
            if (!isRotated)
            {
                transform.Rotate(Vector3.forward * 90);
                isRotated = true;
            }
            transform.Translate(bulletSpeed * (shotDir - 2), 0, 0);   
        }
    }
}
