namespace StudentRecords.DTOs
{
    public class PaginationSortParameters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortAttribute { get; set; }
        public string SortOrder { get; set; }
        public string SearchName { get; set; }
    }
}
