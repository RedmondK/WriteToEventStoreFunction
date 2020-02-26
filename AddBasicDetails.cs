using Amazon.Lambda.Core;
using Domain.Commands;
using EventStore.ClientAPI;
using EventStoreFramework;
using EventStoreFramework.Command;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace EventManager
{
    public class AddBasicEntityDetails
    {
        public string FunctionHandler(AddBasicDetailsRequest input, ILambdaContext context)
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
                var addDetailsCommand = new AddBasicDetails(input.CaseID, input.EntityID, input.DateOfBirth, input.CountryOfResidence);
                _commandDispatcher.Dispatch(addDetailsCommand).Wait();
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message);
            }

            LambdaLogger.Log($"Calling function name: {context.FunctionName}\\n");
            return $"Entity Updated";
        }
    }
}
