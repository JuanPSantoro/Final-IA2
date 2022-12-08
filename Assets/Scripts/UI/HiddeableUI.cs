using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddeableUI : MonoBehaviour
{
    [SerializeField]
    private float _xHide;
    [SerializeField]
    private float _xShow;
    [SerializeField]
    private float _animationTime;
    [SerializeField]
    private bool _startHided;

    private bool _animating;
    private float _currentTime;
    private Vector3 _endPosition;
    private Vector3 _startPosition;

    virtual protected void Start()
    {
        _xShow = transform.localPosition.x;
        _xHide += _xShow;
        transform.localPosition = new Vector3(_startHided ? _xHide : _xShow, transform.localPosition.y, transform.localPosition.z);

    }

    private void Update()
    {
        if (_animating)
        {
            if (_currentTime > _animationTime)
            {
                _animating = false;
                transform.localPosition = _endPosition;
            }
            else
            {
                _currentTime += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, _currentTime / _animationTime);
            }
        }
    }

    public void ShowUI()
    {
        _currentTime = 0;
        _endPosition = new Vector3(_xShow, transform.localPosition.y, transform.localPosition.z);
        _startPosition = transform.localPosition;
        _animating = true;
    }

    public void HideUI()
    {
        _currentTime = 0;
        _endPosition = new Vector3(_xHide, transform.localPosition.y, transform.localPosition.z);
        _startPosition = transform.localPosition;
        _animating = true;
    }
}
