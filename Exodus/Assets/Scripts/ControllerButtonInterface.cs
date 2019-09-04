using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerButtonInterface : MonoBehaviour
{
    [SerializeField] List<GameObject> buttons;
    [SerializeField] int defaultButton = 0;
    [SerializeField] float selectionDelay = 0.5f;
    [SerializeField] float deadZone = 0.2f;
    float selectionChangeTimer;
    int currentButton;
    bool noSelection = true;
    EventSystem eventSystem;

    // Start is called before the first frame update
    private void OnEnable()
    {
        noSelection = true;
        eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.SetSelectedGameObject(null);
        currentButton = defaultButton;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selectionChangeTimer > 0)
        {
            selectionChangeTimer -= Time.deltaTime;
        }
        else if (Input.GetAxisRaw("Horizontal") > deadZone ||  Input.GetAxisRaw("Vertical") < -deadZone)
        {
            selectionChangeTimer = selectionDelay;
            if (!noSelection)
            {
                if (buttons.Count == currentButton + 1)
                {
                    currentButton = 0;
                }
                else
                {
                    currentButton++;
                }
            }
                eventSystem.SetSelectedGameObject(buttons[currentButton]);
            //buttons[currentButton].GetComponent<Button>().OnPointerEnter(null);
            noSelection = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < -deadZone || Input.GetAxisRaw("Vertical") > deadZone)
        {
            selectionChangeTimer = selectionDelay;
            if (!noSelection)
            {
                //buttons[currentButton].GetComponent<Button>().OnPointerExit(null);
                if (currentButton == 0)
                {
                    currentButton = buttons.Count - 1;
                }
                else
                {
                    currentButton--;
                }
            }
            eventSystem.SetSelectedGameObject(buttons[currentButton]);
            //buttons[currentButton].GetComponent<Button>().OnPointerEnter(null);
            noSelection = false;
        }
    }
}
