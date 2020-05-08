using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Mediador2
{
    public class Mediador : IMediador
    {
        public static Dictionary<Type, Type> HandlerCache = new Dictionary<Type, Type>();
        public static Dictionary<Type, Type[]> ValidatorCache = new Dictionary<Type, Type[]>();
        public static Dictionary<Type, Type[]> NotificationCache = new Dictionary<Type, Type[]>();

        public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = GetHandlerOf<ICommandHandler<TCommand>>();
            var validators = GetValidatorsOf<ICommandValidator<TCommand>>();
            var valid = await ApplyCommandValidators(validators, command);
            if (!valid) throw new Exception("Command object failed validation");
            await handler.Handle(command);
        }
        public async Task<TResult> Send<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            var handler = GetHandlerOf<ICommandHandler<TCommand, TResult>>();
            var validators = GetValidatorsOf<ICommandValidator<TCommand, TResult>>();
            var valid = await ApplyCommandValidators(validators, command);
            if (!valid) throw new Exception("Command object failed validation");
            return await handler.Handle(command);
        }
        public async Task NotifyAll<TNotification>(TNotification notification) where TNotification : INotification
        {
            var listeners = GetNotificationHandlers<INotificationListener<TNotification>>();
            await Task.WhenAll(listeners.Select(item => item.OnNotification(notification)).ToArray());
        }

        #region Private Methods       
        private TResult GetHandlerOf<TResult>()
        {
            var type = typeof(TResult);
            Type handlerType = null;

            if (HandlerCache.ContainsKey(type)) handlerType = HandlerCache[type];

            else
            {
                var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());
                var handlers = allTypes.Where(t =>
                                                type.IsAssignableFrom(t) &&
                                                t.IsClass &&
                                                !t.IsAbstract).ToArray();
                switch (handlers.Count())
                {
                    case 0: throw new Exception("Handler not found for:" + type.GenericTypeArguments[0]?.FullName);
                    case 1:
                        HandlerCache.Add(type, handlers.First());
                        handlerType = handlers.First();
                        break;
                    default: throw new Exception("More than one handler found for:" + type.GenericTypeArguments[0]?.FullName);
                };
            }
            return (TResult)Activator.CreateInstance(handlerType);
        }
        private TListeners[] GetNotificationHandlers<TListeners>()
        {
            var type = typeof(TListeners);
            Type[] listeners = null;

            if (ValidatorCache.ContainsKey(type)) listeners = NotificationCache[type];

            else
            {
                var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());
                listeners = allTypes.Where(t =>
                                                   type.IsAssignableFrom(t) &&
                                                   t.IsClass &&
                                                   !t.IsAbstract).ToArray();
                NotificationCache.Add(type, listeners);
            }

            return listeners.Select(x => (TListeners)Activator.CreateInstance(x)).ToArray();
        }
        private TValidator[] GetValidatorsOf<TValidator>()
        {
            var type = typeof(TValidator);
            Type[] validatorTypes = null;

            if (ValidatorCache.ContainsKey(type)) validatorTypes = ValidatorCache[type];

            else
            {
                var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());
                validatorTypes = allTypes.Where(t =>
                                                   type.IsAssignableFrom(t) &&
                                                   t.IsClass &&
                                                   !t.IsAbstract).ToArray();
                ValidatorCache.Add(type, validatorTypes);
            }

            return validatorTypes.Select(x => (TValidator)Activator.CreateInstance(x)).ToArray();
        }
        private static async Task<bool> ApplyCommandValidators<TCommand>(
            ICommandValidator<TCommand>[] validators,
            TCommand command)
            where TCommand : ICommand
        {
            var validationResults = await Task.WhenAll(validators.Select(item => item.Validate(command)).ToArray());
            var valid = validationResults.Aggregate(true, (result, item) => result && item);
            return valid;
        }
        private static async Task<bool> ApplyCommandValidators<TCommand, TResult>(
            ICommandValidator<TCommand, TResult>[] validators,
            TCommand command)
            where TCommand : ICommand<TResult>
        {
            var validationResults = await Task.WhenAll(validators.Select(item => item.Validate(command)).ToArray());
            var valid = validationResults.Aggregate(true, (result, item) => result && item);
            return valid;
        }
        #endregion
    }
}
