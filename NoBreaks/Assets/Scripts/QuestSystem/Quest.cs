using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Quest
{
    public QuestInfoSO questInfo;

    public QuestState questState;

    private int currentStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        this.questInfo = questInfo;
        this.questState = QuestState.CANNOT_START;
        this.currentStepIndex = 0;
    }
    public void MoveToNextStep()
    {
       currentStepIndex++;
    }

    public bool NextStepAvailable()
    {
        return currentStepIndex < questInfo.questStepPrefabs.Length;
    }

    public void instantiateCurrentStep(Transform parent)
    {
        GameObject currentStepPrefab = GetCurrentQuestPrefab();
        if (currentStepPrefab != null)
        {
            Object.Instantiate<GameObject>(currentStepPrefab, parent);
        }
    }

    private GameObject GetCurrentQuestPrefab()
    {
        GameObject currentStepPrefab = null;
        if (NextStepAvailable())
        {
           currentStepPrefab = questInfo.questStepPrefabs[currentStepIndex];
        }
        return currentStepPrefab;
    }
}
