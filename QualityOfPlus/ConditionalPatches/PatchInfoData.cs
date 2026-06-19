using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace QualityOfPlus.ConditionalPatches
{
    class PatchInfoData
    {
        public Type TargetType { get; private set; }
        public string TargetMethod { get; private set; }
        public Type[] ArgumentTypes { get; private set; }

        public bool TargetTypeIsNull => TargetType == null;
        public bool TargetMethodIsNull => TargetMethod.IsNullOrWhiteSpace();
        public bool ArgumentTypesAreNull => ArgumentTypes == null;

        public void Merge(HarmonyPatch patch, string memberName)
        {
            if (patch?.info == null)
                throw new ArgumentException($"Member {memberName} has not supported patch declaration!");

            if (patch.info.declaringType != null)
            {
                if (!TargetTypeIsNull)
                    throw new InvalidOperationException($"Member {memberName} has two target types to patch!");

                TargetType = patch.info.declaringType;
            }

            if (patch.info.methodName != null)
            {
                if (!TargetMethodIsNull)
                    throw new InvalidOperationException($"Member {memberName} has two target methods to patch!");

                TargetMethod = patch.info.methodName;
            }

            if (patch.info.argumentTypes != null)
            {
                if (!ArgumentTypesAreNull)
                    throw new InvalidOperationException($"Member {memberName} has two argument types arrays!");

                ArgumentTypes = patch.info.argumentTypes;
            }
        }

        public MethodInfo GetMethod() => AccessTools.Method(TargetType, TargetMethod, ArgumentTypes);

        public static PatchInfoData Merge(PatchInfoData first, PatchInfoData second, string memberName)
        {
            if (first == null && second == null)
                throw new ArgumentException($"Both patches are null!");

            if (first == null)
                return second;

            if (second == null)
                return first;

            PatchInfoData result = new PatchInfoData();

            if (first.TargetType != null && second.TargetType != null)
                throw new InvalidOperationException($"Member {memberName} has two target types to patch!");

            result.TargetType = first.TargetType ?? second.TargetType;

            if (!first.TargetMethodIsNull && !second.TargetMethodIsNull)
                throw new InvalidOperationException($"Member {memberName} has two target methods to patch!");

            result.TargetMethod = first.TargetMethod ?? second.TargetMethod;

            if (!first.ArgumentTypesAreNull && !second.ArgumentTypesAreNull)
                throw new InvalidOperationException($"Member {memberName} has two argument types arrays!");

            result.ArgumentTypes = first.ArgumentTypes ?? second.ArgumentTypes;

            return result;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("PatchInfoData { ");

            sb.Append($"TargetType: {(TargetType != null ? TargetType.FullName : "null")}, ");

            sb.Append($"TargetMethod: {(!TargetMethodIsNull ? $"\"{TargetMethod}\"" : "null")}, ");

            sb.Append("ArgumentTypes: ");
            if (ArgumentTypes != null)
            {
                sb.Append("[");
                List<string> argTypeNames = new List<string>();
                foreach (var t in ArgumentTypes)
                {
                    argTypeNames.Add(t?.FullName ?? "null");
                }
                sb.Append(string.Join(", ", argTypeNames));
                sb.Append("]");
            }
            else
            {
                sb.Append("null");
            }

            sb.Append(" }");
            return sb.ToString();
        }
    }
}
