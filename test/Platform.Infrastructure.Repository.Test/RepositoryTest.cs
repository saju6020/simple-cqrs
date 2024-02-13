using NUnit.Framework;
using Platform.Infrastructure.Repository.Test.Fake;

namespace Platform.Infrastructure.Repository.Test
{
    [TestFixture]
    public class RepositoryTest
    {
        private  MockRepository<TestUser> mockRepository;
        
        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository<TestUser>();
            mockRepository.SetMock();
            this.InitUser();

        }

        [Test]
        public void Get_ItemById_Successflly()
        {
            var user = mockRepository.MockRepo.Object.GetById<TestUser>(new Guid("58697233-07f3-474b-8152-07dbfaf27c60"));
            Assert.NotNull(user);
        }

        [Test]
        public void Get_ItemById_Failed()
        {
            var user = mockRepository.MockRepo.Object.GetById<TestUser>(new Guid("58697233-07f3-474b-8152-07dbfaf27c61"));
            Assert.IsNull(user);
        }

        [Test]
        public async Task Create_Successfully()
        {

            var id = Guid.NewGuid();
            TestUser newTestUser = new TestUser()
            {
                Id = id,
                Name = "Test",
                Email = "Test",
                Designation = "Test",

            };

            var expectedCount = 6;

            await mockRepository.MockRepo.Object.CreateAsync<TestUser>(newTestUser);

            Assert.IsTrue(expectedCount == this.mockRepository.Context.Count);
        }

        [Test]
        public async Task Delete_Item_Successfully()
        {            
            await mockRepository.MockRepo.Object.DeleteAsync<TestUser>(new Guid("1f9ebad8-d2d4-4ff9-8098-aac879623a54"));

            int expectedCount = 4;

            var user = mockRepository.MockRepo.Object.GetById<TestUser>(new Guid("1f9ebad8-d2d4-4ff9-8098-aac879623a54"));
            Assert.IsNull(user);

            Assert.IsTrue(expectedCount == this.mockRepository.Context.Count);
        }

        private void SetupGetFunctions()
        {

        }

        private void InitUser()
        {
            this.mockRepository.Context.Add(new TestUser() { Id = new Guid("1f9ebad8-d2d4-4ff9-8098-aac879623a54"), Name = "test1", Email = "test1@gmail.com", Designation = "Des-1" });
            this.mockRepository.Context.Add(new TestUser() { Id = new Guid("58697233-07f3-474b-8152-07dbfaf27c60"), Name = "test2", Email = "test2@gmail.com", Designation = "Des-2" });
            this.mockRepository.Context.Add(new TestUser() { Id = new Guid("bcf012c4-6629-48cf-9bc8-4fdca2f83850"), Name = "test3", Email = "test3@gmail.com", Designation = "Des-3" });
            this.mockRepository.Context.Add(new TestUser() { Id = new Guid("0eaf41ea-f3f6-43f3-8d05-e0b074bfc486"), Name = "test4", Email = "test4@yahoo.com", Designation = "Des-4" });
            this.mockRepository.Context.Add(new TestUser() { Id = new Guid("4338dd13-47f9-4667-a098-ddcdf124eb64"), Name = "test5", Email = "test5@yahoo.com", Designation = "Des-5" });
        }
    }
}
