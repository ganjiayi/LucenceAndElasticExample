using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SearchFile
{
    public class DynamicClass : DynamicObject
    {
        private readonly Dictionary<string, KeyValuePair<Type, object>> _fields;

        public DynamicClass(IList<Field> fields)
        {
            _fields = new Dictionary<string, KeyValuePair<Type, object>>();

            foreach (var field in fields)
            {
                _fields.Add(field.FieldName, new KeyValuePair<Type, object>(field.FieldType, null));
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_fields.ContainsKey(binder.Name))
            {
                var type = _fields[binder.Name].Key;
                if (value.GetType() == type)
                {
                    _fields[binder.Name] = new KeyValuePair<Type, object>(type, value);
                    return true;
                }
            }
            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _fields[binder.Name].Value;
            return true;
        }

        public override string ToString()
        {
            return string.Join("\n", _fields.Select(field => $"{field.Key}: {field.Value.Value}"));
        }
    }
}