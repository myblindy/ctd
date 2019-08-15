using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tests
{
    class Support
    {
        public static object Get(object val, string fieldname) =>
            val.GetType().GetProperty(fieldname, BindingFlags.Public | BindingFlags.Instance |BindingFlags.NonPublic)?.GetValue(val)
            ?? val.GetType().GetField(fieldname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(val);

    }
}
