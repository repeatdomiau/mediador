using System.Threading.Tasks;

namespace Mediador
{
    public interface ICommand { }
    public interface ICommand<T> : ICommand { };
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> Handle(TCommand command);
    }
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
    public interface ICommandValidator<TCommand> where TCommand : ICommand
    {
        Task<bool> Validate(TCommand command);
    }
    public interface ICommandValidator<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<bool> Validate(TCommand command);
    }
    public interface IQuery<T> { };
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
    public interface IQueryValidator<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<bool> Validate(TQuery query);
    }
    public interface IMediador
    {
        Task<TResult> SendCommand<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;
        Task SendCommand<TCommand>(TCommand command) where TCommand : ICommand;
        Task<TResult> SendQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
