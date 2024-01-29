
using Base.Common.Event;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;

namespace Base.Common.Command
{
    /// <summary> Handle request trả về dạng List </summary>
    /// Author: trinhtx
    public abstract class RequestList<DataType> : IRequest<List<DataType>>
    {
        public abstract bool IsValid();

        public ValidationResult ValidationResult { get; set; }
    }

    /// <summary> Handle request trả về dạng Dictionary </summary>
    /// Author: trinhtx
    public abstract class RequestDictionary<KeyType, ValueType> : IRequest<Dictionary<KeyType, ValueType>>
    {
        public abstract bool IsValid();

        public ValidationResult ValidationResult { get; set; }
    }

    /// <summary> Handle request trả về Data là Object bất kỳ </summary>
    /// Author: trinhtx
    public abstract class HandleRequest : IRequest<HandleResult>
    {
        public ValidationResult ValidationResult { get; set; }

        public abstract bool IsValid();
    }

    /// <summary> Handle request trả về Data theo kiểu chỉ định </summary>
    /// Author: trinhtx
    public abstract class HandleRequest<DataType> : IRequest<HandleResult<DataType>>
    {
        public ValidationResult ValidationResult { get; set; }

        public abstract bool IsValid();
    }

    /// <summary> Handle request trả về Data dạng List </summary>
    /// Author: trinhtx
    public abstract class HandleRequestList<DataType> : HandleRequest<List<DataType>>
    {
    }

    /// <summary> Handle request trả về Data dạng Dictionary </summary>
    /// Author: trinhtx
    public abstract class HandleRequestDictionary<KeyType, ValueType> : HandleRequest<Dictionary<KeyType, ValueType>>
    {
    }
}