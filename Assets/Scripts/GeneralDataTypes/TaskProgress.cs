using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskProgress<T>
{
    public readonly T task;
    public Progress progress;

    public TaskProgress(T task)
    {
        this.task = task;
        progress = Progress.PENDING;
    }

    public static List<TaskProgress<T>> GenerateTaskProgressList()
    {
        List<TaskProgress<T>> taskList = new();
        foreach (T task in Enum.GetValues(typeof(T)))
        {
            TaskProgress<T> taskProgress = new(task);
            taskList.Add(taskProgress);
        }
        return taskList;
    }

    public static TaskProgress<T> GetCurrentTaskProgress(List<TaskProgress<T>> taskList)
    {
        foreach (TaskProgress<T> taskProgress in taskList)
        {
            if (taskProgress.progress == Progress.PENDING || taskProgress.progress == Progress.DOING)
            {
                return taskProgress;
            }
        }

        return null;
    }

    public void StartedTask()
    {
        progress = Progress.DOING;
    }

    public void FinishedTask()
    {
        progress = Progress.FINISHED;
    }
}
