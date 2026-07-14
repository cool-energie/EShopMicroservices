namespace CatalogAPI.Products.DeleteProduct
{
    // public record DeleteProductRequest(Guid id);
    public record DeleteProductResponse(bool isSuccess);


    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var result = sender.Send(new DeleteProductCommand(id));
                var response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            });
        }
    }
}
