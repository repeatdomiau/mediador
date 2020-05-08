using System;
using System.Collections.Generic;

namespace Mediador3
{
    internal class CommandCacheItem
    {
        public Type Command { get; private set; }
        public Type Handler { get; private set; }
        public IEnumerable<Type> Validators { get; set; }

        public CommandCacheItem(Type command, Type handler, IEnumerable<Type> validators)
        {
            this.Command = command;
            this.Handler = handler;
            this.Validators = validators;
        }
    }

}
