(function (window, document) {
  'use strict';

  var cart = [];
  var E = {};

  function $(id) { return document.getElementById(id); }

  function fmt(n) { return Number(n || 0).toFixed(2); }

  function money(n) { return fmt(n) + ' ج.م'; }

  function showToast(msg, type) {
    if (!E.toast) return;
    E.toast.textContent = msg;
    E.toast.classList.toggle('pos-toast--error', type === 'error');
    E.toast.classList.toggle('pos-toast--success', type === 'success');
    E.toast.hidden = false;
    clearTimeout(showToast._t);
    showToast._t = setTimeout(function () { E.toast.hidden = true; }, 3200);
  }

  function cartItem(productId) {
    return cart.find(function (i) { return i.productId === productId; });
  }

  function cartSubtotal() {
    return cart.reduce(function (s, i) { return s + i.qty * i.price; }, 0);
  }

  function cartCount() {
    return cart.reduce(function (s, i) { return s + i.qty; }, 0);
  }

  function currentDiscount() {
    var d = parseFloat(E.discount.value);
    return isNaN(d) || d < 0 ? 0 : d;
  }

  function currentReceived() {
    var r = parseFloat(E.received.value);
    return isNaN(r) || r < 0 ? 0 : r;
  }

  function totals() {
    var subtotal = cartSubtotal();
    var discount = Math.min(currentDiscount(), subtotal);
    var total = Math.max(subtotal - discount, 0);
    var received = currentReceived();
    var change = received > total ? received - total : 0;
    return { subtotal: subtotal, discount: discount, total: total, received: received, change: change };
  }

  function renderTotals() {
    var t = totals();
    E.subtotal.textContent = money(t.subtotal);
    E.total.textContent = money(t.total);
    E.change.textContent = money(t.change);
    E.checkout.disabled = cart.length === 0;
    E.checkout.classList.toggle('is-loading', false);
  }

  function renderCart() {
    var container = E.cartItems;
    container.innerHTML = '';

    if (cart.length === 0) {
      container.innerHTML =
        '<div class="pos-cart__empty"><i class="bi bi-cart-plus" aria-hidden="true"></i>' +
        '<p>اضغط على منتج لإضافته إلى الفاتورة</p></div>';
    } else {
      cart.forEach(function (item) {
        container.appendChild(buildCartRow(item));
      });
    }

    E.cartCount.textContent = cartCount();
    renderTotals();
  }

  function buildCartRow(item) {
    var row = document.createElement('div');
    row.className = 'cart-item';
    row.dataset.productId = item.productId;

    var img = document.createElement('div');
    img.className = 'cart-item__img';
    if (item.imageUrl) {
      var im = document.createElement('img');
      im.src = item.imageUrl;
      im.alt = item.name;
      img.appendChild(im);
    } else {
      img.innerHTML = '<i class="bi bi-box-seam"></i>';
    }

    var body = document.createElement('div');
    body.className = 'cart-item__body';

    var name = document.createElement('div');
    name.className = 'cart-item__name';
    name.textContent = item.name;

    var row1 = document.createElement('div');
    row1.className = 'cart-item__row';

    var stepper = document.createElement('div');
    stepper.className = 'qty-stepper';
    var btnMinus = document.createElement('button');
    btnMinus.type = 'button';
    btnMinus.className = 'qty-stepper__btn js-dec';
    btnMinus.textContent = '−';
    btnMinus.disabled = item.qty <= 1;
    var qtyVal = document.createElement('span');
    qtyVal.className = 'qty-stepper__value';
    qtyVal.textContent = item.qty;
    var btnPlus = document.createElement('button');
    btnPlus.type = 'button';
    btnPlus.className = 'qty-stepper__btn js-inc';
    btnPlus.textContent = '+';
    btnPlus.disabled = item.qty >= item.stock;
    stepper.appendChild(btnMinus);
    stepper.appendChild(qtyVal);
    stepper.appendChild(btnPlus);

    var priceWrap = document.createElement('div');
    priceWrap.className = 'cart-item__price';
    var priceInput = document.createElement('input');
    priceInput.type = 'number';
    priceInput.min = '0';
    priceInput.step = '0.01';
    priceInput.value = fmt(item.price);
    priceInput.className = 'js-price';
    priceInput.setAttribute('inputmode', 'decimal');
    priceWrap.appendChild(priceInput);
    priceWrap.appendChild(document.createTextNode('ج.م'));

    row1.appendChild(stepper);
    row1.appendChild(priceWrap);

    var row2 = document.createElement('div');
    row2.className = 'cart-item__row';
    var line = document.createElement('span');
    line.className = 'cart-item__line';
    line.textContent = money(item.qty * item.price);
    var remove = document.createElement('button');
    remove.type = 'button';
    remove.className = 'cart-item__remove js-remove';
    remove.title = 'حذف';
    remove.innerHTML = '<i class="bi bi-trash3"></i>';
    row2.appendChild(line);
    row2.appendChild(remove);

    body.appendChild(name);
    body.appendChild(row1);
    body.appendChild(row2);

    row.appendChild(img);
    row.appendChild(body);
    return row;
  }

  function syncRowTotals(row, item) {
    var line = row.querySelector('.cart-item__line');
    var val = row.querySelector('.qty-stepper__value');
    var dec = row.querySelector('.js-dec');
    var inc = row.querySelector('.js-inc');
    if (line) line.textContent = money(item.qty * item.price);
    if (val) val.textContent = item.qty;
    if (dec) dec.disabled = item.qty <= 1;
    if (inc) inc.disabled = item.qty >= item.stock;
  }

  function addToCart(productId) {
    var card = E.products.querySelector('.pos-product[data-id="' + productId + '"]');
    if (!card || card.disabled) return;

    var item = cartItem(productId);
    if (item) {
      if (item.qty < item.stock) { item.qty += 1; }
    } else {
      var img = card.querySelector('img');
      cart.push({
        productId: productId,
        qty: 1,
        name: card.dataset.name,
        barcode: card.dataset.barcode,
        price: parseFloat(card.dataset.price) || 0,
        stock: parseInt(card.dataset.stock, 10) || 0,
        imageUrl: img ? img.src : ''
      });
    }
    renderCart();
  }

  function applyFilter() {
    var q = (E.search.value || '').trim().toLowerCase();
    var activeCat = '';
    var active = E.cats.querySelector('.pos-cat.is-active');
    if (active) activeCat = active.dataset.cat;

    var visible = 0;
    E.products.querySelectorAll('.pos-product').forEach(function (card) {
      var matchCat = !activeCat || card.dataset.cat === activeCat;
      var matchQ = !q ||
        (card.dataset.name || '').toLowerCase().indexOf(q) !== -1 ||
        (card.dataset.barcode || '').toLowerCase().indexOf(q) !== -1;
      var show = matchCat && matchQ;
      card.hidden = !show;
      if (show) visible++;
    });

    var empty = E.products.querySelector('.pos-empty');
    if (!empty) {
      empty = document.createElement('div');
      empty.className = 'pos-empty';
      empty.id = 'posEmptyMsg';
      empty.innerHTML = '<i class="bi bi-search" aria-hidden="true"></i><p>لا توجد منتجات مطابقة</p>';
      E.products.appendChild(empty);
    }
    empty.hidden = visible !== 0 || E.products.querySelectorAll('.pos-product').length === 0;
  }

  function scanBarcode() {
    var code = (E.scanner.value || '').trim().toLowerCase();
    E.scanner.value = '';
    if (!code) return;

    var card = null;
    E.products.querySelectorAll('.pos-product').forEach(function (c) {
      if ((c.dataset.barcode || '').toLowerCase() === code) card = c;
    });

    if (!card) {
      showToast('لا يوجد منتج بهذا الباركود', 'error');
      return;
    }
    if (card.disabled) {
      showToast('هذا المنتج نفذت كميةه', 'error');
      return;
    }
    addToCart(card.dataset.id);
  }

  function checkout() {
    if (cart.length === 0) return;

    var t = totals();
    var payload = {
      Items: cart.map(function (i) {
        return { productId: i.productId, quantity: i.qty, unitPrice: i.price };
      }),
      customerId: E.customer.value || null,
      notes: E.notes.value,
      warehouseId: E.warehouse ? (E.warehouse.value || null) : null,
      receivedAmount: t.received,
      discount: t.discount
    };

    E.checkout.disabled = true;
    E.checkout.classList.add('is-loading');

    var token = E.checkoutForm.querySelector('input[name="__RequestVerificationToken"]');
    fetch('/Pos/Checkout', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'RequestVerificationToken': token ? token.value : ''
      },
      body: JSON.stringify(payload)
    })
      .then(function (r) { return r.json(); })
      .then(function (res) {
        if (res.success) {
          showToast(res.message || 'تم إتمام البيع', 'success');
          setTimeout(function () {
            window.location.href = res.redirectUrl;
          }, 400);
        } else {
          showToast(res.message || 'فشل إتمام البيع', 'error');
          E.checkout.disabled = cart.length === 0;
          E.checkout.classList.remove('is-loading');
        }
      })
      .catch(function () {
        showToast('حدث خطأ في الاتصال بالخادم', 'error');
        E.checkout.disabled = cart.length === 0;
        E.checkout.classList.remove('is-loading');
      });
  }

  function newSale() {
    cart = [];
    E.discount.value = '0';
    E.received.value = '0';
    E.notes.value = '';
    E.customer.value = '';
    renderCart();
  }

  function bindEvents() {
    E.products.addEventListener('click', function (ev) {
      var card = ev.target.closest('.pos-product');
      if (card) addToCart(card.dataset.id);
    });

    E.cats.addEventListener('click', function (ev) {
      var btn = ev.target.closest('.pos-cat');
      if (!btn) return;
      E.cats.querySelectorAll('.pos-cat').forEach(function (c) { c.classList.remove('is-active'); });
      btn.classList.add('is-active');
      applyFilter();
    });

    E.search.addEventListener('input', applyFilter);

    E.scanner.addEventListener('keydown', function (ev) {
      if (ev.key === 'Enter') { ev.preventDefault(); scanBarcode(); }
    });

    E.cartItems.addEventListener('click', function (ev) {
      var row = ev.target.closest('.cart-item');
      if (!row) return;
      var item = cartItem(row.dataset.productId);
      if (!item) return;

      if (ev.target.closest('.js-inc')) {
        if (item.qty < item.stock) item.qty += 1;
      } else if (ev.target.closest('.js-dec')) {
        if (item.qty > 1) item.qty -= 1;
      } else if (ev.target.closest('.js-remove')) {
        cart = cart.filter(function (i) { return i.productId !== item.productId; });
      } else {
        return;
      }
      renderCart();
    });

    E.cartItems.addEventListener('change', function (ev) {
      var input = ev.target.closest('.js-price');
      if (!input) return;
      var row = ev.target.closest('.cart-item');
      var item = cartItem(row.dataset.productId);
      if (!item) return;
      var v = parseFloat(input.value);
      item.price = isNaN(v) || v <= 0 ? item.price : v;
      input.value = fmt(item.price);
      renderCart();
    });

    E.discount.addEventListener('input', renderTotals);
    E.received.addEventListener('input', renderTotals);

    E.checkoutForm.addEventListener('submit', function (ev) {
      ev.preventDefault();
      checkout();
    });

    E.newSale.addEventListener('click', newSale);

    if (E.warehouse) {
      E.warehouse.addEventListener('change', function () {
        if (E.warehouse.value) {
          window.location.href = '/Pos?warehouse=' + encodeURIComponent(E.warehouse.value);
        }
      });
    }

    E.historyToggle.addEventListener('click', function () {
      E.history.hidden = !E.history.hidden;
    });
    E.historyClose.addEventListener('click', function () {
      E.history.hidden = true;
    });

    document.addEventListener('keydown', function (ev) {
      if (ev.key === 'F4') {
        ev.preventDefault();
        if (E.checkout.disabled) {
          newSale();
        } else {
          checkout();
        }
      }
    });
  }

  function init(config) {
    Object.keys(config).forEach(function (k) { E[k] = $(config[k]); });
    if (!E.products || !E.cartItems) return;
    bindEvents();
    applyFilter();
    renderCart();
  }

  window.PosApp = { init: init };
})(window, document);