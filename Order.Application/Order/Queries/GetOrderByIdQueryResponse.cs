namespace Order.Application.Order.Queries
{
    public record GetOrderByIdQueryResponse(int Id, string Name, int Quantity, decimal Price);
}