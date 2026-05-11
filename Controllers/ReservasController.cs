using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.ViewModels.Reserva;
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

        // GET: Reservas/Details/5
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

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");

            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reserva = new Reserva
                {
                    DataInicio = model.DataInicio,
                    DataFim = model.DataFim,
                    ClienteId = model.ClienteId,
                    VeiculoId = model.VeiculoId,

                    // exemplo simples
                    PrecoTotal = 100
                };

                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "Id", "Nome");
            ViewBag.Veiculos = new SelectList(_context.Veiculos, "Id", "Marca");

            return View(model);
        }

        // GET: Reservas/Edit/5
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

        // POST: Reservas/Edit/5
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

        // GET: Reservas/Delete/5
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

        // POST: Reservas/Delete/5
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
    }
}