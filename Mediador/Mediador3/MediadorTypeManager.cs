using System;
using System.Collections.Generic;
using System.Linq;

namespace Mediador3
{
    internal class MediadorTypeManager
    {
        private IEnumerable<Type> Commands { get; set; }
        private IEnumerable<Type> Handlers { get; set; }
        private IEnumerable<Type> Validators { get; set; }

        private IEnumerable<Type> Notifications { get; set; }
        private IEnumerable<Type> Listeners { get; set; }

        public MediadorTypeManager()
        {
            var types = LoadFromAssemblies();

            Commands = types.Where(FN.ClassImplements(typeof(ICommand)));
            Handlers = types.Where(FN.ClassImplements(typeof(IHandler)));
            Validators = types.Where(FN.ClassImplements(typeof(IValidator)));

            Notifications = types.Where(FN.ClassImplements(typeof(INotification)));
            Listeners = types.Where(FN.ClassImplements(typeof(INotificationListener)));
        }

        private IEnumerable<Type> LoadFromAssemblies()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(FN.IsNotSystemAssembly)
                .SelectMany(s => s.GetTypes())
                .Where(FN.AND(FN.IsConcreteClass, FN.IsNotCompilerGenerated));
        }

        private Type GetHandlerForCommand(Type command, Type commandReturnType = null)
        {
            var genericParams = (new[] { command, commandReturnType }).Where(FN.IsNotNull).ToArray();

            Type handlerInterface = commandReturnType != null ?
                                    typeof(ICommandHandler<,>) :
                                    typeof(ICommandHandler<>);

            Type handlerType = handlerInterface.MakeGenericType(genericParams);

            return this.Handlers.Single(FN.ClassImplements(handlerType));
        }

        private IEnumerable<Type> GetListenersForNotification(Type notification)
        {
            Type ValidatorInterface = typeof(INotificationListener<>);
            var validatorType = ValidatorInterface.MakeGenericType(notification);
            return this.Listeners.Where(FN.ClassImplements(validatorType));
        }

        private IEnumerable<Type> GetValidatorsForCommand(Type command)
        {
            Type ValidatorInterface = typeof(ICommandValidator<>);
            var validatorType = ValidatorInterface.MakeGenericType(command);
            return this.Validators.Where(FN.ClassImplements(validatorType));
        }

        public Dictionary<Type, CommandCacheItem> GenerateCommandCache()
        {
            Dictionary<Type, CommandCacheItem> result = new Dictionary<Type, CommandCacheItem>();

            foreach (var command in Commands)
            {
                var valueInterface = command.GetInterface(typeof(ICommand<>).FullName);
                var returnType = valueInterface?.GenericTypeArguments.Single();
                Type commandHandler = GetHandlerForCommand(command, returnType);

                IEnumerable<Type> commandValidators = GetValidatorsForCommand(command);
                result.Add(command, new CommandCacheItem(command, commandHandler, commandValidators));
            }

            return result;
        }

        public Dictionary<Type, NotificationCacheItem> GenerateNotificationCache()
        {
            Dictionary<Type, NotificationCacheItem> result = new Dictionary<Type, NotificationCacheItem>();
            
            foreach (var notification in Notifications)
            {
                IEnumerable<Type> listeners = GetListenersForNotification(notification);
                result.Add(notification, new NotificationCacheItem(notification, listeners));
            }

            return result;
        }
    }
}
