using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Domain;
using Domain.Commands;
using EventStore.ClientAPI;
using EventStoreFramework;
using EventStoreFramework.Command;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace EventManager
{
    public class CreateNewEntity
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(Case input, ILambdaContext context)
        {
            var eventStoreConnection = EventStoreConnection.Create(
                ConnectionSettings.Default,
                new IPEndPoint(IPAddress.Parse("34.254.92.105"), 1113));

            eventStoreConnection.ConnectAsync().Wait();

            var repository = new EventStoreRepository(eventStoreConnection);

            var commandHandlerMap = new CommandHandlerMap(new Handlers(repository));

            var _commandDispatcher = new Dispatcher(commandHandlerMap);

            try
            {
                var createEntityCommand = new CreateEntity(input.EntityName);
                _commandDispatcher.Dispatch(createEntityCommand).Wait();
            }
            catch(Exception e)
            {
                LambdaLogger.Log(e.Message);
            }

            LambdaLogger.Log($"Calling function name: {context.FunctionName}\\n");
            return $"Entity Created";
        }
    }
}
