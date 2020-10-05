using Something.Domain.Models;
using Something.Persistence;
using System.Collections.Generic;

namespace Something.Application
{
    public class SomethingElseReadInteractor : ISomethingElseReadInteractor
    {
        private readonly ISomethingElsePersistence persistence;

        public SomethingElseReadInteractor(ISomethingElsePersistence persistence)
        {
            this.persistence = persistence;
        }

        public List<SomethingElse> GetSomethingElseList()
        {
            return persistence.GetSomethingElseList();
        }

        public List<SomethingElse> GetSomethingElseIncludingSomethingsList()
        {
            return persistence.GetSomethingElseIncludingSomethingList();
        }
    }
}
