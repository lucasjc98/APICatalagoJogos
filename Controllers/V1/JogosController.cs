using APICatalagoJogos.Exceptions;
using APICatalagoJogos.InputModel;
using APICatalagoJogos.Services;
using APICatalagoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalagoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de registros por página. Mínimo 1 e máximo 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não haja jogos</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
                return NoContent();

            return Ok(jogos);
        }

        /// <summary>
        /// Buscar um jogo pelo seu Id
        /// </summary>
        /// <param name="idJogo">ID do jogo que você procura</param>
        /// <response code="200">Retorna o jogo filtrado</response>
        /// <response code="204">Caso não haja jogo com esse id</response>
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            if (jogo == null)
                return NoContent();

            return Ok(jogo);
        }

        /// <summary>
        /// Insere um jogo
        /// </summary>
        /// <param name="jogoInputModel"></param>
        /// <response code="200">Jogo cadastrado com sucesso</response>
        /// <response code="422">Já existe um jogo que você está tentando cadastrar</response>
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);

                return Ok(jogo);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }
        }

        /// <summary>
        /// Atualiza um jogo existente
        /// </summary>
        /// <param name="idJogo">ID do jogo que você procura</param>
        /// <param name="jogoInputModel"></param>
        /// <response code="200">Jogo atualizado com sucesso</response>
        /// <response code="404">Jogo que você quer atualizar não foi encontrado</response>
        /// <response code="400">Não preencheu um campo corretamente</response>
        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Jogo não encontrado");
            }
        }

        /// <summary>
        /// Atualiza o preço de um jogo existente
        /// </summary>
        /// <param name="idJogo">ID do jogo que você procura</param>
        /// <param name="preco"></param>
        /// <response code="200">Preço do jogo atualizado com sucesso</response>
        /// <response code="404">Jogo que você quer atualizar não foi encontrado</response>
        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromRoute] double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Jogo não encontrado");
            }
        }

        /// <summary>
        /// Apagar um jogo existente
        /// </summary>
        /// <param name="idJogo">ID do jogo que você procura</param>
        /// <response code="200">Jogo apagado com sucesso</response>
        /// <response code="404">Jogo que você quer apagar não foi encontrado</response>
        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid idJogo)
        {
            try
            {
                await _jogoService.Remover(idJogo);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Jogo não encontrado");
            }
        }
    }
}
