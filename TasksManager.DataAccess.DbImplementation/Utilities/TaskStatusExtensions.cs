using System;

using VMTS = TasksManager.ViewModels.TaskStatus;
using ETS = TasksManager.Entities.TaskStatus;

namespace TasksManager.DataAccess.DbImplementation.Utilities
{
    internal static class TaskStatusExtensions
    {
        internal static VMTS Convert(this ETS status)
        {
            switch (status)
            {
                case ETS.New:
                    return VMTS.New;
                case ETS.InProgress:
                    return VMTS.InProgress;
                case ETS.Posponded:
                    return VMTS.Posponded;
                case ETS.Completed:
                    return VMTS.Completed;
                default:
                    throw new ArgumentException();
            }
        }

        internal static ETS Convert(this VMTS status)
        {
            switch (status)
            {
                case VMTS.New:
                    return ETS.New;
                case VMTS.InProgress:
                    return ETS.InProgress;
                case VMTS.Posponded:
                    return ETS.Posponded;
                case VMTS.Completed:
                    return ETS.Completed;
                default:
                    throw new ArgumentException();
            }
        }

        internal static bool Equals(this VMTS first, ETS second)
        {
            return first == second.Convert();
        }

        internal static bool Equals(this ETS first, VMTS second)
        {
            return first == second.Convert();
        }
    }
}
