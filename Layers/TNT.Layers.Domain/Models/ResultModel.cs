using System.Collections.Generic;

namespace TNT.Layers.Domain.Models
{
    public class ResultModel
    {
        public ResultModel(object data,
            string code = ResultCodes.ObjectResult,
            IEnumerable<string> messages = null)
        {
            Data = data;
            Code = code;
            Messages = messages;
        }

        public object Data { get; }
        public string Code { get; }
        public IEnumerable<string> Messages { get; }
    }

    public class ResultModel<T> : ResultModel
    {
        public ResultModel(T data,
            string code = ResultCodes.ObjectResult,
            IEnumerable<string> messages = null) : base(data, code, messages)
        {
            Data = data;
        }

        public new T Data { get; }
    }
}