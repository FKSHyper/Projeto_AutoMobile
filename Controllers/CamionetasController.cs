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
    public class CamionetasController : Controller
    {
        private readonly Projeto_AutoMobileContext _context;

        public CamionetasController(Projeto_AutoMobileContext context)
        {
            _context = context;
        }

        // GET: Camionetas
        public async Task<IActionResult> Index()
        {
            var projeto_AutoMobileContext = _context.Camionetas.Include(c => c.Empresa);
            return View(await projeto_AutoMobileContext.ToListAsync());
        }

        // GET: Camionetas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camioneta = await _context.Camionetas
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camioneta == null)
            {
                return NotFound();
            }

            return View(camioneta);
        }

        // GET: Camionetas/Create
        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id");
            return View();
        }

        // POST: Camionetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaxPassageiros,Eixos,Id,Matricula,Marca,Modelo,PrecoDia,Estado,DataDisponibilidade,EmpresaId")] Camioneta camioneta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(camioneta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", camioneta.EmpresaId);
            return View(camioneta);
        }

        // GET: Camionetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camioneta = await _context.Camionetas.FindAsync(id);
            if (camioneta == null)
            {
                return NotFound();
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", camioneta.EmpresaId);
            return View(camioneta);
        }

        // POST: Camionetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaxPassageiros,Eixos,Id,Matricula,Marca,Modelo,PrecoDia,Estado,DataDisponibilidade,EmpresaId")] Camioneta camioneta)
        {
            if (id != camioneta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camioneta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CamionetaExists(camioneta.Id))
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
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", camioneta.EmpresaId);
            return View(camioneta);
        }

        // GET: Camionetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var camioneta = await _context.Camionetas
                .Include(c => c.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camioneta == null)
            {
                return NotFound();
            }

            return View(camioneta);
        }

        // POST: Camionetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var camioneta = await _context.Camionetas.FindAsync(id);
            if (camioneta != null)
            {
                _context.Camionetas.Remove(camioneta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CamionetaExists(int id)
        {
            return _context.Camionetas.Any(e => e.Id == id);
        }
    }
}
