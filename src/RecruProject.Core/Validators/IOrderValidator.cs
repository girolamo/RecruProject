namespace RecruProject.Core.Validators;

public interface IOrderValidator
{
    bool IsValid(int orderId);
}