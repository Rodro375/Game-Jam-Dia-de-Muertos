using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] int _health;
    [SerializeField] int _saltos;
    [SerializeField] TextMeshProUGUI _vida;
    [SerializeField] TextMeshProUGUI _salto;

    PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _vida.text = "VIDA: " + _health;
        _salto.text = "SALTOS: " + _saltos;

        _playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int damage) { _health -= damage; _vida.text = "VIDA: " + _health; }

    public void CantJump(int salto) { _saltos -= salto; _salto.text = "SALTOS: " + _saltos; }

    public void SetCantJump(int salto) { _saltos = salto; _salto.text = "SALTOS: " + _saltos;}

}
