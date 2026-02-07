using Mapster;
using Microsoft.AspNetCore.Mvc;
using NEXA.DTOs.Request;
using NEXA.DTOs.Response;
using NEXA.Models;
using NEXA.Repositories.IRepository;
using System.Threading.Tasks;

namespace NEXA.Areas.Admin.Controllers
{
    public class CourseController : Controller
    {
        private readonly IRepository<Course> _courseRepositry;

        public CourseController(IRepository<Course> courseRepositry) 
        {
            _courseRepositry = courseRepositry;
        }

        //=============== Index ======================
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var courses =await _courseRepositry.GetAsync(tracked: false, cancellationToken: cancellationToken);

            var response = courses.Select(c => new Course
            {
                Id = c!.Id,
                Title = c!.Title,
                Description = c!.Description,
            });
            return View(response);
        }


        //=============== Details ======================

        public async Task<IActionResult> Details(int id , CancellationToken cancellationToken) 
        {
            var course =await _courseRepositry.GetOneAsync(c => c.Id == id, tracked: false, cancellationToken: cancellationToken);

            if(course is null)
                return NotFound();

            var response = course.Adapt<CourseResponse>();

        return View(response);
        }


        //=============== Create ======================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseRequest createCourseRequest ,CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
                return View(createCourseRequest);

            var course = createCourseRequest.Adapt<Course>();

           await _courseRepositry.AddAsync(course, cancellationToken);
           await _courseRepositry.CommitAsync(cancellationToken);

            TempData["success"] = "Course created successfully";

            return RedirectToAction(nameof(Index));
        }


        //=============== Edit ======================

        [HttpGet]
        public async Task<IActionResult> Edit(int id , CancellationToken cancellationToken) 
        {
            var course =await _courseRepositry.GetOneAsync(c => c.Id == id, tracked: false, cancellationToken: cancellationToken);

            if(course is null) return NotFound();

            var model = course.Adapt<UpdateCourseRequest>();

            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id , UpdateCourseRequest updateCourseRequest , CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
                return View(updateCourseRequest);

            var course =await _courseRepositry.GetOneAsync(d => d.Id == id, cancellationToken: cancellationToken);

            if(course is null) return NotFound();

            course.Title = updateCourseRequest.Title;
            course.Description = updateCourseRequest.Description;

            _courseRepositry.Update(course);
           await _courseRepositry.CommitAsync(cancellationToken);


            TempData["success"] = "Course updated successfully";
            return RedirectToAction(nameof(Index));
        }


        //=============== Delete ======================
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var course = await _courseRepositry.GetOneAsync(
                c => c.Id == id,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return NotFound();

            var response = course.Adapt<CourseResponse>();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(
            int id,
            CancellationToken cancellationToken)
        {
            var course = await _courseRepositry.GetOneAsync(
                c => c.Id == id,
                cancellationToken: cancellationToken);

            if (course is null)
                return NotFound();

            _courseRepositry.Delete(course);
            await _courseRepositry.CommitAsync(cancellationToken);

            TempData["success"] = "Course deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
