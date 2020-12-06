using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] GameObject laserPrefab;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float fireRate = 0.1f;

    Coroutine firingCoroutine;

    // Movement Variables
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float xPadding;
    float yPadding;

    void Start()
    {
        SetUpMoveBoundaries();
    }


    void Update()
    {
        MoveWithKeyboard();
        Fire();
    }
    void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xPadding = GetComponent<RectTransform>().rect.width / 2;
        yPadding = GetComponent<RectTransform>().rect.height / 2;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yPadding;
    }
    void MoveWithKeyboard()
    {
        var horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var vertical =   Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newHorizontal = Mathf.Clamp(transform.position.x + horizontal, xMin, xMax);
        var newVertical =   Mathf.Clamp(transform.position.y + vertical, yMin, yMax);

        transform.position = new Vector2(newHorizontal, newVertical);
    }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(Firing());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }
    IEnumerator Firing()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(fireRate);
        }
    }
}
