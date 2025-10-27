using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grave : MonoBehaviour, IInteractable
{
    public GameObject interactionIcon;
    public bool IsTouched = false;
    public bool IsInRange = false;

    public Animator transition;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && IsInRange == true && CanInteract())
        {
            Interact();
        }
    }
    public bool CanInteract()
    {
        return !IsTouched;
    }


    public void Interact()
    {
        if (!CanInteract()) return;
        IsTouched = true;
        TouchGrave();
    }

    private void TouchGrave()
    {
        interactionIcon.SetActive(false);
        StartCoroutine(LoadCredits());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && CanInteract())
        {
            interactionIcon.SetActive(true);
            IsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CanInteract())
        {
            interactionIcon.SetActive(false);
            IsInRange = false;
        }
    }

    IEnumerator LoadCredits()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        transition.Rebind();

        yield return new WaitForSeconds(1);
    }
}
