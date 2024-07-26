using System.Collections.Generic;

namespace TNT.Layers.Application.Models
{
    public class ListResponseModel<T>
    {
        public ListResponseModel(int total, IEnumerable<T> list)
        {
            Total = total;
            List = list;
        }

        public int Total { get; set; }
        public IEnumerable<T> List { get; set; }
    }
}
