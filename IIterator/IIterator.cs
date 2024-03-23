namespace Web_BanXeMoTo.IIterator
{
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }
}
