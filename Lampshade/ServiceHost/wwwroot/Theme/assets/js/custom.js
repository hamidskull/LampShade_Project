﻿const cookieName = "cart-items";

function addToCart(id, name, price, picture) {
    debugger;
    let products = $.cookie(cookieName);
    if (products === undefined) {
        products = [];
    } else {
        products = JSON.parse(products);
    }
    const count = $("#productCount").val();
    const currentProducts = products.find(x => x.id === id);
    if (currentProducts !== undefined) {
        products.find(x => x.id === id).count = parseInt(currentProducts.count) + parseInt(count);
    } else {
        const product = {
            id,
            name,
            unitPrice: price,
            picture,
            count
        }
        products.push(product);
    }

    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
}

function updateCart() {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    $("#cart_items_count").text(products.length);

    const cartItemWrapper = $("#cart_items_wrapper");
    cartItemWrapper.html('');
    products.forEach(product => {
        const productHTML = `<div class="single-cart-item">
                                                <a href="javascript:void(0)" class="remove-icon" onclick="removeFromCart(${product.id},false)">
                                                    <i class="ion-android-close"></i>
                                                </a>
                                                <div class="image">
                                                    <a href="">
                                                        <img src="/SitePictures/${product.picture}" class="img-fluid" alt="" />
                                                    </a>
                                                </div>
                                                <div class="content">
                                                    <p class="product-title">
                                                        <a href="">محصول: ${product.name} </a>
                                                    </p>
                                                    <p class="count"> تعداد: ${product.count} </p>
                                                    <p class="count"> قیمت واحد: ${product.unitPrice} </p>
                                                </div>
                                            </div> `;

        cartItemWrapper.append(productHTML);
    });
}

function removeFromCart(id, reload) {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    const itemToRemove = products.findIndex(x => x.id == id);
    products.splice(itemToRemove, 1);
    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    if (reload) {
        location.reload(true);
    } else {
        updateCart();
    }
}

function changeCartItemCount(id, totalId, count) {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    const productIndex = products.findIndex(x => x.id == id);
    products[productIndex].count = count;
    const product = products[productIndex];
    const newPrice = parseInt(product.unitPrice) * parseInt(count);
    $(`#${totalId}`).text(newPrice);
    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();

    const settings = {
        "url": "https://localhost:5001/api/inventory",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({
            "productId": id,
            "count": count
        }),
    };

    $.ajax(settings).done(function (data) {
        debugger;
        if (data.isStock == false) {
            const warningDiv = $('#productStockWarnings');
            if ($(`#${id}`).length == 0) {
                warningDiv.append(`
                            <div class="alert alert-warning" id="${id}">
                                <i class="fa fa-warning"></i>
                                کالای <strong>${data.productName}</strong> در انبار کمتر از تعداد درخواستی موجود است.
                            </div>
                `);
            }
        } else {
            if ($(`#${id}`).length > 0) {
                $(`#${id}`).remove();
            }
        }
    });
}