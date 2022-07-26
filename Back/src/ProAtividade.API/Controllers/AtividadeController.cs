using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProAtividade.Data.Context;
using ProAtividade.Domain.Entities;
using ProAtividade.Domain.Interfaces.Services;

namespace ProAtividade.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtividadeController : ControllerBase
    {

        private readonly IAtividadeService atividadeService;

        public AtividadeController(IAtividadeService atividadeService){
            this.atividadeService = atividadeService;

        }
        [HttpGet]
        public async Task<IActionResult> Get(){
            try
            {
                var atividades = await this.atividadeService.PegarTodasAtividadesAsync();
                if(atividades == null) return NoContent();

                return Ok(atividades);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Atividades. Erro: {ex.Message}");
             }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            try
            {
                var atividade = await this.atividadeService.PegarAtividadePorIdAsync(id);
                if(atividade == null) return NoContent();

                return Ok(atividade);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar Atividade ${id}. Erro: {ex.Message}");
             }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Atividade model){
            try
            {
                var atividade = await this.atividadeService.AdicionarAtividade(model);
                if(atividade == null) return NoContent();

                return Ok(atividade);
            }
            catch (System.Exception ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao adicionar Atividades. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Atividade model){
            try
            {
                if(model.Id != id)
                    this.StatusCode(StatusCodes.Status409Conflict, "Você está tentando atualizar a atividade errada");
                var atividade = await this.atividadeService.AtualizarAtividade(model);
                if(atividade == null) return NoContent();

                return Ok(atividade);
            }
            catch (System.Exception ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao alterar atividades. Erro: {ex.Message}");
            }

        }
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id){
            try
            {
                var atividade = await this.atividadeService.PegarAtividadePorIdAsync(id);
                if(atividade == null)
                    this.StatusCode(StatusCodes.Status409Conflict, 
                        "Você está tentando deletar a atividade errada ou que não existe");

                if (await this.atividadeService.DeletarAtividade(id)){
                    return Ok(new {message = "Deletado"});
                }
            else
            {
                return BadRequest("Ocorreu um problema não específico ao tentar deletar a atividade.");
            }

            }
            catch (System.Exception ex)
            {
                
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao Deletar atividade com ID ${id}. Erro: {ex.Message}");
            } 
        }
    }
}