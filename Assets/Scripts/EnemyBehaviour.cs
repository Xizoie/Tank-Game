using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    KillCounter killCounterScript;

    GameManager _gameManager;
    GameObject _player;

    float _enemyHealth = 100f;
    float _enemyMoveSpeed = 2f;
    Quaternion _targetRotation;
    bool _disableEnemy = false;
    Vector2 _moveDirection;
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        killCounterScript = GameObject.Find("KCO").GetComponent<KillCounter>();


    }

    void Update()
    {
        if (!_gameManager._gameOver && !_disableEnemy)
        {
            MoveEnemy();
            RotateEnemy();

        }
    }

    void MoveEnemy()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            _player.transform.position, _enemyMoveSpeed * Time.deltaTime);

    }
    void RotateEnemy()
    {
        _moveDirection = _player.transform.position - transform.position;
        _moveDirection.Normalize();

        _targetRotation = Quaternion.LookRotation(Vector3.forward, _moveDirection );

        if (transform.rotation != _targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation,
                200 * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(Damaged());

            _enemyHealth  -= 40f;

            if (_enemyHealth <= 0f)
            {
                Destroy(gameObject);
                killCounterScript.AddKill();

            }

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            _gameManager._gameOver = true;
            collision.gameObject.SetActive(false);
        }
    }
    IEnumerator Damaged()
    {
        _disableEnemy = true;
        yield return new WaitForSeconds(0.5f);
        _disableEnemy = false;
    }
}
