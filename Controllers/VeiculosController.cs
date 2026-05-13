using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.ViewModels;
using Projeto_AutoMobile.ViewModels.Veiculos;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_AutoMobile.Controllers
{
    public class VeiculosController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public VeiculosController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // [HttpGet]
        public async Task<IActionResult> Disponiveis(string tipoSelecionado, string estadoSelecionado)
        {
            // Vai buscar a empresa para sabermos qual é a "Data Atual" do Simulador
            var empresa = await _context.Empresas.FirstOrDefaultAsync();
            if (empresa == null) return NotFound("Empresa não inicializada.");

            // Vai buscar todos os veículos à Base de Dados
            var frota = await _context.Veiculos.ToListAsync();

            var resultados = frota.AsEnumerable();

            // FILTRO DE TIPO DE VEÍCULO:
            if (!string.IsNullOrEmpty(tipoSelecionado) && tipoSelecionado != "Todos")
            {
                resultados = resultados.Where(v => v.GetType().Name == tipoSelecionado);
            }

            // FILTRO DE ESTADO DO VEÍCULO:
            if (!string.IsNullOrEmpty(estadoSelecionado) && estadoSelecionado != "Todos")
            {
                // Converte o estado selecionado para o enum correspondente
                if (Enum.TryParse(typeof(EstadoVeiculo), estadoSelecionado, out var estadoConvertido))
                {
                    resultados = resultados.Where(v => v.Estado == (EstadoVeiculo)estadoConvertido);
                }
            }

            // Montar os dados para o ecrã
            var viewModel = new VeiculosDisponiveisViewModel
            {
                DataAtual = empresa.DataAtual,
                TipoSelecionado = tipoSelecionado ?? "Todos",
                EstadoSelecionado = estadoSelecionado ?? "Todos",
                Veiculos = resultados.ToList()
            };

            return View(viewModel);
        }

        // GET: Veiculos/EmManutencao
        public async Task<IActionResult> EmManutencao()
        {
            // Vai buscar à Base de Dados APENAS os veículos que estão no estado EmManutencao
            var veiculosEmManutencao = await _context.Veiculos
                .Where(v => v.Estado == EstadoVeiculo.EmManutencao)
                .ToListAsync();

            // Envia a lista diretamente para o ecrã
            return View(veiculosEmManutencao);
        }

        [HttpGet]
        public async Task<IActionResult> ExportarCSV()
        {
            // StringBuilder permite construir texto de forma eficiente sem criar novas strings a cada concatenação.
            var veiculos = await _context.Veiculos.ToListAsync();
            var builder = new StringBuilder();

            // Cabeçalho do CSV, cada coluna separada por ponto e vírgula.
            builder.AppendLine("Id;Tipo;Matricula;Marca;Modelo;Preco/Dia;Estado;Disponibilidade");

            foreach (var v in veiculos)
            {
                string tipoObjeto = v.GetType().Name;

                // Converte o enum EstadoVeiculo para texto legível pelo utilizador.
                string estadoFormatado = v.Estado switch
                {
                    EstadoVeiculo.Disponivel => "Disponível",
                    EstadoVeiculo.Alugado => "Alugado",
                    EstadoVeiculo.Reservado => "Reservado",
                    EstadoVeiculo.EmManutencao => "Em Manutenção",
                    _ => v.Estado.ToString()
                };

                string dataTexto = v.DataDisponibilidade.HasValue
                    ? v.DataDisponibilidade.Value.ToString("dd/MM/yyyy")
                    : "N/A";

                builder.AppendLine($"{v.Id};{tipoObjeto};{v.Matricula};{v.Marca};{v.Modelo};{v.PrecoDia.ToString("F2")};{estadoFormatado};{dataTexto}");
            }

            var csvContent = builder.ToString();

            // Converte o texto do CSV para bytes em UTF-8 para poder ser enviado como ficheiro.
            var bytes = Encoding.UTF8.GetBytes(csvContent);

            return File(bytes, "text/csv", "Frota_Veiculos.csv");
        }
        [HttpPost]
        public IActionResult AtualizarDataSimulador(DateTime novaData)
        {
            return RedirectToAction("Index", new { data = novaData.ToString("yyyy-MM-dd") });
        }
    }
}