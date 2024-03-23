using System.Collections.Generic;

public interface ISearchDecorator<T>
{
    IEnumerable<T> Search(string tuKhoa);
}