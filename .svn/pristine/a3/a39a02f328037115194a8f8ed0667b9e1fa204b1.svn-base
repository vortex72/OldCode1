using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Helpers;
using xVal.ServerSide;

namespace EPWI.Components.Models
{
  public class SlideShowRepository : Repository
  {
    public IEnumerable<Slideshow> GetSlides(bool includeRegisteredUsersOnlySlides)
    {
      var slides = from s in Db.Slideshows
                   where s.Enabled
                   orderby s.SlideShowID descending 
                   select s;

      if (!includeRegisteredUsersOnlySlides)
      {
        return slides.Where(s => !s.RegisteredOnly);
      }
       
      return slides;
    }

    public Slideshow GetSlideByID(int id)
    {
      return Db.Slideshows.SingleOrDefault(s => s.SlideShowID == id);
    }

    public IEnumerable<Slideshow> GetAllSlides()
    {
      return (from s in Db.Slideshows select s);
    }

    public void DeleteSlide(int id)
    {
      var slideToDelete = (from s in Db.Slideshows where s.SlideShowID == id select s).SingleOrDefault();

      if (slideToDelete != null)
      {
        Db.Slideshows.DeleteOnSubmit(slideToDelete);
        this.Save();
      }
    }

    public void SaveSlide(Slideshow slide)
    {
      var errors = new List<ErrorInfo>();
      var newSlide = slide.SlideShowID == 0;

      errors.AddRange(DataAnnotationsValidationRunner.GetErrors(slide));
      
      if (newSlide)
      {
        // need an image if it is a new slide
        if (slide.ImageData == null)
        {
          errors.Add(new ErrorInfo("Image", "An image is required for new slides."));
        }
      }

      if (slide.ExternalLink && !slide.Link.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
      {
        errors.Add(new ErrorInfo("Link", "External links must start with 'http'"));
      }

      if (errors.Any())
      {
        throw new RulesException(errors);
      }

      if (newSlide) 
      {
        Db.Slideshows.InsertOnSubmit(slide);
      }
      else
      {
        var existingSlide = GetSlideByID(slide.SlideShowID);

        if (slide.ImageData != null)
        {
          existingSlide.ImageData = slide.ImageData;
          existingSlide.ImageMimeType = slide.ImageMimeType;
        }

        existingSlide.Caption = slide.Caption;
        existingSlide.Enabled = slide.Enabled;
        existingSlide.ExternalLink = slide.ExternalLink;
        existingSlide.Link = slide.Link;
        existingSlide.RegisteredOnly = slide.RegisteredOnly;
      }
    }
  }
}
