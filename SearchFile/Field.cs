using System;

namespace SearchFile
{
    public class Field
    {
        public Field(string name, Type type)
        {
            FieldName = name;
            FieldType = type;
        }

        public string FieldName { get; set; }

        public Type FieldType { get; set; }
    }
}