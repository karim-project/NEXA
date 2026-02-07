using Mapster;
using Microsoft.AspNetCore.Mvc;
using NEXA.DTOs.Request;
using NEXA.DTOs.Response;
using NEXA.Models;
using NEXA.Repositories.IRepository;

namespace NEXA.Areas.Admin.Controllers
{
    namespace NEXA.Areas.Admin.Controllers
    {
        [Area("Admin")]
        public class AnswersController : Controller
        {
            private readonly IRepository<Answer> _answerRepository;
            private readonly IRepository<Question> _questionRepository;

            public AnswersController(
                IRepository<Answer> answerRepository,
                IRepository<Question> questionRepository)
            {
                _answerRepository = answerRepository;
                _questionRepository = questionRepository;
            }

            // ==================== Index ======================
            public async Task<IActionResult> Index(int questionId, CancellationToken cancellationToken)
            {
                var answers = await _answerRepository.GetAsync(
                    a => a.QuestionId == questionId,
                    tracked: false,
                    cancellationToken: cancellationToken);

                ViewBag.QuestionId = questionId;

                var response = answers.Select(a => new AnswerResponse
                {
                    Id = a!.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                });

                return View(response);
            }

            // ==================== Details ======================
            public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
            {
                var answer = await _answerRepository.GetOneAsync(
                    a => a.Id == id,
                    tracked: false,
                    cancellationToken: cancellationToken);

                if (answer is null)
                    return NotFound();

                var response = answer.Adapt<AnswerResponse>();
                return View(response);
            }

            // ==================== Create ======================
            [HttpGet]
            public IActionResult Create(int questionId, CancellationToken cancellationToken)
            {
                ViewBag.QuestionId = questionId;
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(
                CreateAnswerRequest model,
                CancellationToken cancellationToken)
            {
                if (!ModelState.IsValid)
                    return View(model);

                var answer = model.Adapt<Answer>();

                await _answerRepository.AddAsync(answer, cancellationToken);
                await _answerRepository.CommitAsync(cancellationToken);

                TempData["success"] = "Answer created successfully";
                return RedirectToAction(nameof(Index), new { questionId = model.QuestionId });
            }

            // ==================== Edit ======================
            [HttpGet]
            public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
            {
                var answer = await _answerRepository.GetOneAsync(
                    a => a.Id == id,
                    tracked: false,
                    cancellationToken: cancellationToken);

                if (answer is null)
                    return NotFound();

                var model = answer.Adapt<UpdateAnswerRequest>();
                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(
                int id,
                UpdateAnswerRequest model,
                CancellationToken cancellationToken)
            {
                if (!ModelState.IsValid)
                    return View(model);

                var answer = await _answerRepository.GetOneAsync(
                    a => a.Id == id,
                    cancellationToken: cancellationToken);

                if (answer is null)
                    return NotFound();

                answer.Text = model.Text;
                answer.IsCorrect = model.IsCorrect;

                _answerRepository.Update(answer);
                await _answerRepository.CommitAsync(cancellationToken);

                TempData["success"] = "Answer updated successfully";
                return RedirectToAction(nameof(Index), new { questionId = answer.QuestionId });
            }

            // ==================== Delete ======================
            [HttpGet]
            public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
            {
                var answer = await _answerRepository.GetOneAsync(
                    a => a.Id == id,
                    tracked: false,
                    cancellationToken: cancellationToken);

                if (answer is null)
                    return NotFound();

                var response = answer.Adapt<AnswerResponse>();
                return View(response);
            }

            [HttpPost, ActionName("Delete")]
            public async Task<IActionResult> DeleteConfirmed(
                int id,
                CancellationToken cancellationToken)
            {
                var answer = await _answerRepository.GetOneAsync(
                    a => a.Id == id,
                    cancellationToken: cancellationToken);

                if (answer is null)
                    return NotFound();

                _answerRepository.Delete(answer);
                await _answerRepository.CommitAsync(cancellationToken);

                TempData["success"] = "Answer deleted successfully";
                return RedirectToAction(nameof(Index), new { questionId = answer.QuestionId });
            }
        }
    }

}
