using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectTween : MonoBehaviour, ITweanable {
    [SerializeField] private UIController _UIController;

    private Image _image;
    private TextMeshProUGUI _text;
    private RectTransform _rectTransform;

    private Vector2 _startPos;

    [SerializeField] private bool _movable = true;
    [SerializeField] private float _finalAlpha = 1f;
    [SerializeField] private Vector2 _direction;

    private void Awake() {
        TryGetComponent(out _image);
        TryGetComponent(out _text);

        _rectTransform = GetComponent<RectTransform>();

        _startPos = _rectTransform.anchoredPosition;

        _UIController.AddTweenObjects(this);
    }

    public void Appear(float duration) {
        if(_image != null) {
            Color tempColor = _image.color;
            tempColor.a = 0f;

            _image.color = tempColor;

            if (_movable) {
                _rectTransform.anchoredPosition = _rectTransform.anchoredPosition + _direction * 25f;

                _rectTransform.DOAnchorPos(_startPos, duration);
            }

            _image.DOFade(_finalAlpha, duration);
        }

        else if(_text != null) {
            Color tempColor = _text.color;
            tempColor.a = 0f;

            _text.color = tempColor;

            if (_movable) {
                _rectTransform.anchoredPosition = _rectTransform.anchoredPosition + _direction * 25f;

                _rectTransform.DOAnchorPos(_startPos, duration);
            }

            _text.DOFade(_finalAlpha, duration);
        }
    }

    public void Disappear(float duration) {
        if(_image != null) {
            _image.DOFade(0f, duration).OnComplete(() => {
                _UIController.gameObject.SetActive(false);
            });
        }

        else if(_text != null) {
            _text.DOFade(0f, duration).OnComplete(() => {
                _UIController.gameObject.SetActive(false);
            });
        }
    }
}
