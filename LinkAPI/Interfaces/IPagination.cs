namespace LinkAPI.Interfaces;

public interface IPagination<out T> where T : class
{
    IEnumerable<T> GetPaged(int pageNumber, int pageSize);
    
    int Count();
}