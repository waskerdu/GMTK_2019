using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageTutorial
{
    public static void Send(string message)
    {
        GameObject obj = GameObject.Find("TutorialManager");
        if (obj != null) obj.SendMessage("FinishStage",message);
    }
    public static bool InTutorial()
    {
        GameObject obj = GameObject.Find("SceneManager");
        return obj.GetComponent<GameManager>().inTutorial;
    }
}


public class TutorialManager : MonoBehaviour
{

    /*
     Manages the tutorial system
     A tutorial is made up of several stages. Each stage has several objects it can awaken.
     These might include ui elements or other scene objects. 
    */
    [System.Serializable]
    public class TutorialStage
    {
        public string name;
        public GameObject[] objects;
        
        public void Wake()
        {
            foreach (var item in objects)
            {
                item.SetActive(true);
            }
        }
        public void Sleep()
        {
            foreach (var item in objects)
            {
                item.SetActive(false);
            }
        }
    }


    public TutorialStage[] stages;

    public int currentStageIndex = 0;

    public GameObject sceneManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) FinishStage(stages[currentStageIndex].name);
    }

    public void FinishStage(string stageName)
    {
        if (currentStageIndex < stages.Length && stageName == stages[currentStageIndex].name)
        {
            stages[currentStageIndex].Sleep();
            currentStageIndex++;
            if (currentStageIndex < stages.Length) stages[currentStageIndex].Wake();
            else sceneManager.SendMessage("TutorialOver");
        }
    }

    public void StartTutorial() { transform.GetChild(0).GetChild(0).gameObject.SetActive(true); }
}
