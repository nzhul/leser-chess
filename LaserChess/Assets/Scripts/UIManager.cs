using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    [SerializeField]
    private GameObject _selectionPanel = default;

    [SerializeField]
    private Button _endTurnBtn, _attackButton, _defendButton = default;

    [SerializeField]
    private TextMeshProUGUI _selectedPieceName = default;

    [SerializeField]
    private Image _portrait, _hpGreen, _hpFrame = default;

    [SerializeField]
    private Sprite[] _frames = default;

    private Image _attackIcon, _defendIcon;

    private List<Image> _apImages = new List<Image>();

    private Color _dimmedColor = new Color(0.7f, .7f, .7f, 1);

    private void Start()
    {
        _endTurnBtn.onClick.AddListener(delegate { OnEndTurnClicked(); });
        _attackIcon = _attackButton.transform.Find("Icon").GetComponent<Image>();
        _defendIcon = _defendButton.transform.Find("Icon").GetComponent<Image>();
        _apImages.AddRange(_attackButton.transform.FindChildren("AP").Select(x => x.GetComponent<Image>()));
        _apImages.AddRange(_defendButton.transform.FindChildren("AP").Select(x => x.GetComponent<Image>()));
    }

    public void UpdateSelectedPiece(Piece selectedPiece)
    {
        if (selectedPiece == null)
        {
            _selectionPanel.SetActive(false);
            return;
        }

        _selectionPanel.SetActive(true);

        _selectedPieceName.text = selectedPiece.PieceType.Name.ToUpper();
        _portrait.sprite = selectedPiece.Portrait;
        UpdateHealthbar(selectedPiece.Hitpoints, selectedPiece.CurrentHitpoints);
    }

    private void UpdateHealthbar(int hitpoints, int currentHitpoints)
    {
        var frame = _frames[hitpoints - 1];
        _hpFrame.sprite = frame;
        _hpGreen.fillAmount = (float)currentHitpoints / hitpoints;
    }

    public void ToggleUIButtons(bool value)
    {
        _attackButton.interactable = value;
        _defendButton.interactable = value;
        _endTurnBtn.interactable = value;

        if (!value)
        {
            _attackIcon.color = _dimmedColor;
            _defendIcon.color = _dimmedColor;
            _apImages.ForEach(x => x.color = _dimmedColor);
        }
        else
        {
            _attackIcon.color = Color.white;
            _defendIcon.color = Color.white;
            _apImages.ForEach(x => x.color = Color.white);
        }
    }

    private void OnEndTurnClicked()
    {
        PlayerManager.Instance.EndTurn();
    }
}
