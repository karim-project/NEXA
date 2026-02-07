using Mapster;
using Microsoft.AspNetCore.Mvc;
using NEXA.DTOs.Request;
using NEXA.DTOs.Response;
using NEXA.Models;
using NEXA.Repositories.IRepository;

namespace NEXA.Areas.Admin.Controllers

    {
        [Area("Admin")]
        public class CertificatesController : Controller
        {
            private readonly IRepository<Certificate> _certificateRepository;
            private readonly IRepository<Course> _courseRepository;
            private readonly IRepository<ApplicationUser> _userRepository;

            public CertificatesController(
                IRepository<Certificate> certificateRepository,
                IRepository<Course> courseRepository,
                IRepository<ApplicationUser> userRepository)
            {
                _certificateRepository = certificateRepository;
                _courseRepository = courseRepository;
                _userRepository = userRepository;
            }

            // ==================== Index ======================
            public async Task<IActionResult> Index(CancellationToken cancellationToken)
            {
                var certificates = await _certificateRepository.GetAsync(
                    includes: [c=>c.User , c=>c.Course],
                    tracked: false,
                    cancellationToken: cancellationToken);

                var response = certificates.Select(c => new CertificateResponse
                {
                    Id = c!.Id,
                    UserName = c.User.UserName!,
                    CourseTitle = c.Course.Title,
                    IssuedAt = c.IssuedAt
                });

                return View(response);
            }

            // ==================== Details ======================
            public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
            {
                var certificate = await _certificateRepository.GetOneAsync(
                    c => c.Id == id,
                    includes: [c => c.User, c => c.Course],
                    tracked: false,
                    cancellationToken: cancellationToken);

                if (certificate is null)
                    return NotFound();

                var response = certificate.Adapt<CertificateResponse>();
                return View(response);
            }

            // ==================== Create ======================
            [HttpGet]
            public async Task<IActionResult> Create(CancellationToken cancellationToken)
            {
                ViewBag.Users = await _userRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
                ViewBag.Courses = await _courseRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);

                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(
                CreateCertificateRequest model,
                CancellationToken cancellationToken)
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Users = await _userRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
                    ViewBag.Courses = await _courseRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
                    return View(model);
                }

                var certificate = model.Adapt<Certificate>();
                certificate.IssuedAt = DateTime.UtcNow;

                await _certificateRepository.AddAsync(certificate, cancellationToken);
                await _certificateRepository.CommitAsync(cancellationToken);

                TempData["success"] = "Certificate created successfully";
                return RedirectToAction(nameof(Index));
            }

            // ==================== Delete ======================
            [HttpGet]
            public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
            {
                var certificate = await _certificateRepository.GetOneAsync(
                    c => c.Id == id,
                    includes: [c => c.User, c => c.Course],
                    tracked: false,
                    cancellationToken: cancellationToken);

                if (certificate is null)
                    return NotFound();

                var response = certificate.Adapt<CertificateResponse>();
                return View(response);
            }

            [HttpPost, ActionName("Delete")]
            public async Task<IActionResult> DeleteConfirmed(
                int id,
                CancellationToken cancellationToken)
            {
                var certificate = await _certificateRepository.GetOneAsync(
                    c => c.Id == id,
                    cancellationToken: cancellationToken);

                if (certificate is null)
                    return NotFound();

                _certificateRepository.Delete(certificate);
                await _certificateRepository.CommitAsync(cancellationToken);

                TempData["success"] = "Certificate deleted successfully";
                return RedirectToAction(nameof(Index));
            }
        }
    }


