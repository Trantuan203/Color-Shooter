using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterController : CharacterBase
{
    [Header("Shoot")]
    public Transform shootPoint;
    public GameObject bulletPrefab;
    [SerializeField] private float _shootTime;
    float _shootTimeCount = 0;

    void Update()
    {
        TestShoot();
    }

    private void TestShoot()
    {
        if (_shootTimeCount > 0)
        {
            _shootTimeCount -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameObject g = ObjectPool.Instance.GetObject(bulletPrefab);
            g.transform.position = shootPoint.position;

            float dir = character_dir;
            g.transform.rotation = Quaternion.Euler(0,  -180, 0);

            BulletBase bulletBase = g.GetComponent<BulletBase>();
            bulletBase.SetBulletColor(currentColor);
            bulletBase.ChangeBulletColor();

            g.SetActive(true);

            _shootTimeCount = _shootTime;
        }
    }
}
