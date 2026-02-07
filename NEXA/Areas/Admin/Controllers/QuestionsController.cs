using Microsoft.AspNetCore.Mvc;

namespace NEXA.Areas.Admin.Controllers
{
    using global::NEXA.Models;
    using global::NEXA.Repositories.IRepository;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        // ✅ READ – Get All
        public async Task<IActionResult> Index()
        {
            var questions = await _questionRepository.GetAllAsync();
            return View(questions);
        }

        // ✅ READ – Details
        public async Task<IActionResult> Details(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                return NotFound();

            return View(question);
        }

        // ✅ CREATE – GET
        public IActionResult Create()
        {
            return View();
        }

        // ✅ CREATE – POST
        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            if (!ModelState.IsValid)
                return View(question);

            await _questionRepository.AddAsync(question);
            return RedirectToAction(nameof(Index));
        }

        // ✅ UPDATE – GET
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                return NotFound();

            return View(question);
        }

        // ✅ UPDATE – POST
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Question question)
        {
            if (id != question.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(question);

            await _questionRepository.UpdateAsync(question);
            return RedirectToAction(nameof(Index));
        }

        // ✅ DELETE – GET
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                return NotFound();

            return View(question);
        }

        // ✅ DELETE – POST
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                return NotFound();

            await _questionRepository.DeleteAsync(question);
            return RedirectToAction(nameof(Index));
        }
    }

}
