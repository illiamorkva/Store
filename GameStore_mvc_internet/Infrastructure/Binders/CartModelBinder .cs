﻿using GameStore_mvc_internet.Models;
using System.Web.Mvc;

namespace GameStore_mvc_internet.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string SessionKey = "Cart";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            // Получить объект Cart из сеанса
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[SessionKey];
            }

            // Создать объект Cart если он не обнаружен в сеансе
            if (cart == null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[SessionKey] = cart;
                }
            }

            // Возвратить объект Cart
            return cart;
        }
    }
}