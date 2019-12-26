using System.Collections.Generic;

namespace SearchFile
{
    public interface ISearcher
    {
        IList<DynamicClass> Search(string fieldName, string term);

        IList<DynamicClass> Search<NumberType>(string fieldName, NumberType min, NumberType max);
    }
}