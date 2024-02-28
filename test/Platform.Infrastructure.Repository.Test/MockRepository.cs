using Moq;
using Platform.Infrastructure.Common;
using Platform.Infrastructure.Core;

namespace Platform.Infrastructure.Repository.Test
{

    public class MockRepository<T> where T : BaseEntity
    {
        public IList<T> Context { get; set; }
        public Mock<IRepository> MockRepo { get; set; }

        public void SetMock()
        {
            this.Context = new List<T>();
            this.MockRepo = new Mock<IRepository>();
         
           

            this.MockRepo.Setup(r=> r.CreateAsync<T>(It.IsAny<T>()))
                .Callback<T>(item =>
                {                   
                    Context.Add(item);
                });

            //this.MockRepo.Setup(r => r.DeleteAsync<T>(It.IsAny<T>()))
            //    .Callback<Guid>(id =>
            //    {
            //        var item = Context.FirstOrDefault(f => f.Id == id);
            //        Context.Remove(item);
            //    });

        }
    }


}
