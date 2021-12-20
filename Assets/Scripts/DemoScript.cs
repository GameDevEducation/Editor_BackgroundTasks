using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Threading.Tasks;
using Unity.EditorCoroutines.Editor;
#endif

public class DemoScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#if UNITY_EDITOR
    /*
     * This will block everything until it completes as it runs synchronously in the main thread
     */
    public void PerformBackgroundTask()
    {
        int progressID = Progress.Start("Custom Background Task");

        for (int sample = 0; sample < 10; ++sample)
        {
            Progress.Report(progressID, sample + 1, 10, $"Processing sample {(sample + 1)}");
            System.Threading.Thread.Sleep(2000);
        }

        Progress.Remove(progressID);
    }

    /*
     * This will run in a different thread using C# tasks. We must be cautious to only perform thread safe actions
     * Many aspects of Unity (eg. terrains) cannot be accessed from another thread. 
     * Access in this case means both reading and writing.
     */
    public async Task PerformCSharpAsync_Task()
    {
        int progressID = Progress.Start("C# Async Task");

        await Task.Run(async () =>
        {
            for (int sample = 0; sample < 10; ++sample)
            {
                Progress.Report(progressID, sample + 1, 10, $"Processing sample {(sample + 1)}");
                await Task.Delay(2000);
            }
        });

        Progress.Remove(progressID);
    }

    public void PerformEditorCoroutine_Task()
    {
        EditorCoroutineUtility.StartCoroutineOwnerless(PerformEditorCoroutine_Task_Internal());
    }

    /*
     * Editor Coroutines allow us to run things in the background but run in the main thread.
     * We can safely access things like terrains from an editor coroutine.
     */
    IEnumerator PerformEditorCoroutine_Task_Internal()
    {
        int progressID = Progress.Start("Editor Coroutine Task");

        for (int sample = 0; sample < 10; ++sample)
        {
            Progress.Report(progressID, sample + 1, 10, $"Processing sample {(sample + 1)}");

            // For an editor coroutine we must use EditorWaitForSeconds not WaitForSeconds
            // WaitForSeconds will complete almost immediately regardless of the time set
            yield return new EditorWaitForSeconds(2f);
        }

        Progress.Remove(progressID);
    }

    public void PerformCompoundTask()
    {
        EditorCoroutineUtility.StartCoroutineOwnerless(PerformCompoundTask_Internal());
    }

    IEnumerator PerformCompoundTask_Internal()
    {
        int progressID = Progress.Start("Compound Task", null, Progress.Options.Sticky);

        Progress.Report(progressID, 1, 3);
        yield return PerformCompoundTask_Child(progressID);

        Progress.Report(progressID, 2, 3);
        yield return PerformCompoundTask_Child(progressID);

        Progress.Report(progressID, 3, 3);
        yield return PerformCompoundTask_Child(progressID);

        Progress.Finish(progressID, Progress.Status.Succeeded);
    }

    IEnumerator PerformCompoundTask_Child(int parentID)
    {
        int progressID = Progress.Start("Child task", null, Progress.Options.Sticky, parentID);

        for (int index = 0; index < 100; ++index)
        {
            Progress.Report(progressID, (float)(index + 1) / 100f);
            yield return new EditorWaitForSeconds(0.05f);
        }

        Progress.Finish(progressID, Progress.Status.Succeeded);
    }
}
#endif

