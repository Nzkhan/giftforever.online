/*  ---------------------------------------------------
    Template Name: Male Fashion
    Description: Male Fashion - ecommerce teplate
    Author: Colorib
    Author URI: https://www.colorib.com/
    Version: 1.0
    Created: Colorib
---------------------------------------------------------  */

'use strict';

(function ($) {

    // Helper to apply background images based on data-setbg, safe for dynamic DOM updates
    function applySetBg(scope) {
        var $scope = scope ? $(scope) : $(document);
        $scope.find('.set-bg').each(function () {
            var bg = $(this).attr('data-setbg');
            if (bg && bg.length) {
                $(this).css('background-image', 'url("' + String(bg).replace(/"/g, '\\"') + '")');
            }
        });
    }

    /*------------------
        Preloader
    --------------------*/
    $(window).on('load', function () {
        $(".loader").fadeOut();
        $("#preloder").delay(200).fadeOut("slow");

        /*------------------
            Gallery filter
        --------------------*/
        $('.filter__controls li').on('click', function () {
            $('.filter__controls li').removeClass('active');
            $(this).addClass('active');
        });
        var mixer = null;
        if ($('.product__filter').length > 0) {
            var containerEl = document.querySelector('.product__filter');
            mixer = mixitup(containerEl);
            if (containerEl && containerEl.addEventListener) {
                containerEl.addEventListener('mixEnd', function () {
                    applySetBg(containerEl);
                });
            }
        }

        // First pass after everything is laid out
        applySetBg(document);
    });

    // Also run on DOM ready in case of different loading orders
    $(function () {
        applySetBg(document);
    });

    /*------------------
        Background Set
    --------------------*/
    // Old one-time setter removed; handled by applySetBg

    //Search Switch
    $('.search-switch').on('click', function () {
        $('.search-model').fadeIn(400);
    });

    $('.search-close-switch').on('click', function () {
        $('.search-model').fadeOut(400, function () {
            $('#search-input').val('');
        });
    });

    /*------------------
		Navigation
	--------------------*/
    $(".mobile-menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    /*------------------
        Accordin Active
    --------------------*/
    $('.collapse').on('shown.bs.collapse', function () {
        $(this).prev().addClass('active');
    });

    $('.collapse').on('hidden.bs.collapse', function () {
        $(this).prev().removeClass('active');
    });

    //Canvas Menu
    $(".canvas__open").on('click', function () {
        $(".offcanvas-menu-wrapper").addClass("active");
        $(".offcanvas-menu-overlay").addClass("active");
    });

    $(".offcanvas-menu-overlay").on('click', function () {
        $(".offcanvas-menu-wrapper").removeClass("active");
        $(".offcanvas-menu-overlay").removeClass("active");
    });

    /*-----------------------
        Hero Slider
    ------------------------*/
    $(".hero__slider").owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: false,
        nav: true,
        navText: ["<span class='arrow_left'><span/>", "<span class='arrow_right'><span/>"] ,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true, // Enabled autoplay
        autoplayTimeout: 3000, // Auto-scroll every 3 seconds
        autoplayHoverPause: true
    });

    /*--------------------------
        Select
    ----------------------------*/
    $("select").niceSelect();

    /*-------------------
		Radio Btn
	--------------------- */
    $(".product__color__select label, .shop__sidebar__size label, .product__details__option__size label").on('click', function () {
        $(".product__color__select label, .shop__sidebar__size label, .product__details__option__size label").removeClass('active');
        $(this).addClass('active');
    });

    /*-------------------
		Scroll
	--------------------- */
    $(".nice-scroll").niceScroll({
        cursorcolor: "#0d0d0d",
        cursorwidth: "5px",
        background: "#e5e5e5",
        cursorborder: "",
        autohidemode: true,
        horizrailenabled: false
    });

    /*------------------
        CountDown
    --------------------*/
    // For demo preview start
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    if(mm == 12) {
        mm = '01';
        yyyy = yyyy + 1;
    } else {
        mm = parseInt(mm) + 1;
        mm = String(mm).padStart(2, '0');
    }
    var timerdate = mm + '/' + dd + '/' + yyyy;
    // For demo preview end


    // Uncomment below and use your date //

    /* var timerdate = "2020/12/30" */

    $("#countdown").countdown(timerdate, function (event) {
        $(this).html(event.strftime("<div class='cd-item'><span>%D</span> <p>Days</p> </div>" + "<div class='cd-item'><span>%H</span> <p>Hours</p> </div>" + "<div class='cd-item'><span>%M</span> <p>Minutes</p> </div>" + "<div class='cd-item'><span>%S</span> <p>Seconds</p> </div>"));
    });

    /*------------------
		Magnific
	--------------------*/
    $('.video-popup').magnificPopup({
        type: 'iframe'
    });

    /*-------------------
		Quantity change
	--------------------- */
    var proQty = $('.pro-qty');
    proQty.prepend('<span class="fa fa-angle-up dec qtybtn"></span>');
    proQty.append('<span class="fa fa-angle-down inc qtybtn"></span>');
    proQty.on('click', '.qtybtn', function () {
        var $button = $(this);
        var oldValue = $button.parent().find('input').val();
        var newVal = $button.hasClass('inc') ? parseFloat(oldValue) + 1 : Math.max(0, parseFloat(oldValue) - 1);
        $button.parent().find('input').val(newVal);
    });

    var proQty2 = $('.pro-qty-2');
    proQty2.prepend('<span class="fa fa-angle-left dec qtybtn" data-opp="subtract"></span>');
    proQty2.append('<span class="fa fa-angle-right inc qtybtn" data-opp="add"></span>');
    proQty2.on('click', '.qtybtn', function () {
        var $button = $(this);
        var $container = $button.closest('.pro-qty-2');
        var cartId = $container.data('cartid');
        var oldValue = $container.find('input').val();
        var opp = $button.data('opp');
        var newVal = parseFloat(oldValue);

        if ($button.hasClass('inc')) {
            newVal = newVal + 1;
            opp = 'add';
        } else {
            if (oldValue > 1) {
                newVal = newVal - 1;
                opp = 'subtract';
            } else {
                newVal = 1; // keep UI at 1; server may remove item if it hits 0
                opp = 'subtract';
            }
        }
        $container.find('input').val(newVal);

        if (cartId) {
            // Call server to update quantity
            $.ajax({
                type: 'POST',
                url: '/carts/changequantity',
                data: { opp: opp, cartid: cartId },
                success: function () {
                    window.location.reload();
                }
            });
        }
    });

    /*------------------
        Achieve Counter
    --------------------*/
    $('.cn_num').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });

    // Expose simple helpers to toggle global loading overlay
    window.showLoading = function () {
        $('#loading-overlay').fadeIn(150);
    };

    window.hideLoading = function () {
        $('#loading-overlay').fadeOut(150);
    };

    // Update cart counters in header
    window.updateCartIndicators = function () {
        $.ajax({
            type: 'POST',
            url: '/carts/getcartcount',
            data: {},
            success: function (data) {
                $('#cartcount').text(data);
            }
        });
        $.ajax({
            type: 'POST',
            url: '/carts/getcartamount',
            data: {},
            success: function (data) {
                $('#cartamount').text(data);
            }
        });
    };

    // Add-to-cart click handler for product cards
    $(document).on('click', '.add-cart', function (e) {
        e.preventDefault();
        var $btn = $(this);
        var prodId = $btn.data('prodid');
        if (!prodId) {
            var href = $btn.closest('.product__item').find('a[href*="/Products/Details"]').attr('href') || '';
            var match = href.match(/[?&]id=(\d+)/i);
            if (match && match[1]) {
                prodId = match[1];
            }
        }
        if (!prodId) return;

        $btn.addClass('disabled').css('pointer-events', 'none');

        $.ajax({
            type: 'POST',
            url: '/carts/addtocart',
            data: { prodid: prodId },
            success: function () {
                if (window.updateCartIndicators) updateCartIndicators();
                if ($.notify) {
                    $.notify('Product added to cart', { color: '#fff', background: '#20D67B' });
                }
            },
            error: function () {
                if ($.notify) {
                    $.notify('Unable to add to cart. Please try again.', { color: '#fff', background: '#D44950' });
                }
            },
            complete: function () {
                $btn.removeClass('disabled').css('pointer-events', '');
            }
        });
    });

})(jQuery);