using System.Collections;

public interface IWorkStation
{
    void HireWorker(Worker worker);
    void FireWorker(Worker worker);

    IEnumerator WorkerTaskCoroutine();
    void PerformWorkerTask();
}