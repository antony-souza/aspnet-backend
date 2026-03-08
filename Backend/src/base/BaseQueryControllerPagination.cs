namespace Backend.Base;

public class BaseQueryControllerPagination
{
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}