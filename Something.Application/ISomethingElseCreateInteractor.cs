namespace Something.Application
{
    public interface ISomethingElseCreateInteractor
    {
        void CreateSomethingElse(string name);
        void CreateSomethingElse(string name, string[] othernames);
    }
}