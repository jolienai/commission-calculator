namespace FCamara.Commission.Application.Shared;

public interface IRequest<TResponse> { }

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}

public class Mediator(IServiceProvider provider) : IMediator
{
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = provider.GetService(handlerType)!;

        if (handler == null)
            throw new InvalidOperationException($"Handler for {request.GetType().Name} not found");

        return await handler.HandleAsync((dynamic)request, cancellationToken);
    }
}