using System.Collections.Generic;

namespace TNT.Layers.Domain.Models
{
    public class ResultModel
    {
        public ResultModel(object data,
            string code = ResultCodes.ObjectResult,
            IEnumerable<string> details = null)
        {
            Data = data;
            Code = code;
            Details = details;
        }

        public object Data { get; }
        public string Code { get; }
        public IEnumerable<string> Details { get; }
    }

    public class ResultModel<T> : ResultModel
    {
        public ResultModel(T data,
            string code = ResultCodes.ObjectResult,
            IEnumerable<string> details = null) : base(data, code, details)
        {
            Data = data;
        }

        public new T Data { get; }
    }
}