using Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPicker : MonoBehaviour
{
    [SerializeField] private ActionPickEntry _actionPick_InplacePrefab;
    private List<ActionPickEntry> activeAPEs = new List<ActionPickEntry>();

    public void SetupActionPicker(List<ICommand> commands)
    {
        foreach (var command in commands)
        {
            var ape = Instantiate(_actionPick_InplacePrefab, transform);

            ape.Setup(() => CleanAPEs(), Enum.GetName(typeof(CommandId), command.CommandId));

            activeAPEs.Add(ape);

            ape.gameObject.SetActive(true);
        }
    }

    public void CleanAPEs()
    {
        foreach (var ape in activeAPEs)
        {
            Destroy(ape);
        }
        activeAPEs.Clear();
    }
}
