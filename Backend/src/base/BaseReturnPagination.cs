using Backend.Interfaces;


public class BaseReturnPagination<T> : IBaseReturnPagination<T>
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public int Total {get; set;}
    public List<T> Items {get; set; }
    
    public int CountPage {get; private set;}

    public BaseReturnPagination(int page, int perPage, int total, List<T> items)
    {
         Page = page;
         PerPage = perPage;
         Total = total;
         Items = items;
         CountPage = (int)Math.Ceiling((double)total/perPage);
    }
}
