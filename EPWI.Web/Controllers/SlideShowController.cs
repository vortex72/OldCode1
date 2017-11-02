using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPWI.Components.Models;
using xVal.ServerSide;

namespace EPWI.Web.Controllers
{
  public class SlideShowController : EpwiController
  {
    private SlideShowRepository rep = new SlideShowRepository();
    public ActionResult SlideShow()
    {
      var slides = rep.GetSlides(false);
      return View(slides);
    }

    [OutputCache(VaryByParam="id", Duration=300)]
    public ActionResult Slide(int id)
    {
      var slide = rep.GetSlideByID(id);

      if (slide == null)
      {
        Response.StatusCode = 404;
        return new EmptyResult();
      }
      
      return File(slide.ImageData.ToArray(), slide.ImageMimeType);
    }

    public ActionResult UncachedSlide(int id)
    {
      var slide = rep.GetSlideByID(id);

      if (slide == null)
      {
        return new EmptyResult();
      }
      
      return File(slide.ImageData.ToArray(), slide.ImageMimeType);
    }

    [Authorize(Roles = "ADMIN")]
    public ActionResult Index()
    {
      return View(rep.GetAllSlides());
    }

    [Authorize(Roles = "ADMIN")]
    public ActionResult Delete(int id)
    {
      rep.DeleteSlide(id);
      TempData["message"] = "Slide deleted.";
      
      return RedirectToAction("Index");
    }

    [Authorize(Roles = "ADMIN")]
    public ActionResult Edit(int id)
    {
      return View(rep.GetSlideByID(id));
    }

    [Authorize(Roles = "ADMIN")]
    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Edit(Slideshow slide, HttpPostedFileBase image)
    {
      if (image != null)
      {
        slide.ImageMimeType = image.ContentType;
        slide.ImageData = new byte[image.ContentLength];
        using (var reader = new BinaryReader(image.InputStream))
        {
            slide.ImageData = reader.ReadBytes(image.ContentLength);
        }

        //image.InputStream.Read(slide.ImageData.ToArray(), 0, image.ContentLength);
      }

      try
      {
        rep.SaveSlide(slide);
        rep.Save();
      }
      catch (RulesException ex)
      {
        ex.AddModelStateErrors(ModelState, null);
        return View(slide);
      }

      TempData["message"] = "Slide saved.";

      return RedirectToAction("Index");
    }

    [Authorize(Roles = "ADMIN")]
    public ActionResult Create()
    {
      return View("Edit", new Slideshow { Enabled = true });
    }
  }
}
