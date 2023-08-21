using Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ActionPicker : MonoBehaviour
{
    [SerializeField] private ActionPickEntry _actionPick_InplacePrefab;
    private List<ActionPickEntry> activeAPEs = new List<ActionPickEntry>();

    private AutoResetEvent actionPickedEvent = new AutoResetEvent(false);

    public async Task<ICommand> SetupActionPicker(List<ICommand> commands)
    {
        ICommand pickedCommand = null;
    
        foreach (var command in commands)
        {
            actionPickedEvent = new AutoResetEvent(false);
            var ape = Instantiate(_actionPick_InplacePrefab, transform);
            ape.Setup(
                () => {
                    CleanAPEs();
                    pickedCommand = command;
                    actionPickedEvent.Set();
                }, Enum.GetName(typeof(CommandId), command.CommandId));

            activeAPEs.Add(ape);

            ape.gameObject.SetActive(true);
        }

        await Task.Run(() => { actionPickedEvent.WaitOne(); });
        
        CleanAPEs();
        return pickedCommand;

    }

    public void CleanAPEs()
    {
        foreach (var ape in activeAPEs)
        {
            Destroy(ape.gameObject);
        }
        activeAPEs.Clear();
    }
}
