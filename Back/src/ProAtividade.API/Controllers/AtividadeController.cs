using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProAtividade.Api.Data;
using ProAtividade.API.Models;

namespace ProAtividade.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtividadeController : ControllerBase
    {
        public DataContext Context { get; }

        public AtividadeController(DataContext context)
{
            Context = context;
        }
        [HttpGet]
        public IEnumerable<Atividade> Get(){
            return Context.Atividades;
        }

        [HttpGet("{id}")]
        public Atividade Get(int id){
            return Context.Atividades.FirstOrDefault(ati => ati.id == id);
        }

        [HttpPost]
        public Atividade Post(Atividade atividade){
            Context.Atividades.Add(atividade);
            if(Context.SaveChanges()>0)
                return Context.Atividades.FirstOrDefault(ativ=> ativ.id==atividade.id);
            else
                throw new Exception("Você não conseguiu adicionar nova atividade");
                }

        [HttpPut("{id}")]
        public Atividade Put(int id, Atividade atividade){
            if(atividade.id != id) throw new Exception("Você está tentando Atualizar a atividade Errada!");
            Context.Update(atividade);
            if(Context.SaveChanges()>0)
                return Context.Atividades.FirstOrDefault(ativ=> ativ.id==id);
            else
                return new Atividade(); 
        }
        [HttpDelete("{id}")]
        public bool Delete(int id){
            var atividade = Context.Atividades.FirstOrDefault(ativ=> ativ.id==id);
            if(atividade ==null)
                throw new Exception("Você está tentando deletar uma Atividade que não existe");
            else
                Context.Remove(atividade);
            return Context.SaveChanges() >0;
        }
    }
}