using RecruProject.Core.Validators;

namespace RecruProject.Infrastructure.Validators;

public class OrderValidator : IOrderValidator
{
    public bool IsValid(int orderId) => orderId > 0;
}