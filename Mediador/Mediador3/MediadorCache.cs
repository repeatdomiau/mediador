using System;
using System.Collections.ObjectModel;

namespace Mediador3
{
    internal class MediadorCache
    {
        private static MediadorCache _instance;
        
        private readonly ReadOnlyDictionary<Type, CommandCacheItem> CommandCache;
        private readonly ReadOnlyDictionary<Type, NotificationCacheItem> NotificationCache;

        private MediadorCache(MediadorTypeManager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            if (CommandCache == null)
            {
                var cache = manager.GenerateCommandCache();
                CommandCache = new ReadOnlyDictionary<Type, CommandCacheItem>(cache);
            }

            if(NotificationCache == null)
            {
                var cache = manager.GenerateNotificationCache();
                NotificationCache = new ReadOnlyDictionary<Type, NotificationCacheItem>(cache);
            }
        }

        public CommandCacheItem GetHandlerAndValidatorsForCommand(Type CommandType)
        {
            return CommandCache.ContainsKey(CommandType) ? CommandCache[CommandType] : null;
        }

        public NotificationCacheItem GetListenersForNotification(Type notificationType)
        {
            return NotificationCache.ContainsKey(notificationType) ? NotificationCache[notificationType] : null;
        }

        public static MediadorCache GetInstance()
        {
            if(_instance == null)
            {
                _instance = new MediadorCache(new MediadorTypeManager());
            }

            return _instance;            
        }

    }

}
