﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Business.DataAccess.Public.Entities;
using Project.WebUI.Models;

namespace Project.WebUI.Controllers
{
    public partial class RentController : Controller
    {
        public ActionResult EditApartment(long id = 0)
        {
            EditApartmentViewResult result = new EditApartmentViewResult();
            result.Apartment = _apartmentRepository.GetApartment(id);
            result.ApartmentOptions = _apartmentRepository.GetApartmentOptions();
            if (result.Apartment == null)
            {
                result.Apartment = new Apartment();
                result.Apartment.ApartmentOptions = new List<ApartmentOption>();
                result.Apartment.ApartmentPhotos = new List<ApartmentPhoto>();
            }
            result.ApartmentTypes = _apartmentRepository.ApartmentTypes.ToList();
            return View(result);
        }
       
        [HttpPost]
        public ActionResult EditApartment(EditApartmentViewResult newApartment)
        {
            bool newApart = false;
            if (ModelState.IsValid)
            {
                long curId, newId;
                curId = newApartment.Apartment.ApartmentId;
                newApartment.Apartment.ProfileId = user.ProfileId;
                if (newApartment.Apartment.CreateDate==null)
                {
                    newApartment.Apartment.CreateDate = DateTime.Now;
                }
                if (newApartment.Apartment.UpdateDate == null)
                {
                    newApartment.Apartment.UpdateDate = newApartment.Apartment.CreateDate;
                }
                _apartmentRepository.UpdateApartment(newApartment.Apartment);
                newId = newApartment.Apartment.ApartmentId;
                _apartmentRepository.ClearApartmentOptions(newApartment.Apartment);
                foreach (var option in newApartment.SelectedOptions.Where(m => m.Checked == true))
                {
                    _apartmentRepository.AddOption(newApartment.Apartment, new ApartmentOption { ApartmentOptionId = option.ApartmentOptionId });
                }
                if ((curId == 0) && (newId != 0))
                    newApart = true;
                TempData["toastrMessage"] = String.Format("Квартира изменена");
                TempData["toastrType"] = "success";
                //return View(newApartment);
            }
            else
            {
                TempData["toastrMessage"] = String.Format("Ошибка в данных квартиры");
                TempData["toastrType"] = "error";
            }
            newApartment.ApartmentTypes = _apartmentRepository.ApartmentTypes.ToList();
            newApartment.ApartmentOptions = _apartmentRepository.GetApartmentOptions();
            newApartment.Apartment.ApartmentOptions = new List<ApartmentOption>();
            foreach (var option in newApartment.SelectedOptions.Where(m => m.Checked == true))
            {
                newApartment.Apartment.ApartmentOptions.Add(newApartment.ApartmentOptions.Where(m => m.ApartmentOptionId == option.ApartmentOptionId).FirstOrDefault());
            }
            if (newApart)
            {
                TempData["toastrMessage"] = String.Format("Квартира добавлена");
                TempData["toastrType"] = "success";
                return RedirectToAction("EditApartment", new { id = newApartment.Apartment.ApartmentId });
            }
            return View(newApartment);
        }       
	}
}