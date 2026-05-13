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

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Veiculo)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        // GET: Create
        public IActionResult Create()
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");

            return View(new ReservaViewModel());
        }

        // POST: CALCULAR PREÇO (botão "Atualizar preço")
        [HttpPost]
        public async Task<IActionResult> CalcularPreco(ReservaViewModel model)
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");

            var veiculo = await _context.Veiculos.FindAsync(model.VeiculoId);

            if (veiculo == null)
            {
                ModelState.AddModelError("", "Veículo inválido.");
                return View("Create", model);
            }

            if (model.DataInicio == default || model.DataFim == default)
            {
                ModelState.AddModelError("", "Preenche as datas.");
                return View("Create", model);
            }

            if (model.DataFim < model.DataInicio)
            {
                ModelState.AddModelError("", "A data final não pode ser anterior à inicial.");
                return View("Create", model);
            }

            int dias = (model.DataFim - model.DataInicio).Days;

            if (dias <= 0)
            {
                ModelState.AddModelError("", "A reserva tem de ter pelo menos 1 dia.");
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
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
                ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");
                return View(model);
            }

            if (model.DataInicio < DateTime.Today)
            {
                ModelState.AddModelError("", "A data inicial não pode ser anterior a hoje.");
            }

            if (model.DataFim < model.DataInicio)
            {
                ModelState.AddModelError("", "Datas inválidas.");
            }

            var veiculo = await _context.Veiculos.FindAsync(model.VeiculoId);

            if (veiculo == null)
            {
                ModelState.AddModelError("", "Veículo inválido.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
                ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");
                return View(model);
            }

            int dias = (model.DataFim - model.DataInicio).Days;

            var reserva = new Reserva
            {
                DataInicio = model.DataInicio,
                DataFim = model.DataFim,
                ClienteId = model.ClienteId,
                VeiculoId = model.VeiculoId,
                PrecoEstimado = dias * veiculo.PrecoDia
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound();

            var model = new ReservaViewModel
            {
                DataInicio = reserva.DataInicio,
                DataFim = reserva.DataFim,
                ClienteId = reserva.ClienteId,
                VeiculoId = reserva.VeiculoId
            };

            ViewBag.SelectedId = id;
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");

            return View(model);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservaViewModel model)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                reserva.DataInicio = model.DataInicio;
                reserva.DataFim = model.DataFim;
                reserva.ClienteId = model.ClienteId;
                reserva.VeiculoId = model.VeiculoId;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.SelectedId = id;
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");

            return View(model);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Veiculo)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Faturacao
        public IActionResult Faturacao()
        {
            return View();
        }

        // POST: Faturacao
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Faturacao(DateTime dataInicio, DateTime dataFim)
        {
            if (dataFim < dataInicio)
            {
                ModelState.AddModelError("", "A data final não pode ser anterior à inicial.");
                return View();
            }

            var total = await _context.Reservas
                .Where(r => r.DataInicio >= dataInicio && r.DataFim <= dataFim)
                .SumAsync(r => r.PrecoEstimado);

            ViewBag.TotalFaturacao = total;
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;

            return View();
        }
    }
}