﻿// Copyright (c) DNN Software. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;
using System.Web.Mvc;
using Dnn.ContactList.Api;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;

namespace Dnn.ContactList.Mvc.Controllers
{
    /// <summary>
    /// ContactController is the MVC Controller class for managing Contacts in the UI
    /// </summary>
    public class ContactController : DnnController
    {
        private readonly IContactRepository _repository;

        /// <summary>
        /// Default Constructor constructs a new ContactController
        /// </summary>
        public ContactController() : this(ContactRepository.Instance) { }

        /// <summary>
        /// Constructor constructs a new ContactController with a passed in repository
        /// </summary>
        public ContactController(IContactRepository repository)
        {
            Requires.NotNull(repository);

            _repository = repository;
        }

        /// <summary>
        /// The Delete method is used to delete a Contact
        /// </summary>
        /// <param name="contactId">The Id of the Contact to delete</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int contactId)
        {
            var contact = _repository.GetContact(contactId, PortalSettings.PortalId);

            _repository.DeleteContact(contact);

            return RedirectToDefaultRoute();
        }

        /// <summary>
        /// This Edit method is used to set up editing a Contact
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int contactId = -1)
        {
            var contact = (contactId == -1)
                        ? new Contact { PortalId = PortalSettings.PortalId }
                        : _repository.GetContact(contactId, PortalSettings.PortalId);

            return View(contact);
        }

        /// <summary>
        /// This Edit method is used to save the posted Contact
        /// </summary>
        /// <param name="contact">The contact to save</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Contact contact)
        {
            if (contact.ContactId == -1)
            {
                _repository.AddContact(contact);
            }
            else
            {
                _repository.UpdateContact(contact);
            }

            return RedirectToDefaultRoute();
        }

        /// <summary>
        /// The Index method is the default Action method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAction(ControlKey = "Edit", TitleKey = "AddContact")]
        public ActionResult Index()
        {
            var contacts = _repository.GetContacts(PortalSettings.PortalId);

            return View(contacts.ToList());
        }
    }
}
