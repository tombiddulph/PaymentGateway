using System;

namespace PaymentGateway.Application.Models
{
    public interface IModelId
    {
        Guid Id { get; set; }
    }
}