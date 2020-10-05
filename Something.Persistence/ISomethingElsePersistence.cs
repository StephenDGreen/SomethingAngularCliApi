using Something.Domain.Models;
using System.Collections.Generic;

namespace Something.Persistence
{
    public interface ISomethingElsePersistence
    {
        void DeleteSomethingElseById(int id);
        List<SomethingElse> GetSomethingElseIncludingSomethingList();
        List<SomethingElse> GetSomethingElseList();
        void SaveSomethingElse(SomethingElse somethingElse);
        SomethingElse UpdateSomethingElseByIdAddSomething(int id, Domain.Models.Something something);
        SomethingElse UpdateSomethingElseByIdDeleteSomethingById(int else_id, int something_id);
    }
}