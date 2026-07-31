using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace QualityOfPlus.Helpers
{
    public static class UnityEventDebugger
    {
        /// <summary>
        /// Converts a <see cref="UnityEventBase"/> to a pseudocode string
        /// showing each persistent call in order — target, method, arguments and whether it is static.
        /// </summary>
        public static string ToPseudocode(UnityEventBase unityEvent)
        {
            if (unityEvent == null)
                return "(null UnityEvent)";

            StringBuilder sb = new StringBuilder();

            object persistentCallGroup = GetPrivateField(unityEvent, "m_PersistentCalls");
            if (persistentCallGroup == null)
                return "(could not read m_PersistentCalls)";

            System.Collections.IList calls =
                GetPrivateField(persistentCallGroup, "m_Calls") as System.Collections.IList;

            if (calls == null || calls.Count == 0)
                return "(no persistent calls)";

            for (int i = 0; i < calls.Count; i++)
            {
                object call = calls[i];
                if (call == null) continue;

                AppendCall(sb, i, call);
            }

            return sb.ToString().TrimEnd();
        }

        private static void AppendCall(StringBuilder sb, int index, object call)
        {
            //   m_Target        - UnityEngine.Object (null if static)
            //   m_TargetAssemblyTypeName — type name for static calls
            //   m_MethodName    - string
            //   m_Mode          - PersistentListenerMode enum
            //   m_Arguments     - ArgumentCache (holds one argument of any supported type)
            //   m_CallState     - UnityEventCallState (Off / EditorAndRuntime / RuntimeOnly)

            UnityEngine.Object target = GetPrivateField(call, "m_Target") as UnityEngine.Object;
            string typeName = GetPrivateField(call, "m_TargetAssemblyTypeName") as string ?? "";
            string methodName = GetPrivateField(call, "m_MethodName") as string ?? "?";
            object callState = GetPrivateField(call, "m_CallState");
            object mode = GetPrivateField(call, "m_Mode");
            object arguments = GetPrivateField(call, "m_Arguments");

            bool isStatic = target == null;
            string targetLabel = isStatic
                ? ExtractTypeName(typeName)
                : $"{target.name} ({target.GetType().Name})";

            string argString = FormatArgument(mode, arguments);
            string stateLabel = callState != null ? $" [{callState}]" : "";

            sb.AppendLine($"[{index}]{stateLabel} {(isStatic ? "static " : "")}{targetLabel}.{methodName}({argString})");
        }

        private static string FormatArgument(object mode, object arguments)
        {
            if (mode == null || arguments == null)
                return "";

            //   0 = EventDefined (no persistent arg, uses runtime arg)
            //   1 = Void
            //   2 = Object   - m_ObjectArgument
            //   3 = Int      - m_IntArgument
            //   4 = Float    - m_FloatArgument
            //   5 = String   - m_StringArgument
            //   6 = Bool     - m_BoolArgument
            int modeInt = Convert.ToInt32(mode);
            switch (modeInt)
            {
                case 0: return "<runtime arg>";
                case 1: return "";
                case 2:
                    UnityEngine.Object obj = GetPrivateField(arguments, "m_ObjectArgument") as UnityEngine.Object;
                    string objTypeName = GetPrivateField(arguments, "m_ObjectArgumentAssemblyTypeName") as string ?? "";
                    return obj != null
                        ? $"{obj.name} ({ExtractTypeName(objTypeName)})"
                        : $"null ({ExtractTypeName(objTypeName)})";
                case 3:
                    return GetPrivateField(arguments, "m_IntArgument")?.ToString() ?? "0";
                case 4:
                    return GetPrivateField(arguments, "m_FloatArgument")?.ToString() ?? "0";
                case 5:
                    return $"\"{GetPrivateField(arguments, "m_StringArgument") ?? ""}\"";
                case 6:
                    return GetPrivateField(arguments, "m_BoolArgument")?.ToString() ?? "false";
                default:
                    return $"<unknown mode {modeInt}>";
            }
        }

        private static string ExtractTypeName(string assemblyQualifiedName)
        {
            if (string.IsNullOrEmpty(assemblyQualifiedName))
                return "?";

            int comma = assemblyQualifiedName.IndexOf(',');
            string fullName = comma >= 0
                ? assemblyQualifiedName.Substring(0, comma).Trim()
                : assemblyQualifiedName.Trim();

            int dot = fullName.LastIndexOf('.');
            return dot >= 0 ? fullName.Substring(dot + 1) : fullName;
        }

        private static object GetPrivateField(object obj, string fieldName)
        {
            if (obj == null) return null;

            Type type = obj.GetType();
            while (type != null)
            {
                FieldInfo field = type.GetField(fieldName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (field != null)
                    return field.GetValue(obj);

                type = type.BaseType;
            }

            return null;
        }
    }
}