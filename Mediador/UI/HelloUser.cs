using System;
using System.Threading.Tasks;
using Mediador3;

namespace Mediador.UI
{
    public class HelloUserCommand : ICommand<string>
    {
        public string UserName { get; set; }
    }
    public class HelloUserHandler : ICommandHandler<HelloUserCommand, string>
    {
        public async Task<string> Handle(HelloUserCommand command)
        {
            Mediador3.Mediador mediador = Mediador3.Mediador.CreateInstance();
            
            await mediador.NotifyAllAsync(new HelloUserCommandWillRunNotification { UserName = command.UserName });
            var result = await Task.FromResult($"Hello {command.UserName}!");
            return result;
        }
    }
    public class HelloUserCommandValidation : ICommandValidator<HelloUserCommand>
    {
        public async Task<bool> Validate(HelloUserCommand command)
        {
            var invalid = string.IsNullOrEmpty(command.UserName) || string.IsNullOrWhiteSpace(command.UserName);
            return await Task.FromResult(!invalid);
        }
    }

    public class HelloUserCommandWillRunNotification : INotification
    {
        public string UserName { get; set; }
    }
    public class HelloUserCommandWillRunNotificationListener :
        INotificationListener<HelloUserCommandWillRunNotification>
    {
        public async Task OnNotification(HelloUserCommandWillRunNotification data)
        {
            await Task.Run(() => Console.WriteLine($"HelloUser will run for user: {data.UserName}"));
        }
    }
    public class HelloUserCommandWillRunNotificationListener2 :
        INotificationListener<HelloUserCommandWillRunNotification>
    {
        public async Task OnNotification(HelloUserCommandWillRunNotification data)
        {
            await Task.Run(() => Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Logging hello user..."));
        }
    }
    
    public class HelloWordCommand : ICommand { }
    public class HelloWorldHandler : ICommandHandler<HelloWordCommand>
    {
        public async Task Handle(HelloWordCommand command)
        {
            Console.WriteLine("Helloworld");
            await Task.CompletedTask;
        }
    }
}
