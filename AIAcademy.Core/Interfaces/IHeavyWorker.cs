using AIAcademy.Core;
using AIAcademy.Core.Models;

public interface IHeavyWorker
{
    Task<WorkResult> DoHeavyWorkAsync(
        RobotSnapshot snapshot, int workload, CancellationToken token);
}
