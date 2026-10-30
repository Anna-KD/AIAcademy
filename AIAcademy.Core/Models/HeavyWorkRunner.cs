using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AIAcademy.Core.Interfaces; // если WorkResult лежит в другом namespace, поправь using

namespace AIAcademy.Core.Models
{
    public static class HeavyWorkRunner
    {
        public static Task<WorkResult> RunAsync(RobotSnapshot snapshot, Func<double> calculation)
        {
            // Токен в Task.Run специально не передаём: задача всегда запускается
            // и сама вернёт статус Cancelled, а не бросит TaskCanceledException.
            return Task.Run(() =>
            {
                var watch = Stopwatch.StartNew();
                try
                {
                    double value = calculation();
                    return new WorkResult
                    {
                        RobotName = snapshot.Name,
                        Status = "Success",
                        Result = value.ToString("N0"),
                        DurationMs = watch.ElapsedMilliseconds
                    };
                }
                catch (OperationCanceledException)
                {
                    return new WorkResult
                    {
                        RobotName = snapshot.Name,
                        Status = "Cancelled",
                        Result = "",
                        DurationMs = watch.ElapsedMilliseconds
                    };
                }
                catch (Exception ex)
                {
                    return new WorkResult
                    {
                        RobotName = snapshot.Name,
                        Status = "Error",
                        ErrorMessage = ex.Message,
                        DurationMs = watch.ElapsedMilliseconds
                    };
                }
            });
        }
    }
}
