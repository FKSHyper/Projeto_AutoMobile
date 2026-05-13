using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.ViewModels.Reserva;

namespace Projeto_AutoMobile.Controllers
{
    public class ReservasController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public ReservasController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Veiculo)
                .ToListAsync();

            return View(reservas);
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");

            return View(new ReservaViewModel());
        }

        // POST: Calcular preço (botão "Atualizar preço")
        [HttpPost]
        public async Task<IActionResult> CalcularPreco(ReservaViewModel model)
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");

            if (model.DataInicio == default || model.DataFim == default || model.VeiculoId == 0)
            {
                ModelState.AddModelError("", "Preenche datas e veículo.");
                return View("Create", model);
            }

            if (model.DataFim < model.DataInicio)
            {
                ModelState.AddModelError("", "A data final não pode ser anterior à inicial.");
                return View("Create", model);
            }

            var veiculo = await _context.Veiculos.FindAsync(model.VeiculoId);

            if (veiculo == null)
            {
                ModelState.AddModelError("", "Veículo não encontrado.");
                return View("Create", model);
            }

            int dias = (model.DataFim - model.DataInicio).Days;

            if (dias <= 0)
            {
                ModelState.AddModelError("", "A duração tem de ser pelo menos 1 dia.");
                return View("Create", model);
            }

            model.PrecoEstimado = dias * veiculo.PrecoDia;

            return View("Create", model);
        }

        // POST: Create (criar reserva final)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservaViewModel model)
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");

            if (model.DataInicio < DateTime.Today)
            {
                ModelState.AddModelError("", "A data inicial não pode ser anterior a hoje.");
                return View(model);
            }

            if (model.DataFim < model.DataInicio)
            {
                ModelState.AddModelError("", "A data final não pode ser anterior à inicial.");
                return View(model);
            }

            var veiculo = await _context.Veiculos.FindAsync(model.VeiculoId);

            if (veiculo == null)
            {
                ModelState.AddModelError("", "Veículo inválido.");
                return View(model);
            }

            int dias = (model.DataFim - model.DataInicio).Days;

            if (dias <= 0)
            {
                ModelState.AddModelError("", "Datas inválidas.");
                return View(model);
            }

            var reserva = new Reserva
            {
                DataInicio = model.DataInicio,
                DataFim = model.DataFim,
                ClienteId = model.ClienteId,
                VeiculoId = model.VeiculoId,
                PrecoTotal = dias * veiculo.PrecoDia
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}