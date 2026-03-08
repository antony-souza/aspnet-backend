namespace Backend.Interfaces;

public interface IBaseReturnPagination<T>
{
    int Total { get; }
    int Page { get; }
    int PerPage { get; }
    int CountPage { get; }
    List<T> Items { get; }
}