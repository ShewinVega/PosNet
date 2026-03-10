using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosNet.UseCases.Dtos.Pagination
{
    public class PaginationDto
    {

        public int Page {  get; set; }
        public int PageSize { get; set; }
    }
}
