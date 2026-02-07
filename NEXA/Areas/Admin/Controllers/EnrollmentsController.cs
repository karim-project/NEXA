using Microsoft.AspNetCore.Mvc;
using NEXA.Models;
using NEXA.Repositories.IRepository;

namespace NEXA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EnrollmentsController : Controller
    {
        private readonly IRepository<Enrollment> _enrollmentRepository;

        public EnrollmentsController(IRepository<Enrollment> enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        // ==================== Index ======================
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var enrollments = await _enrollmentRepository.GetAsync(
                includes: [e => e.Course],
                tracked: false,
                cancellationToken: cancellationToken);

            return View(enrollments);
        }

        // ==================== Details ======================
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepository.GetOneAsync(
                e => e.Id == id,
               includes: [e => e.Course],
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        // ==================== Create ======================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Enrollment model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.EnrolledAt = DateTime.UtcNow;

            await _enrollmentRepository.AddAsync(model, cancellationToken);
            await _enrollmentRepository.CommitAsync(cancellationToken);

            TempData["success"] = "Enrollment created successfully";
            return RedirectToAction(nameof(Index));
        }

        // ==================== Edit ======================
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepository.GetOneAsync(
                e => e.Id == id,
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Enrollment model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(model);

            var enrollment = await _enrollmentRepository.GetOneAsync(
                e => e.Id == id,
                cancellationToken: cancellationToken);

            if (enrollment == null)
                return NotFound();

            enrollment.UserId = model.UserId;
            enrollment.CourseId = model.CourseId;

            _enrollmentRepository.Update(enrollment);
            await _enrollmentRepository.CommitAsync(cancellationToken);

            TempData["success"] = "Enrollment updated successfully";
            return RedirectToAction(nameof(Index));
        }

        // ==================== Delete ======================
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepository.GetOneAsync(
                e => e.Id == id,
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var enrollment = await _enrollmentRepository.GetOneAsync(
                e => e.Id == id,
                cancellationToken: cancellationToken);

            if (enrollment == null)
                return NotFound();

            _enrollmentRepository.Delete(enrollment);
            await _enrollmentRepository.CommitAsync(cancellationToken);

            TempData["success"] = "Enrollment deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}

