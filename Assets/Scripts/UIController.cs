using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _weaponUpgradeMenu;
    [SerializeField] private TMP_Text _enemyRemainingText, _timerRoundText, _roundIdText;
    
    [SerializeField] private GameObject[] _aims;
    [SerializeField] private Upgrade[] _upgrades;
    [SerializeField] private GameObject _winMenu;

    private int _currentRoundId;

    private void Start() {
        GameManager.Instance.EventManager.OnNextRoundStarted += OnNextRoundStarted;
        GameManager.Instance.EventManager.OnRoundFinished += OnRoundFinished;
        GameManager.Instance.EventManager.OnChoseWeapon += OnChoseWeapon;
        GameManager.Instance.EventManager.OnImprovedWeapon += OnImprovedWeapon;
        GameManager.Instance.EventManager.OnGameStarted += OnGameStarted;
        GameManager.Instance.EventManager.OnWinGame += OnWinGame;

        _winMenu.SetActive(false);
        _timerRoundText.enabled = false;
        UpdateRoundText();
    }

    private void OnWinGame() {
        foreach (var aim in _aims) {
            aim.SetActive(false);
        }
        _winMenu.SetActive(true);
    }

    private void OnGameStarted() { 
        _startMenu.SetActive(false);
        
    }

    private void OnImprovedWeapon(Weapon value) {
        _weaponUpgradeMenu.SetActive(false);
    }

    private void OnChoseWeapon(Weapon value) {
        _weaponUpgradeMenu.SetActive(true);
       
    }

    private void OnRoundFinished() {
        _startMenu.SetActive(true);
        _timerRoundText.enabled = false;
        _aims[_currentRoundId].SetActive(false);
        UpdateRoundText();
    }

    private void OnNextRoundStarted(int value) {
        _currentRoundId = value;
        _startMenu.SetActive(false);
        for (int i = 0; i < _aims.Length; i++) {
            if (i == value) {
                _aims[i].SetActive(true);
            }
            else { 
                _aims[i].SetActive(false);
            }
        }
        _timerRoundText.enabled = true;
        
    }

    public void UpdateEnemiesText(int enemyRemaining) {
        _enemyRemainingText.text = "Remaining: " + enemyRemaining; 
    }

    public void SetTimerText() {
        _timerRoundText.text = GameManager.Instance.TimerRound.ToString("00.00");
    }

    public void UpdateRoundText() {
        _roundIdText.text = "Round " + (GameManager.Instance.GetActiveIdRound() + 1); 
    }

    public void UpdateCoinText() {

    }
}
