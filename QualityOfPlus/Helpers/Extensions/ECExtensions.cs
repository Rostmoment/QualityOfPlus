using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityOfPlus.Helpers.Extensions
{
    internal static class ECExtensions
    {
        public static int GetElevatorsCount(this EnvironmentController ec) => ec.ElevatorManager.Elevators.Count;
        public static int GetTotalOutOfOrderElevators(this EnvironmentController ec) => ec.ElevatorManager.TotalOutOfOrderElevators;
        public static int GetOutOfElevatorsCount(this EnvironmentController ec) => ec.ElevatorManager.Elevators.Count(x => x.CurrentState == ElevatorState.OutOfOrder);
    }
}
