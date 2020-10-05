using Something.Domain.Models;

namespace Something.Application
{
    public interface ISomethingElseUpdateInteractor
    {
        SomethingElse UpdateSomethingElseAddSomething(int id, string name);
        SomethingElse UpdateSomethingElseDeleteSomething(int else_id, int something_id);
    }
}