using Something.Domain;
using Something.Persistence;

namespace Something.Application
{
    public class SomethingElseDeleteInteractor : ISomethingElseDeleteInteractor
    {
        private readonly ISomethingFactory somethingFactory;
        private ISomethingElseFactory somethingElseFactory;
        private ISomethingElsePersistence persistence;

        public SomethingElseDeleteInteractor(ISomethingFactory somethingFactory, ISomethingElseFactory somethingElseFactory, ISomethingElsePersistence persistence)
        {
            this.somethingFactory = somethingFactory;
            this.somethingElseFactory = somethingElseFactory;
            this.persistence = persistence;
        }
        public void DeleteSomethingElse(int id)
        {
            persistence.DeleteSomethingElseById(id);
        }
    }
}
