using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_AutoMobile.Data;
using Projeto_AutoMobile.Data.Projeto_AutoMobile;

namespace Projeto_AutoMobile.Controllers
{
    public class MotasController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public MotasController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // GET: Motas
        public async Task<IActionResult> Index()
        {
            var projeto_AutoMobileContext = _context.Motas.Include(m => m.Empresa);
            return View(await projeto_AutoMobileContext.ToListAsync());
        }

        // GET: Motas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mota = await _context.Motas
                .Include(m => m.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mota == null)
            {
                return NotFound();
            }

            return View(mota);
        }

        // GET: Motas/Create
        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id");
            return View();
        }

        // POST: Motas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Projeto_AutoMobile.ViewModels.Mota.MotaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var mota = new Mota
                {
                    Matricula = viewModel.Matricula,
                    Marca = viewModel.Marca,
                    Modelo = viewModel.Modelo,
                    PrecoDia = viewModel.PrecoDia,
                    Cilindrada = viewModel.Cilindrada,
                    Estado = EstadoVeiculo.Disponivel, //ao criar o carro quero que o estado seja Disponivel
                    DataDisponibilidade = DateTime.Now, //a DataDisponibilidade vai ser o dia em que for criado
                    EmpresaId = 1 //Colocar um valor senao vai dar erro na bd - EmpresaId é FK
                };

                _context.Add(mota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Motas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mota = await _context.Motas.FindAsync(id);
            if (mota == null)
            {
                return NotFound();
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", mota.EmpresaId);
            return View(mota);
        }

        // POST: Motas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cilindrada,Id,Matricula,Marca,Modelo,PrecoDia,Estado,DataDisponibilidade,EmpresaId")] Mota mota)
        {
            if (id != mota.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MotaExists(mota.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", mota.EmpresaId);
            return View(mota);
        }

        // GET: Motas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mota = await _context.Motas
                .Include(m => m.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mota == null)
            {
                return NotFound();
            }

            return View(mota);
        }

        // POST: Motas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mota = await _context.Motas.FindAsync(id);
            if (mota != null)
            {
                _context.Motas.Remove(mota);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MotaExists(int id)
        {
            return _context.Motas.Any(e => e.Id == id);
        }
    }
}
