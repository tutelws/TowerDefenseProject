using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LaunchScreen : MonoBehaviour {
    [SerializeField]
    private MaskableGraphic _fadeElem;
    [SerializeField]
    private Vector3 _endScale;
    [SerializeField]
    private float _sequenceDuration;
    [SerializeField]
    private GameObject _mainMenu;

    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_fadeElem.DOFade(1f, _sequenceDuration))
            .Join(_fadeElem.transform.DOScale(_endScale, seq.Duration()))
            //.Append(_fadeElem.DOFade(0f, _sequenceDuration / 4f))
            .OnComplete(() => {
                _mainMenu.SetActive(true);
            });
    }

}
