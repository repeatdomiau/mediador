using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mediador3;

namespace Mediador.UI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Mediador3.Mediador mediador = Mediador3.Mediador.CreateInstance();
            HelloUserCommand cmd = new HelloUserCommand() { UserName = "Chan" };
            string result = await mediador.SendAsync<HelloUserCommand, string>(cmd);
            Console.WriteLine(result);

            HelloWordCommand cmd2 = new HelloWordCommand();
            await mediador.SendASync<HelloWordCommand>(cmd2);
        }
    }

}