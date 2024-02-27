namespace StudentRecords.DTOs
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResult(IEnumerable<T> data, int totalPages)
        {
            Data = data;
            TotalPages = totalPages;
        }
    }

}
