using System.Collections;

public interface IWorkStation
{
    void HireWorker(Worker worker);
    IEnumerator WorkerTaskCoroutine();
    void PerformWorkerTask();
}