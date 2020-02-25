using Domain;
using Domain.Commands;
using Domain.Enums;
using EventStoreFramework;
using EventStoreFramework.Command;

namespace EventManager
{
    public class Handlers : CommandHandler
    {
        public Handlers(EventStoreRepository repository)
        {
            Register<CreateEntity>(async c =>
            {
                var onboardingCase = new Case(CaseType.Onboarding, c.EntityId, c.EntityName);
                await repository.Save(onboardingCase);
            });

            Register<AddBasicDetails>(async e =>
            {
                var existingCase = await repository.Get<Case>(e.CaseId);
                existingCase.AddBasicDetails(e.DateOfBirth, e.CountryOfResidence);
                await repository.Save(existingCase);
            });
        }
    }
}