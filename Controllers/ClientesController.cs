using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;
using Projeto_AutoMobile.ViewModels.Cliente;

namespace Projeto_AutoMobile.Controllers
{
    public class ClientesController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public ClientesController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var listaClientes = _context.Clientes;
            return View(await listaClientes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null) return NotFound();

            ViewBag.SelectedId = id;

            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cliente = new Cliente
                {
                    Nome = model.Nome,
                    NIF = model.NIF,
                    CartaConducao = model.CartaConducao,
                    Email = model.Email,
                    Telemovel = model.Telemovel
                };

                _context.Add(cliente);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null) return NotFound();

            ViewBag.SelectedId = id;

            var viewModel = new ClienteViewModel
            {
                Nome = cliente.Nome,
                NIF = cliente.NIF,
                CartaConducao = cliente.CartaConducao,
                Email = cliente.Email,
                Telemovel = cliente.Telemovel
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel model)
        {
            if (!_context.Clientes.Select(x => x.Id).Contains(id))
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var cliente = await _context.Clientes.FindAsync(id);

                    if (cliente == null) return NotFound();

                    cliente.Nome = model.Nome;
                    cliente.NIF = model.NIF;
                    cliente.CartaConducao = model.CartaConducao;
                    cliente.Email = model.Email;
                    cliente.Telemovel = model.Telemovel;

                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Clientes.Any(e => e.Id == id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.SelectedId = id;
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null) return NotFound();

            ViewBag.SelectedId = id;

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}