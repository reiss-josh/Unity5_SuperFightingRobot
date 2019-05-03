using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBullet : MonoBehaviour
{
    public float bulletSpeed;
    public enum shotDirsEnum {East = 1, West = -1, North = 2, South = -2};
    public int shotDir;

    void Awake()
    {
        bulletSpeed = 0.015f;
        shotDir = (int)shotDirsEnum.East;
    }

    // Update is called once per frame
    void Update()
    {
        if(shotDir == 1 || shotDir == -1)
        {
            transform.Translate(bulletSpeed * shotDir, 0, 0);
        }
        if (shotDir == 2 || shotDir == -2)
        {
            transform.Translate(0, bulletSpeed * (shotDir/2), 0);
        }
    }
}
