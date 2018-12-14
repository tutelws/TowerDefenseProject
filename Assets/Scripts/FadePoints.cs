using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class FadePoints : MonoBehaviour {

    [SerializeField]
    private float _sequenceTime;
    [SerializeField]
    private float _translateY;
    [SerializeField]
    private GameObject _textGO;
    [SerializeField]
    private Color _textColor;

    private float _maxRandom = 0;
    private float _points;
    private Transform _targetTrans;

    public Color TextColor { set { _textColor = value; } }
    public float Points { set { _points = value; } }
    public Transform TargetTransform { set { _targetTrans = value; } }
    public float MaxRandom { set { _maxRandom = value; } }

    public Vector3 Offset;
    private void Start()
    {
        Text t = _textGO.GetComponent<Text>();
        _points *= -1;
        if (_points > 0)
        {
            t.color = Color.green;
            t.text = "+";
        }
        else
        {
            t.color = _textColor;
            t.text = "";
        }
        t.text += _points.ToString();
       
        Vector3 randVect = Vector3.zero;
        if (_maxRandom != 0)
                randVect = new Vector3(
                   UnityEngine.Random.Range(-_maxRandom, _maxRandom),
                   UnityEngine.Random.Range(-_maxRandom, _maxRandom),
                   UnityEngine.Random.Range(-_maxRandom, _maxRandom));
        transform.localPosition = CanvasManager.Instance.WorldToCanvas(_targetTrans.position + Offset + randVect);

        Sequence seq = DOTween.Sequence();
        seq.Append(_textGO.transform.DOLocalMoveY(_translateY, _sequenceTime))
            .Join(t.DOFade(0f, seq.Duration()))
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });

    }
   

}
