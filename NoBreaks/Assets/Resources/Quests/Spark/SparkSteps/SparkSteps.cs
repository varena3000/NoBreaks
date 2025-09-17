using UnityEngine;

public class SparkSteps : QuestStep
{
    private bool objectiveCompleted = false;

    private void OnEnable()
    {
        FPController.OnFirstCrouch += MoveComplete;
        FPController.OnFirstMove += MoveComplete;
    }

    private void OnDisable()
    {
        FPController.OnFirstCrouch -= MoveComplete;
        FPController.OnFirstMove -= MoveComplete;
    }

    private void MoveComplete()
    {
        if (!objectiveCompleted)
        {
            objectiveCompleted = true;
            CompleteStep();
        }
    }
    
      
    
}
