using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Mediador3
{
    public class Mediador : IMediador
    {
        private readonly MediadorCache cache;

        private Mediador(MediadorCache cache) => this.cache = cache;
        
        public async Task SendASync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handlerType = await this.ApplyValidationAndReturnHandlerForCommandAsync(command).ConfigureAwait(false);
            var handler = FN.Instanciate<ICommandHandler<TCommand>>(handlerType);
            await handler.Handle(command).ConfigureAwait(false);
        }
        public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            var handlerType = await this.ApplyValidationAndReturnHandlerForCommandAsync(command).ConfigureAwait(false);
            var handler = FN.Instanciate<ICommandHandler<TCommand, TResult>>(handlerType);
            return await handler.Handle(command).ConfigureAwait(false);
        }
        public async Task NotifyAllAsync<TNotification>(TNotification notification) where TNotification : INotification
        {
            var listenerTypes = await GetNotificationListenersAsync(notification).ConfigureAwait(false);
            var listeners = listenerTypes.Select(FN.Instanciate<INotificationListener<TNotification>>).ToArray();
            await Task.WhenAll(listeners.Select(item => item.OnNotification(notification)).ToArray()).ConfigureAwait(false);
        }

        private async Task<Type> ApplyValidationAndReturnHandlerForCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var type = typeof(TCommand);
            var cached = cache.GetHandlerAndValidatorsForCommand(type);
            var validators = cached.Validators.Select(FN.Instanciate<ICommandValidator<TCommand>>).ToArray();
            var valid = await ApplyCommandValidators(validators, command).ConfigureAwait(false);
            if (!valid) throw new Exception($"Command object of type {typeof(TCommand).FullName} failed validation");
            return cached.Handler;
        }
        private async Task<bool> ApplyCommandValidators<TCommand>(ICommandValidator<TCommand>[] validators, TCommand command)
            where TCommand : ICommand
        {
            var validationResults = await Task.WhenAll(validators.Select(item => item.Validate(command)).ToArray()).ConfigureAwait(false);
            return validationResults.Aggregate(true, FN.AndAgregator);
        }
        private async Task<IEnumerable<Type>> GetNotificationListenersAsync(INotification notification) 
        {
            var type = notification.GetType();
            var cached = cache.GetListenersForNotification(type);
            return await Task.FromResult(cached.Listeners).ConfigureAwait(false);
        }
        
        public static Mediador CreateInstance() => new Mediador(MediadorCache.GetInstance());
    }
}
