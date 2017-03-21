using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    private SpriteRenderer _renderer;
    private new SpriteRenderer renderer
    {
        get
        {
            if(_renderer==null)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }
            return _renderer;
        }
    }
    private BoxCollider2D _collider;
    private new BoxCollider2D collider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<BoxCollider2D>();
            }
            return _collider;
        }
    }
    private Animator _animator;
    private Animator animator
    {
        get
        {
            if(_animator==null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    private WorldSreenPosition _screenWorldBorders;
    private float _speed;

    public void InitDefaults(float speed, WorldSreenPosition screenWorldBorders)
    {
        _speed = speed;
        _screenWorldBorders = screenWorldBorders;
        ResetPosition();
        animator.SetTrigger("Rise");
    }
    private void ResetPosition()
    {
        Vector3 pos = transform.position;
        pos.x = UnityEngine.Random.Range(_screenWorldBorders.left, _screenWorldBorders.right);

        if (pos.x - collider.bounds.extents.x < _screenWorldBorders.left)
        {
            pos.x = _screenWorldBorders.left + collider.bounds.extents.x;
        }

        if (pos.x + collider.bounds.extents.x > _screenWorldBorders.right)
        {
            pos.x = _screenWorldBorders.right - collider.bounds.extents.x;
        }

        pos.y = _screenWorldBorders.down + collider.bounds.extents.y;
        pos.z = UnityEngine.Random.Range(3.0f, 5.0f);

        transform.position = pos;
    }
	
	private void Update () {
        CheckForOut();
        transform.Translate(0, _speed * Time.deltaTime, 0);
	}

    private void CheckForOut()
    {
       if(transform.position.y - collider.bounds.extents.y > _screenWorldBorders.up)
        {
            gameObject.SetActive(false);
            GeneratorGhost.instance.ghostsCount--;
        }
    }
}
