
public class PaginationDTO<T> where T : class
{
    public int Pages { get; set; }
    public int NumOfItems { get; set; }
    public IEnumerable<T> Data { get; set; } = new List<T>();
}