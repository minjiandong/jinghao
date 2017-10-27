using System;
using System.Collections.Generic;
using System.Reflection;
/*
*/

namespace DBUtility
{
    public class Mapping
    {
        internal static string GetTableName(object mappingClassObject)
        {
            Type type = mappingClassObject.GetType();
            return (string)type.InvokeMember("GetTableName", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, mappingClassObject, new object[0]);
        }
        internal static Dictionary<string, string> GetIdentityMapping(object mappingClassObject)
        {
            Type type = mappingClassObject.GetType();
            return (Dictionary<string, string>)type.InvokeMember("GetIdentityMapping", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, mappingClassObject, new object[0]);
        }
        internal static Dictionary<string, string> GetBaseFieldMapping(object mappingClassObject)
        {
            Type type = mappingClassObject.GetType();
            return (Dictionary<string, string>)type.InvokeMember("GetBaseFieldMapping", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, mappingClassObject, new object[0]);
        }
        internal static Dictionary<string, string> GetAllFieldMapping(object mappingClassObject)
        {
            Dictionary<string, string> identityMapping = Mapping.GetIdentityMapping(mappingClassObject);
            Dictionary<string, string> baseFieldMapping = Mapping.GetBaseFieldMapping(mappingClassObject);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string current in baseFieldMapping.Keys)
            {
                dictionary.Add(current, baseFieldMapping[current]);
            }
            baseFieldMapping.Clear();
            foreach (string current in identityMapping.Keys)
            {
                baseFieldMapping.Add(current, identityMapping[current]);
            }
            foreach (string current in dictionary.Keys)
            {
                if (!baseFieldMapping.ContainsKey(dictionary[current]))
                    baseFieldMapping.Add(current, dictionary[current]);
            }
            return baseFieldMapping;
        }
    }
}
