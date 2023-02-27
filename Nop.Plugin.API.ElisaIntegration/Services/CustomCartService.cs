using Nop.Data;
using Nop.Plugin.API.ElisaIntegration.Domain;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nop.Plugin.API.ElisaIntegration.Services
{
    public class CustomCartService
    {
        #region Fields
        private readonly IRepository<CustomCart> _customCartRepository;
        private readonly IRepository<CustomCartItems> _customCartItemsRepository;
        private readonly IEventPublisher _eventPublisher;
        #endregion

        #region Ctor
        public CustomCartService(IRepository<CustomCart> customCartRepository,
            IRepository<CustomCartItems> customCartItemsRepository,
            IEventPublisher eventPublisher) 
        {
            _customCartRepository = customCartRepository;
            _customCartItemsRepository = customCartItemsRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        #region Custom cart
        public void DeleteCustomCart(CustomCart cart) 
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            _customCartRepository.Delete(cart);

            //event notification
            _eventPublisher.EntityDeleted(cart);
        }

        public CustomCart GetCustomCartByElisaCartId(Guid elisaCartId) 
        {
            if (elisaCartId == null || elisaCartId == Guid.Empty)
                return null;

            var customCart = (from ec in _customCartRepository.Table
                         where ec.ElisaCartId == elisaCartId
                         select ec).FirstOrDefault();

            return customCart;
        }

        public CustomCart GetFirstCustomCart()
        {
            return _customCartRepository.Table.OrderBy(x => x.Id).FirstOrDefault();
        }

        public void InsertCustomCart(CustomCart cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            _customCartRepository.Insert(cart);

            //event notification
            _eventPublisher.EntityInserted(cart);
        }

        public void UpdateCustomCart(CustomCart cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            _customCartRepository.Update(cart);

            //event notification
            _eventPublisher.EntityUpdated(cart);
        }

        #endregion

        #region Custom cart items
        public void DeleteCustomCartItems(IList<CustomCartItems> cartItems)
        {
            if (cartItems == null)
                throw new ArgumentNullException(nameof(cartItems));

            _customCartItemsRepository.Delete(cartItems);

        }

        public IList<CustomCartItems> GetCustomCartItemsByCartId(Guid cartId)
        {
            if (cartId == null || cartId == Guid.Empty)
                return null;

            var items = (from ec in _customCartItemsRepository.Table
                              where ec.CustomCartId == cartId
                              select ec).ToList();

            return items;
        }

        public void InsertCustomCartItem(CustomCartItems cartItems)
        {
            if (cartItems == null)
                throw new ArgumentNullException(nameof(cartItems));

            _customCartItemsRepository.Insert(cartItems);

            //event notification
            _eventPublisher.EntityInserted(cartItems);
        }

        public void UpdateCustomCartItem(CustomCartItems cartItems)
        {
            if (cartItems == null)
                throw new ArgumentNullException(nameof(cartItems));

            _customCartItemsRepository.Update(cartItems);

            //event notification
            _eventPublisher.EntityUpdated(cartItems);
        }
        #endregion

        #endregion
    }
}
