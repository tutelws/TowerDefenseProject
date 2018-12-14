using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WaveCounter : MonoBehaviour {
    [SerializeField]
    private float _sequenceDuration;
    [SerializeField]
    private float _maxScale;
    [SerializeField]
    private GameObject _counterTextGO;
    [SerializeField]
    private Color _textColor;

    private Text _counterText;
    private int _currWaveNumber;

    public int MaxWaveNumber;

    private void Start()
    {
        _currWaveNumber = -1;
        MaxWaveNumber = GameManager.Instance.GetLevelConfig.waves.Length;
        _counterText = _counterTextGO.GetComponent<Text>();
    }
    public void SetWaveNumber(int currWaveNumber)
    {
        if (_currWaveNumber == currWaveNumber || currWaveNumber > MaxWaveNumber)
            return;
        _currWaveNumber = currWaveNumber;
        _counterText.text = _currWaveNumber + " | " + MaxWaveNumber;
        Vector3 _oldScale = _counterTextGO.transform.localScale;
        Sequence seq = DOTween.Sequence();
        seq.Append(_counterTextGO.transform.DOScale(_maxScale, _sequenceDuration / 2f))
            .Append(_counterTextGO.transform.DOScale(_oldScale, _sequenceDuration / 2f));
    }
}
