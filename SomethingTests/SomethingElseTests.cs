using Microsoft.EntityFrameworkCore;
using Moq;
using Something.Application;
using Something.Domain;
using Something.Persistence;
using SomethingTests.Infrastructure.Factories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Domain = Something.Domain.Models;

namespace SomethingTests
{
    [ExcludeFromCodeCoverage]
    public class SomethingElseTests
    {
        private readonly Domain.SomethingElse somethingElse = Domain.SomethingElse.CreateNamedSomethingElse("Fred Bloggs");
        private readonly Domain.Something something = new Domain.Something() { Name = "Alice Bloggs" };

        public SomethingElseTests()
        {
            somethingElse.Somethings.Add(something);
        }

        [Fact]
        public void SomethingElse_HasAnId()
        {
            var name = "Fred Bloggs";
            var something1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            int expected = 0;

            int actual = something1.Id;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SomethingElse_SetsId()
        {
            var name = "Fred Bloggs";
            var something1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            int expected = 1;

            something1.Id = expected;
            int actual = something1.Id;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SomethingElse_HasAName()
        {
            var expected = "Fred Bloggs";
            var something1 = Domain.SomethingElse.CreateNamedSomethingElse(expected);

            string actual = something1.Name;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SomethingElseFactory_Create_CreatesSomethingElseWithName()
        {
            SomethingElseFactory factory = new SomethingElseFactory();
            string expected = "Fred Bloggs";

            Domain.SomethingElse actual = factory.Create(expected);

            Assert.IsType<Domain.SomethingElse>(actual);
            Assert.Equal(expected, actual.Name);
        }
        [Fact]
        public void SomethingElse_CreateNamedSomethingElse_ThrowsArgumentExceptionWithoutName()
        {
            string name = null;

            var exception = Assert.Throws<ArgumentException>(() => Domain.SomethingElse.CreateNamedSomethingElse(name));
            Assert.Equal("name", exception.ParamName);
        }
        [Fact]
        public void SomethingElseFactory_Create_ThrowsArgumentExceptionWithoutName()
        {
            SomethingElseFactory factory = new SomethingElseFactory();
            string name = null;

            var exception = Assert.Throws<ArgumentException>(() => factory.Create(name));
            Assert.Equal("name", exception.ParamName);
        }
        [Fact]
        public void DbContextFactory_CreateAppDbContext_SavesSomethingElseToDatabaseAndRetrievesIt()
        {

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseToDatabaseAndRetrievesIt)))
            {
                ctx.SomethingElses.Add(somethingElse);
                ctx.SaveChanges();
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseToDatabaseAndRetrievesIt)))
            {
                var savedSomethingElse = ctx.SomethingElses.Single();
                Assert.Equal(somethingElse.Name, savedSomethingElse.Name);
            };
        }
        [Fact]
        public void DbContextFactory_CreateAppDbContext_SavesSomethingElseToDatabaseAndRetrievesItSettingItsId()
        {
            int expected = 1;
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseToDatabaseAndRetrievesItSettingItsId)))
            {
                ctx.SomethingElses.Add(somethingElse);
                ctx.SaveChanges();
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseToDatabaseAndRetrievesItSettingItsId)))
            {
                var savedSomethingElse = ctx.SomethingElses.Single();
                Assert.Equal(expected, savedSomethingElse.Id);
            };
        }

        [Fact]
        public void SomethingElsePersistence__SaveSomethingElse__SavesSomethingElseToDatabase()
        {
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__SaveSomethingElse__SavesSomethingElseToDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__SaveSomethingElse__SavesSomethingElseToDatabase)))
            {
                var savedSomethingElse = ctx.SomethingElses.Include(s => s.Somethings).Single();
                Assert.Equal(somethingElse.Somethings[0].Name, savedSomethingElse.Somethings[0].Name);
                Assert.Equal(somethingElse.Name, savedSomethingElse.Name);
            };
        }

        [Fact]
        public void SomethingElsePersistence__GetSomethingElseList__RetrievesSomethingElseListFromDatabase()
        {
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__GetSomethingElseList__RetrievesSomethingElseListFromDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__GetSomethingElseList__RetrievesSomethingElseListFromDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                var savedSomethingElse = persistence.GetSomethingElseList();
                Assert.Equal(somethingElse.Name, savedSomethingElse.Single().Name);
            };
        }

        [Fact]
        public void SomethingElseCreateInteractor_CreateSomethingElse_PersistsSomethingElseWithName()
        {
            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            Mock<ISomethingElseFactory> mockSomethingElseFactory = new Mock<ISomethingElseFactory>();
            mockSomethingElseFactory.Setup(x => x.Create(somethingElse.Name)).Returns(somethingElse);
            Mock<ISomethingElsePersistence> mockPersistence = new Mock<ISomethingElsePersistence>();
            SomethingElseCreateInteractor somethingElseInteractor = new SomethingElseCreateInteractor(mockSomethingFactory.Object, mockSomethingElseFactory.Object, mockPersistence.Object);

            somethingElseInteractor.CreateSomethingElse(somethingElse.Name);

            mockPersistence.Verify(x => x.SaveSomethingElse(somethingElse));
        }

        [Fact]
        public void SomethingElseReadInteractor_GetSomethingElseList_RetrievesSomethingElseListFromPersistence()
        {
            var somethingElseList = new List<Domain.SomethingElse>();
            somethingElseList.Add(somethingElse);
            var mockPersistence = new Mock<ISomethingElsePersistence>();
            mockPersistence.Setup(x => x.GetSomethingElseList()).Returns(somethingElseList);
            SomethingElseReadInteractor interactor = new SomethingElseReadInteractor(mockPersistence.Object);

            List<Domain.SomethingElse> somethingElseList1 = interactor.GetSomethingElseList();

            Assert.Equal(somethingElseList.Count, somethingElseList1.Count);
            Assert.Equal(somethingElseList[somethingElseList.Count - 1].Name, somethingElseList1[somethingElseList1.Count - 1].Name);
        }
        [Fact]
        public void SomethingElse_HasAListOfSomethings()
        {
            var name = "Fred Bloggs";
            var somethingElse1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            int expected = 0;

            int actual = somethingElse1.Somethings.Count;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SomethingElse_AddSomething_AddsSomethingToSomethings()
        {
            var name = "Fred Bloggs";
            var somethingElse1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            int expected = 1;

            somethingElse1.Somethings.Add(something);
            int actual = somethingElse1.Somethings.Count;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void DbContextFactory_CreateAppDbContext_SavesSomethingElseWithSomethingToDatabaseAndRetrievesIt()
        {
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseWithSomethingToDatabaseAndRetrievesIt)))
            {
                ctx.SomethingElses.Add(somethingElse);
                ctx.SaveChanges();
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingElseWithSomethingToDatabaseAndRetrievesIt)))
            {
                var savedSomethingElse = ctx.SomethingElses.Include(s => s.Somethings).Single();
                Assert.Equal(somethingElse.Somethings[0].Name, savedSomethingElse.Somethings[0].Name);
            };
        }

        [Fact]
        public void SomethingElsePersistence__GetSomethingElseList__RetrievesListOfSomethingElseIncludingSomethingListFromDatabase()
        {
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__GetSomethingElseList__RetrievesListOfSomethingElseIncludingSomethingListFromDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__GetSomethingElseList__RetrievesListOfSomethingElseIncludingSomethingListFromDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                var savedSomethingElses = persistence.GetSomethingElseIncludingSomethingList();
                foreach (var savedSomethingElse in savedSomethingElses)
                {
                    Assert.Equal(somethingElse.Somethings[0].Name, savedSomethingElse.Somethings[0].Name);
                }
            };
        }

        [Fact]
        public void SomethingElseReadInteractor_GetSomethingElseIncludingSomethingsList_RetrievesSomethingElseIncludingSomethingsListFromPersistence()
        {
            var somethingElseList = new List<Domain.SomethingElse>();
            somethingElseList.Add(somethingElse);
            var mockPersistence = new Mock<ISomethingElsePersistence>();
            mockPersistence.Setup(x => x.GetSomethingElseIncludingSomethingList()).Returns(somethingElseList);
            SomethingElseReadInteractor interactor = new SomethingElseReadInteractor(mockPersistence.Object);

            List<Domain.SomethingElse> somethingElseList1 = interactor.GetSomethingElseIncludingSomethingsList();

            foreach (var savedSomethingElse in somethingElseList1)
            {
                Assert.Equal(somethingElse.Somethings[0].Name, savedSomethingElse.Somethings[0].Name);
            }
        }

        [Fact]
        public void SomethingElseCreateInteractor_CreateSomethingElse_PersistsSomethingElseWithSomethings()
        {
            var name = "Fred Bloggs";
            var somethingElse1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            mockSomethingFactory.Setup(x => x.Create(something.Name)).Returns(something);
            Mock<ISomethingElseFactory> mockSomethingElseFactory = new Mock<ISomethingElseFactory>();
            mockSomethingElseFactory.Setup(x => x.Create(somethingElse1.Name)).Returns(somethingElse1);
            Mock<ISomethingElsePersistence> mockPersistence = new Mock<ISomethingElsePersistence>();
            SomethingElseCreateInteractor somethingElseInteractor = new SomethingElseCreateInteractor(mockSomethingFactory.Object, mockSomethingElseFactory.Object, mockPersistence.Object);
            string[] othernames = { "Alice Bloggs" };
            somethingElseInteractor.CreateSomethingElse(name, othernames);

            mockPersistence.Verify(x => x.SaveSomethingElse(somethingElse1));
        }

        [Fact]
        public void SomethingElsePersistence__UpdateSomethingElseByIdAddSomething__RetrievesSomethingElseByIdFromDatabase()
        {
            int id = 1;
            var something1 = new Domain.Something() { Name = "Bob" };
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdAddSomething__RetrievesSomethingElseByIdFromDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdAddSomething__RetrievesSomethingElseByIdFromDatabase)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                var updatedSomethingElse = persistence.UpdateSomethingElseByIdAddSomething(id, something1);
                Assert.Equal(somethingElse.Name, updatedSomethingElse.Name);
                Assert.Equal(somethingElse.Somethings.Count + 1, updatedSomethingElse.Somethings.Count);
            };
        }

        [Fact]
        public void SomethingElsePersistence__UpdateSomethingElseByIdAddSomething__ThrowsInvalidOperationExceptionGivenIdOfNonexistentSomethingElse()
        {
            int id = 5;
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdAddSomething__ThrowsInvalidOperationExceptionGivenIdOfNonexistentSomethingElse)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            mockSomethingFactory.Setup(x => x.Create(something.Name)).Returns(something);

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdAddSomething__ThrowsInvalidOperationExceptionGivenIdOfNonexistentSomethingElse)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                Domain.Something something1 = mockSomethingFactory.Object.Create(something.Name);
                var exception = Assert.Throws<InvalidOperationException>(() => persistence.UpdateSomethingElseByIdAddSomething(id, something1));
            };
        }
        [Fact]
        public void SomethingElsePersistence__UpdateSomethingElseByIdAddSomething__ThrowsInvalidOperationExceptionGivenNonexistentSomething()
        {
            int id = 5;
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdAddSomething__ThrowsInvalidOperationExceptionGivenNonexistentSomething)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            mockSomethingFactory.Setup(x => x.Create(something.Name)).Returns((Domain.Something)null);

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdAddSomething__ThrowsInvalidOperationExceptionGivenNonexistentSomething)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                Domain.Something something1 = mockSomethingFactory.Object.Create(something.Name);
                var exception = Assert.Throws<InvalidOperationException>(() => persistence.UpdateSomethingElseByIdAddSomething(id, something1));
            };
        }

        [Fact]
        public void SomethingElseUpdateInteractor_UpdateSomethingElseAddSomething_PersistsSomethingElseWithSomethings()
        {
            var name = "Fred Bloggs";
            var somethingElse1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            mockSomethingFactory.Setup(x => x.Create(something.Name)).Returns(something);
            Mock<ISomethingElseFactory> mockSomethingElseFactory = new Mock<ISomethingElseFactory>();
            mockSomethingElseFactory.Setup(x => x.Create(somethingElse1.Name)).Returns(somethingElse1);
            Mock<ISomethingElsePersistence> mockPersistence = new Mock<ISomethingElsePersistence>();
            SomethingElseUpdateInteractor somethingElseInteractor = new SomethingElseUpdateInteractor(mockSomethingFactory.Object, mockSomethingElseFactory.Object, mockPersistence.Object);
            string othername = "Alice Bloggs";
            int id = 1;
            somethingElseInteractor.UpdateSomethingElseAddSomething(id, othername);

            mockPersistence.Verify(x => x.UpdateSomethingElseByIdAddSomething(id, something));
        }


        [Fact]
        public void SomethingElsePersistence__UpdateSomethingElseByIdDeleteSomethingById__UpdatesSomethingElseByIdFromDatabaseWithSomethingDeleted()
        {
            int id = 1;
            int something_id = 1;
            var something1 = new Domain.Something() { Name = "Bob" };
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdDeleteSomethingById__UpdatesSomethingElseByIdFromDatabaseWithSomethingDeleted)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdDeleteSomethingById__UpdatesSomethingElseByIdFromDatabaseWithSomethingDeleted)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                var updatedSomethingElse = persistence.UpdateSomethingElseByIdDeleteSomethingById(id, something_id);
                Assert.Equal(somethingElse.Name, updatedSomethingElse.Name);
                Assert.Equal(somethingElse.Somethings.Count - 1, updatedSomethingElse.Somethings.Count);
            };
        }

        [Fact]
        public void SomethingElsePersistence__UpdateSomethingElseByIdDeleteSomethingById__ThrowsInvalidOperationExceptionGivenNonexistentSomethingElse()
        {
            int id = 5;
            int id2 = 1;
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdDeleteSomethingById__ThrowsInvalidOperationExceptionGivenNonexistentSomethingElse)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            mockSomethingFactory.Setup(x => x.Create(something.Name)).Returns((Domain.Something)null);

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdDeleteSomethingById__ThrowsInvalidOperationExceptionGivenNonexistentSomethingElse)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                Domain.Something something1 = mockSomethingFactory.Object.Create(something.Name);
                var exception = Assert.Throws<InvalidOperationException>(() => persistence.UpdateSomethingElseByIdDeleteSomethingById(id, id2));
            };
        }

        [Fact]
        public void SomethingElsePersistence__UpdateSomethingElseByIdDeleteSomethingById__ThrowsInvalidOperationExceptionGivenNonexistentSomething()
        {
            int id = 1;
            int id2 = 5;
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdDeleteSomethingById__ThrowsInvalidOperationExceptionGivenNonexistentSomething)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            mockSomethingFactory.Setup(x => x.Create(something.Name)).Returns((Domain.Something)null);

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__UpdateSomethingElseByIdDeleteSomethingById__ThrowsInvalidOperationExceptionGivenNonexistentSomething)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                Domain.Something something1 = mockSomethingFactory.Object.Create(something.Name);
                var exception = Assert.Throws<InvalidOperationException>(() => persistence.UpdateSomethingElseByIdDeleteSomethingById(id, id2));
            };
        }
        [Fact]
        public void SomethingElseUpdateInteractor_UpdateSomethingElseDeleteSomething_PersistsSomethingElseWithSomethingDeleted()
        {
            var name = "Fred Bloggs";
            var somethingElse1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            mockSomethingFactory.Setup(x => x.Create(something.Name)).Returns(something);
            Mock<ISomethingElseFactory> mockSomethingElseFactory = new Mock<ISomethingElseFactory>();
            mockSomethingElseFactory.Setup(x => x.Create(somethingElse1.Name)).Returns(somethingElse1);
            Mock<ISomethingElsePersistence> mockPersistence = new Mock<ISomethingElsePersistence>();
            SomethingElseUpdateInteractor somethingElseInteractor = new SomethingElseUpdateInteractor(mockSomethingFactory.Object, mockSomethingElseFactory.Object, mockPersistence.Object);
            int else_id = 1;
            int something_id = 1;
            somethingElseInteractor.UpdateSomethingElseDeleteSomething(else_id, something_id);

            mockPersistence.Verify(x => x.UpdateSomethingElseByIdDeleteSomethingById(else_id, something_id));
        }

        [Fact]
        public void SomethingElsePersistence__DeleteSomethingElseById__DeleteSomethingElseFromDatabaseById()
        {
            int id = 1;
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__DeleteSomethingElseById__DeleteSomethingElseFromDatabaseById)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__DeleteSomethingElseById__DeleteSomethingElseFromDatabaseById)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.DeleteSomethingElseById(id);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence__DeleteSomethingElseById__DeleteSomethingElseFromDatabaseById)))
            {
                var persistence = new SomethingElsePersistence(ctx);

                var savedSomethingElses = persistence.GetSomethingElseIncludingSomethingList().Where(f => f.Id == id).ToList();

                int expected = 0;
                int actual = savedSomethingElses.Count;
                Assert.Equal(expected, actual);
            };
        }


        [Fact]
        public void SomethingElseCreateInteractor_DeleteSomethingElse_DeletesSomethingElseFromPersistence()
        {
            var name = "Fred Bloggs";
            var somethingElse1 = Domain.SomethingElse.CreateNamedSomethingElse(name);
            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            Mock<ISomethingElseFactory> mockSomethingElseFactory = new Mock<ISomethingElseFactory>();
            mockSomethingElseFactory.Setup(x => x.Create(somethingElse1.Name)).Returns(somethingElse1);
            Mock<ISomethingElsePersistence> mockPersistence = new Mock<ISomethingElsePersistence>();
            SomethingElseDeleteInteractor somethingElseInteractor = new SomethingElseDeleteInteractor(mockSomethingFactory.Object, mockSomethingElseFactory.Object, mockPersistence.Object);

            somethingElseInteractor.DeleteSomethingElse(somethingElse1.Id);

            mockPersistence.Verify(x => x.DeleteSomethingElseById(somethingElse1.Id));
        }

        [Fact]
        public void SomethingElsePersistence_DeleteSomethingElseById_DeletionCascades()
        {
            int id = 1;
            List<int> childIds;
            var something1 = new Domain.Something() { Name = "Bob" };
            Domain.SomethingElse somethingElse1 = Domain.SomethingElse.CreateNamedSomethingElse("Fred Bloggs");
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence_DeleteSomethingElseById_DeletionCascades)))
            {
                var persistence = new SomethingElsePersistence(ctx);
                persistence.SaveSomethingElse(somethingElse1);
                var updatedSomethingElse = persistence.UpdateSomethingElseByIdAddSomething(somethingElse1.Id, something1);
                var somethingElse = ctx.SomethingElses.Include(s => s.Somethings).Where(r => r.Id == id).FirstOrDefault();
                childIds = somethingElse.Somethings.Select(c => c.Id).ToList();
                ctx.Remove(somethingElse);
                ctx.SaveChanges();
            }

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingElsePersistence_DeleteSomethingElseById_DeletionCascades)))
            {
                Assert.Empty(ctx.Somethings.Where(c => childIds.Contains(c.Id)));
            };
        }
    }
}
