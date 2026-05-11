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
    public class CamioesController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public CamioesController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // GET: Camioes
        public async Task<IActionResult> Index()
        {
            var projeto_AutoMobileContext = _context.Camioes.Include(c => c.Empresa);
            return View(await projeto_AutoMobileContext.ToListAsync());
        }

        // GET: Camioes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camiao = await _context.Camioes
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camiao == null)
            {
                return NotFound();
            }

            return View(camiao);
        }

        // GET: Camioes/Create
        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id");
            return View();
        }

        // POST: Camioes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaxCarga,Id,Matricula,Marca,Modelo,PrecoDia,Estado,DataDisponibilidade,EmpresaId")] Camiao camiao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(camiao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", camiao.EmpresaId);
            return View(camiao);
        }

        // GET: Camioes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camiao = await _context.Camioes.FindAsync(id);
            if (camiao == null)
            {
                return NotFound();
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", camiao.EmpresaId);
            return View(camiao);
        }

        // POST: Camioes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaxCarga,Id,Matricula,Marca,Modelo,PrecoDia,Estado,DataDisponibilidade,EmpresaId")] Camiao camiao)
        {
            if (id != camiao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camiao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamiaoExists(camiao.Id))
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
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", camiao.EmpresaId);
            return View(camiao);
        }

        // GET: Camioes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camiao = await _context.Camioes
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camiao == null)
            {
                return NotFound();
            }

            return View(camiao);
        }

        // POST: Camioes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camiao = await _context.Camioes.FindAsync(id);
            if (camiao != null)
            {
                _context.Camioes.Remove(camiao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CamiaoExists(int id)
        {
            return _context.Camioes.Any(e => e.Id == id);
        }
    }
}
