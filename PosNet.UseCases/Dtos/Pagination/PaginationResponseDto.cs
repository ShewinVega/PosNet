namespace PosNet.UseCases.Dtos.Pagination
{
    public class PaginationResponseDto<T>
    {
        public bool Success {  get; set; }
        public int CurrentPage { get; set; } 
        public int PageCount {  get; set; }
        public int PageSize {  get; set; }
        public int RowsCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public List<T> Data { get; set; }
    }
}
