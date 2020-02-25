using Domain.Enums;
using Domain.Events;
using EventStoreFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WriteToEventStoreFunction
{
    public class Person : AggregateRoot
    {
        public string firstName { get; set; }
        public string lastName { get; set; }

        public Person(string entityName) : this()
        {
            Raise(new CaseInitiated(Guid.NewGuid(), CaseType.Maintenance));
            Raise(new EntityCreated(Guid.NewGuid(), firstName));
        }

        private Person()
        {
            Register<CaseInitiated>(When);
            Register<EntityCreated>(When);
        }

        public void When(CaseInitiated e)
        {
            Id = e.CaseId;
        }

        public void When(EntityCreated e)
        {
            firstName = e.EntityName;
        }
    }
}
