using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProAtividade.Data.Context;
using ProAtividade.Domain.Interfaces.Repositories;

namespace ProAtividade.Data.Repositories
{
    public class GeneralRepo : IGeneralRepo
    {
        public DataContext Context { get; }
        public GeneralRepo(DataContext context)
        {
            this.Context = context;
            
        }
        public void Adicionar<T>(T entity) where T : class
        {
            this.Context.Add(entity);
        }

        public void Atualizar<T>(T entity) where T : class
        {
            this.Context.Update(entity);
        }

        public void Deletar<T>(T entity) where T : class
        {
            this.Context.Remove(entity);
        }

        public async Task<bool> SalvarMudancasAsync()
        {
            return (await this.Context.SaveChangesAsync())>0;
        }
    }
}