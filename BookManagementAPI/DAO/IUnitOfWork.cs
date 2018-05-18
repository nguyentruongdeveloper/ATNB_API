using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> CategoryRepository { get; }
        void Save();
    }
}
