using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager
{
    public class AddBasicDetailsRequest
    {
        public Guid CaseID { get; set; }
        public Guid EntityID { get; set; }
        public String DateOfBirth { get; set; }
        public String CountryOfResidence { get; set; }
    }
}
