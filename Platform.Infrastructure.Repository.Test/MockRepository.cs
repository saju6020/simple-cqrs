﻿using Moq;
using Platform.Infrastructure.Common;
using Platform.Infrastructure.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            this.MockRepo.Setup(r => r.GetById<T>(It.IsAny<Guid>()))
                .Returns<Guid>(id =>
                {
                    return this.Context.Where(f => f.Id == id).Single();
                });

            this.MockRepo.Setup(r => r.GetByIdAsync<T>(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) =>
                {
                    return this.Context.Where(f => f.Id == id).Single();
                });

        }
    }


}
