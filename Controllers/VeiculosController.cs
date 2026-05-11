using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.ViewModels;
using Projeto_AutoMobile.ViewModels.Veiculos;
using System.Linq;
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
        public async Task<IActionResult> Disponiveis(string tipoSelecionado)
        {
            // Vai buscar a empresa para sabermos qual é a "Data Atual" do Simulador
            var empresa = await _context.Empresas.FirstOrDefaultAsync();
            if (empresa == null) return NotFound("Empresa não inicializada.");

            // Vai buscar todos os veículos à Base de Dados
            var frota = await _context.Veiculos.ToListAsync();

            // Só são disponíveis os que têm estado Disponível OU cuja data já expirou perante o simulador
            var disponiveis = frota.Where(v =>
                v.Estado == EstadoVeiculo.Disponivel ||
                (v.DataDisponibilidade.HasValue && v.DataDisponibilidade.Value.Date <= empresa.DataAtual.Date)
            ).ToList();

            // O FILTRO DE TIPO DE VEÍCULO:
            if (!string.IsNullOrEmpty(tipoSelecionado) && tipoSelecionado != "Todos")
            {
                disponiveis = disponiveis.Where(v => v.GetType().Name == tipoSelecionado).ToList();
            }

            // Montar os dados para o ecrã
            var viewModel = new VeiculosDisponiveisViewModel
            {
                DataAtual = empresa.DataAtual,
                TipoSelecionado = tipoSelecionado ?? "Todos",
                Veiculos = disponiveis
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

    }
}